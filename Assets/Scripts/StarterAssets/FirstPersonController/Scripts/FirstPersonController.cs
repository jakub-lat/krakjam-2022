using Cyberultimate.Unity;
using System;
using Player;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoSingleton<FirstPersonController>
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6.0f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        private bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Crouching")]
        [SerializeField]
        private float crouchedHeight = 0;
        private float defaultHeight;

        public float SlideSpeed = 10;

        [SerializeField]
        private float speedStartCrouch = 6;
        [SerializeField]
        private float speedEndCrouch = 8;

        [SerializeField]
        private float crouchSpeed;

        [SerializeField]
        private float howLongSlide;

        [SerializeField]
        private float slideForce;

        [SerializeField]
        private float waitForNextCrouch = 1;

        private float cooldownSlideValue;
        private float cooldownCrouchValue;


        private float _speed;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private ParticleSystem bars = null;

        private float targetSpeed;

        [SerializeField]
        private BasicRigidBodyPush push;
        private float normalPush;

        private void Start()
        {
            normalPush = push.strength;
            cooldownSlideValue = Time.time + howLongSlide;
            cooldownCrouchValue = Time.time + waitForNextCrouch;
            this.transform.eulerAngles = new Vector3(0, 90, 0);
            _controller = GetComponent<CharacterController>();
            defaultHeight = _controller.height;
            _input = GetComponent<StarterAssetsInputs>();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            bars = GameObject.FindGameObjectWithTag("Jojo").GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            Run();
            Crouch();
            ShowBarsEffect();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                PlayerAnim.Current.SetIsWalking(false);
            }
            else
            {
                PlayerAnim.Current.SetIsWalking(true);
            }


            float horizontalVelocity = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (horizontalVelocity < targetSpeed - speedOffset || horizontalVelocity > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(horizontalVelocity, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            if (_input.move != Vector2.zero)
            {
                inputDirection = new Vector3(CameraHelper.MainCamera.transform.forward.x *
                    _input.move.y, 0, CameraHelper.MainCamera.transform.forward.z * _input.move.y)
                     + CameraHelper.MainCamera.transform.right * _input.move.x;
            }

            _controller.Move((inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));
        }

        public Vector3 GetDirection()
        {
            return new Vector3(CameraHelper.MainCamera.transform.forward.x *
            _input.move.y, 0, CameraHelper.MainCamera.transform.forward.z * _input.move.y)
            + CameraHelper.MainCamera.transform.right * _input.move.x;
        }

        private void ShowBarsEffect()
        {
            if (_speed > SprintSpeed / 1.07f)
            {
                bars.Play();
            }

            else
            {
                bars.Stop();
            }
        }

        private void Run()
        {
            if (_input.sprint && !_input.crouch)
            {
                targetSpeed = SprintSpeed;
            }
        }


        private void Crouch()
        {
            if (_input.crouch)
            {
                if (_controller.height > crouchedHeight)
                {
                    _controller.height -= Time.deltaTime * speedStartCrouch;
                }

                // cooldown is over
                if (Time.time > cooldownCrouchValue)
                {
                    if (_speed > SprintSpeed / 2)
                    {
                        targetSpeed = SlideSpeed;
                        print("sliding!");
                        push.strength *= slideForce;

                        if ((Time.time > cooldownSlideValue))
                        {
                            print("stop slide");
                            targetSpeed = crouchSpeed;
                        }

                        cooldownCrouchValue = Time.time + waitForNextCrouch;
                    }

                    else
                    {
                        push.strength = normalPush;
                        cooldownSlideValue = Time.time + howLongSlide;
                        targetSpeed = crouchSpeed;
                    }
                }
            }

            else
            {
                push.strength = normalPush;
                targetSpeed = MoveSpeed;

                if (_controller.height < defaultHeight)
                {
                    _controller.height += Time.deltaTime * speedEndCrouch;
                }

                cooldownSlideValue = Time.time + howLongSlide;
            }

        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }
}
