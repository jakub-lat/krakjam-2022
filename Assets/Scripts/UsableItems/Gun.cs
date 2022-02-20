using System;
using DG.Tweening;
using Game;
using LetterBattle.Utility;
using Player;
using StarterAssets;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UsableItems
{
    public class Gun : UsableItem
    {
        [SerializeField] private float damage = 25;
        [SerializeField] private float headshotDamage = 50;
        [SerializeField] private float damageRandomness = 3f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float fireCooldown;
        [SerializeField] private float reloadDuration;
        [SerializeField] private int maxCurrentAmmo;
        [SerializeField] private int startingAmmo;


        [SerializeField] private string bulletholePoolingTag;
        [SerializeField] private string hitParticlePoolingTag;

        [SerializeField] private ParticleSystem fireParticles;

        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private Transform trailSpawnPoint;
        [SerializeField] private float trailSpeed = 50f;

        [SerializeField] private Color warningColorAmmo = Color.yellow;
        [SerializeField] private Color dangerColorAmmo = Color.red;
        [SerializeField] private Collider gunCollider;

        [SerializeField] private GunUI ui;

        [SerializeField]
        private AudioClip shoot;

        [SerializeField]
        private AudioClip reload;

        [SerializeField]
        private AudioClip pickup;
        
        [SerializeField]
        private AudioClip noAmmo = null;

        [SerializeField]
        private AudioSource gunSource = null;
        
        private float gunSourceOriginalVolume;
            
        private float trailDurationMultiplier => 10 / trailSpeed;


        private int currentAmmo;
        private int totalAmmo;

        private bool isCooldown = false;
        private float cooldownTimer = 0;

        private bool isReloading = false;
        private float reloadTimer = 0;

        private bool enableUpdate;

        public static Gun Current => HandController.Current.CurrentItem is Gun gun ? gun : null;

        private void Awake()
        {
            currentAmmo = maxCurrentAmmo;
            totalAmmo = startingAmmo - maxCurrentAmmo;
            
            gunSourceOriginalVolume = gunSource.volume;
        }

        private void ResetReload()
        {
            reloadTimer = reloadDuration;
            ReloadUI.Current.SetInfo(reloadTimer, reloadDuration);
        }

        private void Update()
        {
            if (!enableUpdate)
            {
                if (isReloading)
                {
                    ResetReload();
                    isReloading = false;
                }

                return;
            }

            ShootCooldown();

            if (isReloading)
            {
                ReloadTimer();
                ui?.SetInfo($"?? / {totalAmmo}");
                return;
            }

            // todo nie robić tego w update
            string first = currentAmmo.ToString();
            float ammo = ((float)currentAmmo / maxCurrentAmmo);
            if (ammo < 0.3f)
            {
                first = $"<color={ColorHelper.GetColorHex(dangerColorAmmo, true)}>{currentAmmo}</color>";
            }
            else if (ammo < 0.6f)
            {
                first = $"<color={ColorHelper.GetColorHex(warningColorAmmo, true)}>{currentAmmo}</color>";
            }

            ui?.SetInfo($"{first} / {totalAmmo}");
        }
        
        public override bool Use()
        {
            Shoot();
            return true;
        }

        public override void OnPickup()
        {
            base.OnPickup();
            enableUpdate = true;
            gunSource.PlayOneShot(pickup);
            gunCollider.isTrigger = true;
        }

        public override void OnDrop()
        {
            base.OnDrop();
            enableUpdate = false;
            gunCollider.isTrigger = false;
        }

        public void Reload()
        {
            if (currentAmmo < maxCurrentAmmo && totalAmmo > 0 && !isReloading)
            {
                gunSource.volume = gunSourceOriginalVolume;
                gunSource.pitch = 1;
                gunSource.PlayOneShot(reload);
                isReloading = true;
                ResetReload();
            }
        }

        public void Shoot()
        {
            if (PauseManager.Current.IsPaused)
            {
                return;
            }

            if (currentAmmo <= 0)
            {
                gunSource.PlayOneShot(noAmmo);
                return;
            }

            if (isCooldown || isReloading)
            {
                return;
            }
            
            PlayerAnim.Current.Shoot();

            isCooldown = true;
            cooldownTimer = fireCooldown;
            currentAmmo--;

            fireParticles.Play();
            gunSource.pitch = Random.Range(0.95F, 1.05F);
            gunSource.volume = Random.Range(0.3f, gunSourceOriginalVolume);
            gunSource.PlayOneShot(shoot);
 

            var trail = Instantiate(trailPrefab, trailSpawnPoint.position, Quaternion.identity);

            if (Physics.Raycast(CameraHelper.MainCamera.transform.position, CameraHelper.MainCamera.transform.forward,
                out var hit, float.PositiveInfinity, layerMask,
                QueryTriggerInteraction.Ignore))
            {
                // WE NEED END VALUE NOT DIRECTION
                trail.transform.DOMove(hit.point,
                        trailDurationMultiplier * Vector3.Distance(trail.transform.position, hit.point))
                    .SetLink(gameObject);

                EnemyDamageUtils.EnemyDamage(hit, damage, headshotDamage, damageRandomness);
                
                if (hit.collider.gameObject.CompareTag("Pipe"))
                {
                    if (hit.transform.GetComponent<Pipe>().Hit())
                    {

                        HitmarkManager.Current.GetNormalHit();
                        GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, hit.point,
                            Quaternion.LookRotation(hit.normal));
                        bulletHole.transform.parent = hit.collider.transform;
                    }
                }
                else
                {
                    GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                    bulletHole.transform.parent = hit.collider.transform;
                    
                    ObjectPooler.Current.SpawnPool("EnvironmentDamageParticle", hit.point,
                        Quaternion.Euler(hit.point - trailSpawnPoint.position));
                }
            }
        }

        private void ReloadFinal()
        {
            var prevAmmo = currentAmmo;
            currentAmmo = Math.Min(maxCurrentAmmo, totalAmmo);
            totalAmmo -= (currentAmmo - prevAmmo);
        }

        private void ShootCooldown()
        {
            if (isCooldown)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    isCooldown = false;
                    cooldownTimer = 0;
                }
            }
        }

        private void ReloadTimer()
        {
            reloadTimer -= Time.deltaTime;
            ReloadUI.Current.SetInfo(reloadTimer, reloadDuration);

            if (reloadTimer <= 0)
            {
                isReloading = false;
                reloadTimer = 0;
                ReloadFinal();
            }
        }

        public void AddAmmo(int amount)
        {
            totalAmmo += amount;
        }
    }
}
