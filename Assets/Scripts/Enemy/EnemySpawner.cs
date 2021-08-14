using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Time between each enemy entity spawning
    //[SerializeField]
    //float[] enemySpawnTimes;

    //Weight/Chance that each enemy spawns
    [SerializeField]
    float[] enemySpawnWeights;

    float totalWeight = 0;

    //Enemy prefabs corresponding to enemySpawnWeights' weights
    [SerializeField]
    GameObject[] enemyPrefabs;

    int currentEnemy = 0;


    bool runningCoroutine = false;

    int difficulty = 0;

    //Spawns enemies enemySpawnPositionOffset units higher than they should be, so the player can't camp the top part
    [SerializeField]
    float enemySpawnPositionOffset = 0.5f;

    [SerializeField]
    float baseEnemies = 5;
    [SerializeField]
    float enemiesPerDifficulty = 1.5f;
    [SerializeField]
    float baseTimePerEnemy = 15f;
    [SerializeField]
    float timePerEnemyDifficultyScaling = -0.5f;
    [SerializeField]
    float maxDiffMaxEnemies = 150;
    [SerializeField]
    float maxDifficultyTimePerEnemy = 2f;

    int maxEnemies = 0;
    float timePerEnemy = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyPrefabs.Length == 0 || enemySpawnWeights.Length == 0)
        {
            Debug.LogError("Enemy Spawner: Array is empty and shouldn't be.");
        }
        if (enemyPrefabs.Length != enemySpawnWeights.Length)
        {
            Debug.LogError("Enemy Spawner: Enemy prefabs and spawn weights arrays are different sizes.");
        }


        difficulty = PlayerPrefs.GetInt(DifficultySelect.difficulty);
           
        if (difficulty == 10)
        {
            //Only spawn first enemy (One that shoots linearly)
            totalWeight = enemySpawnWeights[0];
        }
        else
        {
            for (int i = 0; i < enemySpawnWeights.Length; i++)
            {
                totalWeight += enemySpawnWeights[i];
            }
        }



        if(difficulty == 10)
        {
            maxEnemies = (int) maxDiffMaxEnemies;
            timePerEnemy = maxDifficultyTimePerEnemy;
        }
        else
        {
            maxEnemies = (int)(difficulty * enemiesPerDifficulty + baseEnemies);
            timePerEnemy = (int)(difficulty * timePerEnemyDifficultyScaling + baseTimePerEnemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!runningCoroutine)
        {
            if(currentEnemy < maxEnemies)
            {
                StartCoroutine(EnemySpawningCoroutine());
            }
        }
    }

    protected IEnumerator EnemySpawningCoroutine()
    {
        runningCoroutine = true;
        yield return new WaitForSeconds(timePerEnemy);
        SpawnEnemy();
        currentEnemy++;
        runningCoroutine = false;
    }

    //Spawns a random enemy using the enemy spawn weight array
    protected void SpawnEnemy()
    {
        float randNum = Random.Range(0, (float) totalWeight);

        float currentWeightSum = 0;

        for (int i = 0; i < enemySpawnWeights.Length; i++)
        {
            currentWeightSum += enemySpawnWeights[i];
            if (currentWeightSum >= randNum)
            {
                SpawnEnemy(enemyPrefabs[i]);
                break;
            }
        }
    }

    protected void SpawnEnemy(GameObject enemyPref)
    {
        float yPos = Random.Range(DisappearingBlockController.minY, DisappearingBlockController.maxY);
        yPos += enemySpawnPositionOffset;

        int a = 1;
        if (difficulty == 10)
        {
            a = Random.Range(1, 2);
        }
        else
        {
            a = Random.Range(1, 3);
        }
        float xPos = 8;
        if (a == 1)
        {
            xPos *= -1;
            Instantiate(enemyPref, new Vector3(xPos, yPos, enemyPref.transform.position.z), Quaternion.identity);
        }
        else
        {
            GameObject enemy = Instantiate(enemyPref, new Vector3(xPos, yPos, enemyPref.transform.position.z), Quaternion.identity);
            enemy.GetComponentInChildren<ShootBullet>().Flip();
        }

    }
}
