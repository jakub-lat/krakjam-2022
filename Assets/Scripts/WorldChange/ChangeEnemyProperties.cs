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

    public float movingSpeed=3.5f;
    public float range = 5f;
}

[Serializable]
public class MeleeEnemyProperties
{
    public float attackSpeed = 0.5f;
    public float attackDelay = 0.1f;
    public float knockback = 1f;

    public float movingSpeed=3.5f;
    public float range = 5f;
}

public class ChangeEnemyProperties : WorldChangeLogic
{

    [SerializeField]
    private WorldTypeDict<ShootingEnemyProperties> shootingEnemyData;
    [SerializeField]
    private WorldTypeDict<MeleeEnemyProperties> meleeEnemyData;

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        UpdateEnemies(type);
    }

    public void UpdateEnemies(WorldTypeController.WorldType type)
    {
        var p = shootingEnemyData[type];
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
            se.range = p.range;

            NavMeshAgent n = g.GetComponent<NavMeshAgent>();
            n.speed = p.movingSpeed;
        }

        var p2 = meleeEnemyData[type];
        foreach (GameObject g in EnemySpawner.Current.meleeEnemies)
        {
            MeleeEnemy se = g.GetComponent<MeleeEnemy>();
            se.attackSpeed = p2.attackSpeed;
            se.attackDelay = p2.attackDelay;
            se.knockback = p2.knockback;
            se.range = p2.range;

            NavMeshAgent n = g.GetComponent<NavMeshAgent>();
            n.speed = p2.movingSpeed;
        }
    }
}
