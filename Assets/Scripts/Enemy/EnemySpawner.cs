using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum EnemyType { Shooting, Melee }
    [Header("Start Spawning")]
    public List<Transform> spawnpoints;
    public int shootingEnemyAmount = 2;
    public int meleeEnemyAmount = 0;

    [Header("Enemy Prefabs")]
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;

    public void Spawn(Vector3 pos, Quaternion rot, EnemyType et)
    {
        switch (et)
        {
            case EnemyType.Shooting:
                Instantiate(shootingEnemy, pos, rot);
                break;
            case EnemyType.Melee:
                Instantiate(meleeEnemy, pos, rot);
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
    }
}
