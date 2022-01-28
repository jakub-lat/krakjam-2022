using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEnemies : MonoBehaviour
{
    public int shootingEnemies = 5;
    public int meleeEnemies = 0;

    public float diffX = 2f;
    public float diffZ = 2f;

    private void Start()
    {
        transform.position = Game.LevelManager.Current.startingPosA.position;
        Spawn();
        
    }

    public void Spawn() { 
        for(int i=0;i<shootingEnemies;i++)
        {        
            Vector3 pos = new Vector3(transform.position.x + Random.Range(diffX, diffX), transform.position.y, transform.position.z + Random.Range(-diffZ, diffZ));
            EnemySpawner.Current.Spawn(pos, transform.rotation, EnemySpawner.EnemyType.Shooting);
        }

        for (int i = 0; i < meleeEnemies; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(diffX, diffX), transform.position.y, transform.position.z + Random.Range(-diffZ, diffZ));
            EnemySpawner.Current.Spawn(pos, transform.rotation, EnemySpawner.EnemyType.Melee);
        }

        ChangeEnemyProperties.instance.UpdateEnemies(WorldTypeController.WorldType.Normal);
    }
}
