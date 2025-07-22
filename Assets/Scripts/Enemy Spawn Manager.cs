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

    private int aliveEnemies = 0; // ���� ���������� ����ִ� ���� ��
    private int enemySpawnedThisStage = 0; // �ش� ������������ ������ ���� ��
    private int enemyToSpawnThisStage = 0; // �ش� ���������� ������ ���� ��

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
            Debug.LogError("EnemySpawnManager: ���� ����Ʈ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        Debug.Log($"���� ����Ʈ ����: {enemySpawnPoints.Length}");
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
                Debug.Log($"���� ����! ��������: {stage}, ��ġ: {spawnPos}");
                return;
            }
            else
            {
                Debug.LogError("���� ���� ����: ��������Ʈ ���� �Ǵ� ���� ������ ����");
                return;
            }
        }

        if (possibleEnemy.Count == 0)
        {
            Debug.LogError($"�������� {stage}�� ������ ���� �����ϴ�!");
            return;
        }

        prefabToSpawn = possibleEnemy[Random.Range(0, possibleEnemy.Count)];

        if (prefabToSpawn == null)
        {
            Debug.LogError("���õ� �� �������� null�Դϴ�!");
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

        Debug.Log($"�� ����! ��������: {stage}, ������ ��: {enemySpawnedThisStage}/{enemyToSpawnThisStage}, ��ġ: {spawnPosition}");
    }

    public void OnEnemyKilled()
    {
        aliveEnemies--;
        Debug.Log($"�� óġ! ���� ��: {aliveEnemies}");

        if (enemySpawnedThisStage >= enemyToSpawnThisStage && aliveEnemies <= 0)
        {
            currentStage++;
            if (currentStage <= maxStage)
            {
                StartStage(currentStage);
            }
            else
            {
                Debug.Log("��� �������� �Ϸ�! Ŭ����!");
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

        Debug.Log($"�������� {stage} ����! ������ ���� ��: {enemyToSpawnThisStage}");
    }
}