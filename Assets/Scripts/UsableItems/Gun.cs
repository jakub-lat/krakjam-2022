using System;
using DG.Tweening;
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


        [SerializeField] private string bulletholePoolingTag;
        [SerializeField] private string hitParticlePoolingTag;

        [SerializeField] private ParticleSystem fireParticles;

        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private Transform trailSpawnPoint;
        [SerializeField] private float trailSpeed = 50f;

        [SerializeField] private Color warningColorAmmo = Color.yellow;
        [SerializeField] private Color dangerColorAmmo = Color.red;
        [SerializeField] private Collider gunCollider;

        [SerializeField]
        private AudioClip shoot;

        [SerializeField]
        private AudioClip reload;

        [SerializeField]
        private AudioClip pickup;

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

        public static Gun Current => HandController.Current.CurrentItem is Gun gun ? gun : null;

        private void Awake()
        {
            currentAmmo = maxCurrentAmmo;
            totalAmmo = maxCurrentAmmo * 10;
            
            gunSourceOriginalVolume = gunSource.volume;
        }

        private void Update()
        {
            ShootCooldown();

            if (isReloading)
            {
                ReloadTimer();
                if (GunUI.Current) GunUI.Current.SetInfo($"?? / {totalAmmo}");
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
            if (GunUI.Current) GunUI.Current.SetInfo($"{first} / {totalAmmo}");
        }
        
        public override void Use()
        {
            Shoot();
        }

        public override void OnPickup()
        {
            base.OnPickup();
            gunSource.PlayOneShot(pickup);
            gunCollider.isTrigger = true;
        }

        public override void OnDrop()
        {
            base.OnDrop();
            gunCollider.isTrigger = false;
        }

        public void Reload()
        {
            if (currentAmmo < maxCurrentAmmo && !isReloading)
            {
                gunSource.volume = gunSourceOriginalVolume;
                gunSource.pitch = 1;
                gunSource.PlayOneShot(reload);
                isReloading = true;
                reloadTimer = reloadDuration;
            }
        }

        public void Shoot()
        {
            if (isCooldown || currentAmmo <= 0 || isReloading) return;

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

                var damageRandom = Random.Range(-damageRandomness, damageRandomness);

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    var dmg = damage + damageRandom;

                    if(hit.transform.GetComponentInParent<Enemy>()) //its a normal enemy
                    {
                        Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                        enemy.GotHit(dmg);
                    } else if (hit.transform.GetComponentInParent<Boss>()) //its a boss
                    {
                        Boss boss = hit.transform.GetComponentInParent<Boss>();
                        boss.GotHit(dmg);
                    } else
                    {
                        Debug.LogError("Enemy doesnt have a enemy or boss component");
                    }
                    
                    HitmarkManager.Current.GetNormalHit();
                    PopupManager.Current.SpawnStandardDamage(hit.transform, (int)dmg);
                    GameObject particle = ObjectPooler.Current.SpawnPool(hitParticlePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                }
                else if (hit.collider.gameObject.CompareTag("EnemyHead"))
                {
                    var dmg = headshotDamage + damageRandom;


                    if (hit.transform.GetComponentInParent<Enemy>()) //its a normal enemy
                    {
                        Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                        enemy.GotHit(dmg);
                    }
                    else if (hit.transform.GetComponentInParent<Boss>()) //its a boss
                    {
                        Boss boss = hit.transform.GetComponentInParent<Boss>();
                        boss.GotHit(dmg);
                    }
                    else
                    {
                        Debug.LogError("Enemy doesnt have a enemy or boss component");
                    }

                    PopupManager.Current.SpawnHeadshotDamage(hit.transform, (int)dmg);
                    HitmarkManager.Current.GetHeadshotHit();
                    GameObject particle = ObjectPooler.Current.SpawnPool(hitParticlePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                    Scoreboard.GameScoreboard.Current.levelData.headshots++;
                }
                else
                {
                    GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                    bulletHole.transform.parent = hit.collider.transform;
                }
            }
        }

        private void ReloadFinal()
        {
            currentAmmo = Math.Min(maxCurrentAmmo, totalAmmo);
            totalAmmo -= currentAmmo;
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
