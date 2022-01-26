using System;
using Cyberultimate.Unity;
using TMPro;
using UnityEngine;

namespace KrakJam2022.Player
{
    class GunController : MonoSingleton<GunController>
    {
        // todo lepsze nazwy zmiennych
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float fireCooldown;
        [SerializeField] private float reloadDuration;
        [SerializeField] private int maxCurrentAmmo;

        [SerializeField] private TMP_Text infoText;

        [SerializeField] private GameObject bulletHolePrefab;
        
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
            infoText.text = $"ammo: {currentAmmo} / {totalAmmo} | isReloading: {isReloading} | isCooldown: {isCooldown}";
        }

        public void OnFire()
        {
            if (!isCooldown && currentAmmo > 0 && !isReloading)
            {
                Shoot();
            }
        }

        public void OnReload()
        {
            if (currentAmmo < maxCurrentAmmo && !isReloading)
            {
                isReloading = true;
                reloadTimer = reloadDuration;
            }
        }

        private void Shoot()
        {
            isCooldown = true;
            cooldownTimer = fireCooldown;
            currentAmmo--;
            
            if (Physics.Raycast(transform.position, transform.forward, out var hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    // todo
                }
                else
                {
                    var bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    bulletHole.transform.parent = hit.collider.transform;
                }
            }
        }

        private void Reload()
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
            if (reloadTimer <= 0)
            {
                isReloading = false;
                reloadTimer = 0;
                Reload();
            }
        }

        public void AddAmmo(int amount)
        {
            totalAmmo += amount;
        }
    }
}
