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
}

public class ChangeEnemyProperties : WorldChangeLogic
{

    [SerializeField]
    private WorldTypeDict<ShootingEnemyProperties> shootingEnemyData;
    [SerializeField]
    private WorldTypeDict<MeleeEnemyProperties> meleeEnemyData;

    public static ChangeEnemyProperties instance;
    private void Awake()
    {
        instance = this;
    }

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        UpdateEnemies(type);
    }

    public void UpdateEnemies(WorldTypeController.WorldType type)
    {
        ShootingEnemyProperties p = shootingEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.shootingEnemies)
        {
            ShootingEnemy se = g.GetComponent<ShootingEnemy>();
            se.bulletPoolTag = p.bulletPoolTag;
            se.attackSpeed = p.attackSpeed;
            se.shootDelay = p.shootDelay;
            se.bulletSpeed = p.bulletSpeed;
            se.magazineSize = p.magazineSize;
            se.reloadSpeed = p.reloadSpeed;
            se.magazine = p.magazine;
            se.attackRange = p.attackRange;
            se.followRange = p.followRange;
            se.dispersion = p.dispersion;

            se.fleeRange = p.fleeRange;
            se.fleeMultiplier = p.fleeMultiplier;

            se.moveSpeed = p.moveSpeed;
            se.fleeSpeed = p.fleeSpeed;
            se.flee = p.flee;

            se.bulletDamage = p.damage;
        }

        MeleeEnemyProperties p2 = meleeEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.meleeEnemies)
        {
            Debug.Log(g);
            MeleeEnemy se = g.GetComponent<MeleeEnemy>();
            se.attackSpeed = p2.attackSpeed;
            se.attackDelay = p2.attackDelay;
            se.knockback = p2.knockback;
            se.attackRange = p2.attackRange;
            se.followRange = p2.followRange;
            se.fleeRange = p2.fleeRange;
            se.fleeMultiplier = p2.fleeMultiplier;

            se.moveSpeed = p2.moveSpeed;
            se.fleeSpeed = p2.fleeSpeed;
            se.flee = p2.flee;

            se.damage = p2.damage;
        }
    }
}
