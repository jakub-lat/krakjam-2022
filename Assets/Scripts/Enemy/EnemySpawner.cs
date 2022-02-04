using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public enum EnemyType { Shooting, Melee }


    [Header("Balance")]
    public AnimationCurve shootingEnemyCurve;
    public AnimationCurve meleeEnemyCurve;
    public AnimationCurve elevatorShootingEnemyCurve;
    public AnimationCurve elevatorMeleeEnemyCurve;

    [Header("Spawning")]
    public float posY = 2;
    public float minRangeFromPlayer = 30f;

    public float elevatorEnemiesTime = 5f;
    public GameObject elevatorEnemies;

    [Header("Enemy Prefabs")]
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;

    [HideInInspector] public List<GameObject> shootingEnemies;
    [HideInInspector] public List<GameObject> meleeEnemies;

    private ChangeEnemyProperties changeProps;
    private List<Vector3> spawnpoints;
    private float elevatorEnemiesTimer= 10;

    private int shootingEnemyAmount = 2;
    private int meleeEnemyAmount = 0;
    private int elevatorShootingEnemyAmount = 2;
    private int elevatorMeleeEnemyAmount = 1;

    public void Spawn(Vector3 pos, Quaternion rot, EnemyType et)
    {
        switch (et)
        {
            case EnemyType.Shooting:
                shootingEnemies.Add(Instantiate(shootingEnemy, pos, rot,transform));
                break;
            case EnemyType.Melee:
                meleeEnemies.Add(Instantiate(meleeEnemy, pos, rot, transform));
                break;
        }
    }

    bool spawnedElevator = true;
    private void Update()
    {
        if (spawnedElevator) return;

        elevatorEnemiesTimer -= Time.deltaTime;
        if (elevatorEnemiesTimer <= 0)
        {
            Instantiate(elevatorEnemies, transform);
            spawnedElevator = true;
        }
    }

    public void StartSpawning()
    {

        spawnedElevator = false;
        elevatorEnemiesTimer = elevatorEnemiesTime;

        if (spawnpoints.Count <= 0) { Debug.LogError("No enemy spawnpoints set!"); return; }

        for(int i = 0; i < shootingEnemyAmount; i++)
        {
            Vector3 t = spawnpoints[Random.Range(0, spawnpoints.Count)];
            Spawn(t, Quaternion.identity, EnemyType.Shooting);
        }

        for (int i = 0; i < meleeEnemyAmount; i++)
        {
            Vector3 t = spawnpoints[Random.Range(0, spawnpoints.Count)];
            Spawn(t, Quaternion.identity, EnemyType.Melee);
        }
    }

    private void Start()
    {
        changeProps = GetComponent<ChangeEnemyProperties>();
        changeProps.UpdateEnemies(WorldTypeController.WorldType.Normal);
    }

    public void SetupSpawners(Vector3 pos, float width, float height, float spaceX, float spaceZ, int amount)
    {
        var difficulty = Game.LevelManager.Current.Difficulty;
        var curvePoint = Game.LevelManager.Current.CurrentLevel / Game.LevelManager.Current.levelCount;

        shootingEnemyAmount = (int)( difficulty * shootingEnemyCurve.Evaluate(curvePoint));
        meleeEnemyAmount = (int)(difficulty * meleeEnemyCurve.Evaluate(curvePoint));

        spawnpoints = new List<Vector3>();
        for(int i = 0; i < amount; i++)
        {
            float x = Random.Range(0, width * 4) * spaceX + pos.x;
            float z = Random.Range(0, height * 2) * spaceZ + pos.z;

            Vector3 npos = new Vector3(x, pos.y + posY, z);

            while (Vector3.Distance(npos, PlayerInstance.Current.transform.position) <= minRangeFromPlayer)
            {
                x = Random.Range(0, width * 4) * spaceX + pos.x;
                z = Random.Range(0, height * 2) * spaceZ + pos.z;

                npos = new Vector3(x, pos.y + posY, z);
            }

            spawnpoints.Add(npos);
        }
    }

    public void KillAll()
    {
        transform.KillAllChildren();
        shootingEnemies = new List<GameObject>();
        meleeEnemies = new List<GameObject>();
    }
}
