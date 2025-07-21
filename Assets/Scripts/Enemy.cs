using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth;
    private float enemyCurrentHealth;

    public GameObject explosionEffectPrefab;


    void Start()
    {
        enemyCurrentHealth = enemyHealth;
    }


    public void TakeDamage(float damage)
    {
        enemyCurrentHealth -= damage;
        Debug.Log("Enemy hit!  HP : " + enemyCurrentHealth);

        if (enemyCurrentHealth < 0) 
        {
            Die();
        }
    }


    public void Die()
    {
        GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        Destroy(effect, 2f);

        Destroy(gameObject);
    }
}