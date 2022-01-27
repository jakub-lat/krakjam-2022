using System;
using Cyberultimate.Unity;
using DG.Tweening;
using UI;
using UnityEngine;

namespace Player
{
    class GunController : MonoSingleton<GunController>
    {
        // todo lepsze nazwy zmiennych
        [SerializeField] private float damage = 25;
        [SerializeField] private float headshotDamage = 50;
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
        private float trailDurationMultiplier => 10 / trailSpeed;


        private int currentAmmo;
        private int totalAmmo;

        private bool isCooldown = false;
        private float cooldownTimer = 0;

        private bool isReloading = false;
        private float reloadTimer = 0;

        protected override void Awake()
        {
            currentAmmo = maxCurrentAmmo;
            totalAmmo = maxCurrentAmmo * 10;
            base.Awake();
        }

        public void Update()
        {
            ShootCooldown();

            if (isReloading)
            {
                ReloadTimer();
            }

            // todo nie robić tego w update
            if (GunUI.Current) GunUI.Current.SetInfo($"{currentAmmo} / {totalAmmo}");
        }

        public void Reload()
        {
            if (currentAmmo < maxCurrentAmmo && !isReloading)
            {
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

            var trail = Instantiate(trailPrefab, trailSpawnPoint.position, Quaternion.identity);

            if (Physics.Raycast(transform.position, transform.forward, out var hit))
            {
                trail.transform.DOMove(hit.point, trailDurationMultiplier * Vector3.Distance(trail.transform.position, hit.point))
                    .SetLink(gameObject);
                    
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<Enemy>().GotHit(damage);
                    HitmarkManager.Current.GetNormalHit();
                    GameObject particle = ObjectPooler.Current.SpawnPool(hitParticlePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                }
                else if (hit.collider.gameObject.CompareTag("EnemyHead"))
                {
                    print("BOOM! HEADSHOT!");
                    hit.collider.transform.parent.GetComponent<Enemy>().GotHit(headshotDamage);
                    HitmarkManager.Current.GetHeadshotHit();
                    GameObject particle = ObjectPooler.Current.SpawnPool(hitParticlePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    GameObject bulletHole = ObjectPooler.Current.SpawnPool(bulletholePoolingTag, hit.point,
                        Quaternion.LookRotation(hit.normal));
                    bulletHole.transform.parent = hit.collider.transform;
                }

            }
            else
            {
                trail.transform.DOMove(transform.forward * 40f, trailDurationMultiplier * 40f).SetLink(gameObject);
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
