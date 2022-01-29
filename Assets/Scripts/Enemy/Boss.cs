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
    private bool punching = false;

    public float startingHealth = 600;
    public Image healthBar;
    public Canvas BossUI;
    public Transform gunRotate;
    public Transform gunPoint;
    public CharacterController playerCharacter;

    private Animator anim;
    private Transform player;

    [Header("Pipes")]
    public int pipeOverHealth = 3;
    public float pipeDmg = 150;


    [Header("Shooting")]
    public string bulletPoolTag = "EnemyBullet";
    public int burstSizeMin = 30;
    public int burstSizeMax = 45;
    public float damage = 10f;
    public float burstFireRate = 0.05f;
    public float fireRate = 2f;
    public float dispersion = 40;
    public float disperionY = 20;
    public float bulletSpeed = 300f;

    public bool shootInFront = true;
    public float inFrontMultiplierMin = 0.7f;
    public float inFrontMultiplierMax = 1.3f;

    [Header("Punch")]
    public float punchDamage=20;
    public float punchKnock = 5;
    public float rangeToPunch=5;
    public MeleeAttack attack;

    [SerializeField]
    private AudioSource bossSource;

    public AudioSource BossSource => bossSource;


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
        anim = GetComponent<Animator>();
        attack.damage = punchDamage;
        attack.knockback = punchKnock;
        attack.myPos = transform;
        healthToNextPipe = startingHealth - (startingHealth / (pipeOverHealth + 1));
    }

    private int currBurst = 0;
    private float burstTimer = 0;
    private float shootingTimer = 0;
    private bool pipeAnim = false;
    private void Update()
    {
        if (dead || !battle || punching || pipeAnim) return;

        var dist = Vector3.Distance(transform.position, player.position);
        if (dist <= rangeToPunch)
        {
            punching = true;
            Punch();
            return;
        }

        var look = playerCharacter.transform.position;
        gunRotate.transform.LookAt(look);
        look.y = transform.position.y;
        transform.LookAt(look);

        if (currBurst <= 0)
        {
            anim.SetBool("Shooting", false);
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0)
            {
                burstTimer = fireRate;
                currBurst = Random.Range(burstSizeMin,burstSizeMax);
                shootingTimer = fireRate;
            }
        } else
        {
            anim.SetBool("Shooting", true);
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                shootingTimer = burstFireRate;
                currBurst--;
                Shoot();
            }
        }
    }

    float healthToNextPipe;
    public void GotHit(float amount)
    {
        if (dead || !battle) return;

        health -= amount;
        if (health <= 0)
        {
            dead = true;
            health = 0;
            healthBar.fillAmount = health / startingHealth;
            return;
        }

        while (health < healthToNextPipe)
        {
            healthToNextPipe -= startingHealth / (pipeOverHealth + 1);
            BossPipe.Current.NextPipe();
        }
        healthBar.fillAmount = health/ startingHealth;
    }

    public void PipeHit()
    {
        Debug.Log("Pipee");
        pipeAnim = true;
        anim.Play("PipeBroken");
    }

    public void EndPipe()
    {
        GotHit(pipeDmg);
        pipeAnim = false;
    }

    void Punch()
    {
        var look = playerCharacter.transform.position;
        look.y = transform.position.y;
        transform.LookAt(look);

        burstTimer = fireRate;
        shootingTimer = burstFireRate;
        currBurst = Random.Range(burstSizeMin, burstSizeMax); ;
        anim.Play("Punch");
        attack.attacking = true;
    }

    public void EndPunching()
    {
        punching = false;
        attack.attacking = false;
    }

    void Shoot()
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(bulletPoolTag, gunPoint.position, Quaternion.identity);

        var vel = playerCharacter.velocity;
        Vector3 inFront = player.position + vel * Random.Range(inFrontMultiplierMin,inFrontMultiplierMax) * Vector3.Distance(transform.position,player.position)/20;

        
        Vector3 dir = ((shootInFront ? inFront : player.position) - gunPoint.position).normalized * bulletSpeed;
        Vector3 dispDir = new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-disperionY, disperionY), Random.Range(-dispersion, dispersion));


        obj.transform.forward = dir+dispDir;
        obj.GetComponent<Rigidbody>().AddForce(dir+dispDir);
        obj.GetComponent<EnemyBullet>().damage = damage;
    }
}
