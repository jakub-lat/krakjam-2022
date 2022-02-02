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
    public float health = 100f;
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
    public float health = 100f;
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
        se.attackSpeed = sd.attackSpeed + (sd.attackSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.attackSpeedM);
        se.shootDelay = sd.shootDelay;
        se.bulletSpeed = sd.bulletSpeed;
        se.magazineSize = sd.magazineSize;
        se.reloadSpeed = sd.reloadSpeed;
        se.magazine = sd.magazine;
        se.attackRange = sd.attackRange + (sd.attackRange * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.attackRangeM);
        se.followRange = sd.followRange;
        se.dispersion = sd.dispersion;

        se.fleeRange = sd.fleeRange;
        se.fleeMultiplier = sd.fleeMultiplier;

        se.moveSpeed = sd.moveSpeed + (sd.moveSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.speedM);
        se.fleeSpeed = sd.fleeSpeed + (sd.moveSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.speedM);
        se.flee = sd.flee;

        se.bulletDamage = sd.damage + (sd.damage * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.damageM);

        var e = g.GetComponent<Enemy>();
        e.startingHealth = sd.health + (sd.health * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.healthM); ;
        e.HPrefresh();
    }

    public void UpdateEnemies(WorldTypeController.WorldType type)
    {
        ShootingEnemyProperties sd = shootingEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.shootingEnemies)
        {
            if (g == null)
            {
                print($"{g.name} was null, that's weird...");
            }
            SetShootingData(g, sd);
        }

        MeleeEnemyProperties md = meleeEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.meleeEnemies)
        {
            g.GetComponent<ShootingEnemy>().isEnabled = md.shooting;
            g.GetComponent<MeleeEnemy>().isEnabled = !md.shooting;

            if (md.shooting)
            {
                SetShootingData(g, meleeShootingModeData);
            } else
            {
                MeleeEnemy se = g.GetComponent<MeleeEnemy>();
                se.attackSpeed = md.attackSpeed + (md.attackSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.attackSpeedM);
                se.attackDelay = md.attackDelay;
                se.knockback = md.knockback;
                se.attackRange = md.attackRange;
                se.followRange = md.followRange;
                se.fleeRange = md.fleeRange;
                se.fleeMultiplier = md.fleeMultiplier;

                se.moveSpeed = md.moveSpeed + (md.moveSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.speedM);
                se.fleeSpeed = md.fleeSpeed + (md.moveSpeed * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.speedM); 
                se.flee = md.flee;
                

                se.damage = md.damage + (md.damage * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.damageM);
                se.meleeEnemyScript.damage = se.damage;
                se.meleeEnemyScript.knockback = se.knockback;
                var e = g.GetComponent<Enemy>();
                e.startingHealth = md.health + (md.health * Game.LevelManager.Current.Difficulty * Game.LevelManager.Current.shootingEnemy.healthM); ;
                e.HPrefresh();
            }
        }
    }
}
