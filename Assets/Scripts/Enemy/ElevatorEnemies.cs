using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEnemies : MonoBehaviour
{
    private int shootingEnemies = 5;
    private int meleeEnemies = 0;

    public float diffX = 2f;
    public float diffZ = 2f;

    public float minDistToSpawn;

    Transform pl;

    private void Start()
    {
        transform.position = Game.LevelManager.Current.startingPosA.position;

        var difficulty = Game.LevelManager.Current.Difficulty;
        var curvePoint = Game.LevelManager.Current.CurrentLevel / Game.LevelManager.Current.levelCount;

        shootingEnemies =(int)( EnemySpawner.Current.elevatorShootingEnemyCurve.Evaluate(curvePoint)*difficulty);
        meleeEnemies = (int)(EnemySpawner.Current.elevatorMeleeEnemyCurve.Evaluate(curvePoint) * difficulty);

        pl = PlayerInstance.Current.transform;

        spawned = false;

        Invoke(nameof(TrySpawn),0.01f);
        
    }

    bool spawned = false;

    void TrySpawn()
    {
        if (spawned) return;
        if(Vector3.Distance(pl.position,transform.position)< minDistToSpawn)
        {
            Invoke(nameof(TrySpawn), 0.1f);
            return;
        }

        spawned = true;
        ElevatorSpawn();
    }

    public void ElevatorSpawn()
    {
        Game.LevelManager.Current.startingElevator.Open();
        Invoke(nameof(Spawn), 0.1f);
        Invoke(nameof(CloseElevator), 5f);
    }

    public void Spawn() { 

        for(int i=0;i<shootingEnemies;i++)
        {        
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-diffX, diffX), transform.position.y, transform.position.z + Random.Range(-diffZ, diffZ));
            EnemySpawner.Current.Spawn(pos, transform.rotation, EnemySpawner.EnemyType.Shooting);
        }

        for (int i = 0; i < meleeEnemies; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-diffX, diffX), transform.position.y, transform.position.z + Random.Range(-diffZ, diffZ));
            EnemySpawner.Current.Spawn(pos, transform.rotation, EnemySpawner.EnemyType.Melee);
        }

        ChangeEnemyProperties.instance.UpdateEnemies(WorldTypeController.Current.CurrentWorldType);
    }

    void CloseElevator()
    {
        Game.LevelManager.Current.startingElevator.Close();
    }
}
