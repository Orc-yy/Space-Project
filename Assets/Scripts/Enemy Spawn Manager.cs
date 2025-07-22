using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject bossPrefab;

    public Transform[] enemySpawnPoints;

    public float enemySpawnDelay;
    private float currentEnemySpawnDelay;

    public int currentStage = 1;
    public int maxStage = 10;
    public int maxAliveEnemies = 10;

    private int aliveEnemies = 0; // 현재 스테이지에 살아있는 적의 수
    private int enemySpawnedThisStage = 0; // 해당 스테이지에서 생성된 적의 수
    private int enemyToSpawnThisStage = 0; // 해당 스테이지에 생성할 적의 수

    private List<GameObject> currentStageEnemies = new List<GameObject>();

    public bool isPlayerAlive = true;

    void Start()
    {
        if (enemySpawnPoints == null || enemySpawnPoints.Length == 0)
        {
            int childCount = transform.childCount;
            enemySpawnPoints = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                enemySpawnPoints[i] = transform.GetChild(i);
            }
        }

        if (enemySpawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawnManager: 스폰 포인트가 설정되지 않았습니다!");
            return;
        }

        Debug.Log($"스폰 포인트 개수: {enemySpawnPoints.Length}");
        StartStage(currentStage);
    }

    void Update()
    {
        if (!isPlayerAlive)
            return;

        if (currentStage <= maxStage && enemySpawnedThisStage < enemyToSpawnThisStage)
        {
            currentEnemySpawnDelay += Time.deltaTime;

            if (currentEnemySpawnDelay > enemySpawnDelay)
            {
                if (aliveEnemies < maxAliveEnemies)
                {
                    SpawnEnemyByStage(currentStage);
                    currentEnemySpawnDelay = 0f;
                    enemySpawnDelay = Random.Range(0.5f, 2f);
                }
            }
        }
    }

    void SpawnEnemyByStage(int stage)
    {
        GameObject prefabToSpawn = null;

        List<GameObject> possibleEnemy = new List<GameObject>();

        bool spawnBoss = (stage % 5 == 0);

        if (stage == 1 || stage == 2)
        {
            possibleEnemy.Add(enemyPrefab1);
        }
        else if (stage >= 3 && stage <= 5)
        {
            possibleEnemy.Add(enemyPrefab1);
            possibleEnemy.Add(enemyPrefab2);
        }
        else if (stage >= 6 && stage <= 9)
        {
            possibleEnemy.Add(enemyPrefab1);
            possibleEnemy.Add(enemyPrefab2);
            possibleEnemy.Add(enemyPrefab3);
        }
        else // stage 10
        {
            possibleEnemy.Add(enemyPrefab1);
            possibleEnemy.Add(enemyPrefab2);
            possibleEnemy.Add(enemyPrefab3);
        }

        if (spawnBoss && enemySpawnedThisStage == enemyToSpawnThisStage - 1)
        {
            if (enemySpawnPoints.Length > 4 && bossPrefab != null)
            {
                prefabToSpawn = bossPrefab;

                Vector2 offset = Random.insideUnitCircle * 0.3f;
                Vector3 spawnPos = enemySpawnPoints[4].position + new Vector3(offset.x, offset.y, 0);
                GameObject boss = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

                Enemy enemyScript = boss.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.spawnManager = this;
                }

                enemySpawnedThisStage++;
                aliveEnemies++;
                Debug.Log($"보스 스폰! 스테이지: {stage}, 위치: {spawnPos}");
                return;
            }
            else
            {
                Debug.LogError("보스 스폰 실패: 스폰포인트 부족 또는 보스 프리팹 없음");
                return;
            }
        }

        if (possibleEnemy.Count == 0)
        {
            Debug.LogError($"스테이지 {stage}에 스폰할 적이 없습니다!");
            return;
        }

        prefabToSpawn = possibleEnemy[Random.Range(0, possibleEnemy.Count)];

        if (prefabToSpawn == null)
        {
            Debug.LogError("선택된 적 프리팹이 null입니다!");
            return;
        }

        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
        Vector2 offsetNormal = Random.insideUnitCircle * 0.3f;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(offsetNormal.x, offsetNormal.y, 0);

        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        Enemy enemyScriptNormal = enemy.GetComponent<Enemy>();
        if (enemyScriptNormal != null)
        {
            enemyScriptNormal.spawnManager = this;
        }

        enemySpawnedThisStage++;
        aliveEnemies++;

        Debug.Log($"적 스폰! 스테이지: {stage}, 스폰된 수: {enemySpawnedThisStage}/{enemyToSpawnThisStage}, 위치: {spawnPosition}");
    }

    public void OnEnemyKilled()
    {
        aliveEnemies--;
        Debug.Log($"적 처치! 남은 적: {aliveEnemies}");

        if (enemySpawnedThisStage >= enemyToSpawnThisStage && aliveEnemies <= 0)
        {
            currentStage++;
            if (currentStage <= maxStage)
            {
                StartStage(currentStage);
            }
            else
            {
                Debug.Log("모든 스테이지 완료! 클리어!");
            }
        }
    }

    void StartStage(int stage)
    {
        enemySpawnedThisStage = 0;
        aliveEnemies = 0;

        enemyToSpawnThisStage = 3 + 2 * stage;

        bool spawnBoss = (stage % 5 == 0);
        if (spawnBoss)
            enemyToSpawnThisStage += 1;

        Debug.Log($"스테이지 {stage} 시작! 스폰할 적의 수: {enemyToSpawnThisStage}");
    }
}