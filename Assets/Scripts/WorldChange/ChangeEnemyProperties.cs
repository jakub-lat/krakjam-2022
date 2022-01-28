using System;
using Cyberultimate.Unity;
using Player;
using UnityEngine;
using WorldChange;
using UnityEngine.AI;

[Serializable]
public class ShootingEnemyProperties
{
    public string bulletPoolTag;

    public float attackSpeed = 0.5f;
    public float shootDelay = 0.1f;
    public float bulletSpeed = 32f;
    public int magazineSize = 6;
    public float reloadSpeed = 3f;
    public bool magazine = true;
    public float dispersion = 1f;

    public float attackRange = 10f;
    public float followRange = 30f;
    public float fleeRange = 0f;
    public float fleeMultiplier = 5f;
    public float moveSpeed = 3f;
    public float fleeSpeed = 5f;
    public bool flee=false;
    public float damage = 10f;
}

[Serializable]
public class MeleeEnemyProperties
{
    public float attackSpeed = 0.5f;
    public float attackDelay = 0.1f;
    public float knockback = 1f;

    public float attackRange = 5f;
    public float followRange = 30f;
    public float fleeRange = 0f;
    public float fleeMultiplier = 5f;
    public float moveSpeed = 3f;
    public float fleeSpeed = 5f;
    public bool flee = false;
    public float damage =10f;

    public bool shooting = false;
}

public class ChangeEnemyProperties : WorldChangeLogic
{

    [SerializeField]
    private WorldTypeDict<ShootingEnemyProperties> shootingEnemyData;
    [SerializeField]
    private WorldTypeDict<MeleeEnemyProperties> meleeEnemyData;
    [SerializeField]
    private ShootingEnemyProperties meleeShootingModeData;

    public static ChangeEnemyProperties instance;
    private void Awake()
    {
        instance = this;
    }

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        UpdateEnemies(type);
    }

    void SetShootingData(GameObject g, ShootingEnemyProperties sd)
    {
        ShootingEnemy se = g.GetComponent<ShootingEnemy>();
        se.bulletPoolTag = sd.bulletPoolTag;
        se.attackSpeed = sd.attackSpeed;
        se.shootDelay = sd.shootDelay;
        se.bulletSpeed = sd.bulletSpeed;
        se.magazineSize = sd.magazineSize;
        se.reloadSpeed = sd.reloadSpeed;
        se.magazine = sd.magazine;
        se.attackRange = sd.attackRange;
        se.followRange = sd.followRange;
        se.dispersion = sd.dispersion;

        se.fleeRange = sd.fleeRange;
        se.fleeMultiplier = sd.fleeMultiplier;

        se.moveSpeed = sd.moveSpeed;
        se.fleeSpeed = sd.fleeSpeed;
        se.flee = sd.flee;

        se.bulletDamage = sd.damage;
    }

    public void UpdateEnemies(WorldTypeController.WorldType type)
    {
        ShootingEnemyProperties sd = shootingEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.shootingEnemies)
        {
            SetShootingData(g, sd);
        }

        MeleeEnemyProperties md = meleeEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.meleeEnemies)
        {
            g.GetComponent<ShootingEnemy>().enabled = md.shooting;
            g.GetComponent<MeleeEnemy>().enabled = !md.shooting;

            if (md.shooting)
            {
                SetShootingData(g, meleeShootingModeData);
            } else
            {
                MeleeEnemy se = g.GetComponent<MeleeEnemy>();
                se.attackSpeed = md.attackSpeed;
                se.attackDelay = md.attackDelay;
                se.knockback = md.knockback;
                se.attackRange = md.attackRange;
                se.followRange = md.followRange;
                se.fleeRange = md.fleeRange;
                se.fleeMultiplier = md.fleeMultiplier;

                se.moveSpeed = md.moveSpeed;
                se.fleeSpeed = md.fleeSpeed;
                se.flee = md.flee;

                se.damage = md.damage;
            }
        }
    }
}
