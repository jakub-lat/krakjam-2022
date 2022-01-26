using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public enum EnemyType { Shooting, Melee }
    [Header("Start Spawning")]
    public List<Transform> spawnpoints;
    public int shootingEnemyAmount = 2;
    public int meleeEnemyAmount = 0;

    [Header("Enemy Prefabs")]
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;

    [HideInInspector] public List<GameObject> shootingEnemies;
    [HideInInspector] public List<GameObject> meleeEnemies;

    private ChangeEnemyProperties changeProps;

    public void Spawn(Vector3 pos, Quaternion rot, EnemyType et)
    {
        switch (et)
        {
            case EnemyType.Shooting:
                shootingEnemies.Add(Instantiate(shootingEnemy, pos, rot));
                break;
            case EnemyType.Melee:
                meleeEnemies.Add(Instantiate(meleeEnemy, pos, rot));
                break;
        }
    }

    public void StartSpawning()
    {
        if (spawnpoints.Count <= 0) { Debug.LogError("No enemy spawnpoints set!"); return; }

        for(int i = 0; i < shootingEnemyAmount; i++)
        {
            Transform t = spawnpoints[Random.Range(0, spawnpoints.Count)];
            Spawn(t.position, t.rotation, EnemyType.Shooting);
        }

        for (int i = 0; i < meleeEnemyAmount; i++)
        {
            Transform t = spawnpoints[Random.Range(0, spawnpoints.Count)];
            Spawn(t.position, t.rotation, EnemyType.Melee);
        }
    }

    private void Start()
    {
        StartSpawning(); //to change
        changeProps = GetComponent<ChangeEnemyProperties>();
        changeProps.UpdateEnemies(Player.WorldTypeController.WorldType.Normal);
    }
}
