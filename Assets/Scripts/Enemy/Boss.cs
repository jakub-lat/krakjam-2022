using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoSingleton<Boss>
{
    private bool battle = false;
    private bool dead = false;
    private float health;

    public float startingHealth = 600;
    public Image healthBar;
    public Canvas BossUI;
    public Transform gunRotate;
    public Transform gunPoint;
    public CharacterController playerCharacter;

    private Transform player;


    [Header("Shooting")]
    public string bulletPoolTag = "EnemyBullet";
    public int burstSize = 15;
    public float damage = 10f;
    public float burstFireRate = 0.05f;
    public float fireRate = 2f;
    public float dispersion = 40;
    public float bulletSpeed = 300f;

    public bool shootInFront = true;
    public float inFrontMultiplier = 1f;
    

    public void StartBattle()
    {
        health = startingHealth;
        dead = false;
        BossUI.enabled = true;
        healthBar.fillAmount = startingHealth / health;
        EnemySpawner.Current.KillAll();

        battle = true;
    }

    private void Start()
    {
        BossUI.enabled = false;
        player = PlayerInstance.Current.transform;
    }

    private int currBurst = 0;
    private float burstTimer = 0;
    private float shootingTimer = 0;
    private void Update()
    {
        if (dead || !battle) return;

        var look = playerCharacter.transform.position;
        gunRotate.transform.LookAt(look);
        look.y = transform.position.y;
        transform.LookAt(look);

        if (currBurst <= 0)
        {
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0)
            {
                burstTimer = fireRate;
                currBurst = burstSize;
                shootingTimer = fireRate;
            }
        } else
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                shootingTimer = burstFireRate;
                currBurst--;
                Shoot();
            }
        }
    }

    public void GotHit(float amount)
    {
        if (dead || !battle) return;

        health -= amount;
        if (health <= 0)
        {
            dead = true;
            health = 0;
        }

        healthBar.fillAmount = health/ startingHealth;
    }

    void Shoot()
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(bulletPoolTag, gunPoint.position, Quaternion.identity);

        var vel = playerCharacter.velocity;
        Vector3 inFront = player.position + vel * inFrontMultiplier;

        
        Vector3 dir = ((shootInFront ? inFront : player.position) - gunPoint.position).normalized * bulletSpeed;
        Vector3 dispDir = new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion));


        obj.transform.forward = dir+dispDir;
        obj.GetComponent<Rigidbody>().AddForce(dir+dispDir);
        obj.GetComponent<EnemyBullet>().damage = damage;
    }
}
