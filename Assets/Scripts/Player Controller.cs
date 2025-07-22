using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;
    [SerializeField]
    private float moveSpeed;

    public GameObject explosionEffectPrefab;
    public GameObject bulletPrefab;

    public Transform bulletSpawnPoint;

    public EnemySpawnManager enemySpawnManager;

    public float bulletFireDelay;
    public float playerHealth;
    private float playerCurrentHealth;
    public float level;


    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        playerCurrentHealth = playerHealth;
        StartCoroutine(AutoFireBullet());
    }


    void Update()
    {
        Move();
    }


    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(x, y).normalized;
        rigid.linearVelocity = moveDirection * moveSpeed;
    }


    public void TakeDamage(float damage)
    {
        playerCurrentHealth -= damage;
        Debug.Log("Player Health : " + playerCurrentHealth);
        if (playerCurrentHealth < 0)
        {
            Die();
        }
    }


    private void Die()
    {
        GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 2f);

        if (enemySpawnManager != null)
        {
            enemySpawnManager.isPlayerAlive = false;
        }

        Destroy(gameObject);
    }


    public void LevelUp()
    {
        level++;
        Debug.Log("�÷��̾� ���� 1 ��� ! ���� ���� : " + level);
    }


    private IEnumerator AutoFireBullet()
    {
        while (true)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(bulletFireDelay);
        }
    }
}
