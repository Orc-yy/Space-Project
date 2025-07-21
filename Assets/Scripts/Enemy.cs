using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float enemyHealth;
    private float enemyCurrentHealth;
    public GameObject explosionEffectPrefab;
    public GameObject enemyBulletPrefab;
    public Transform bulletSpawnPoint;
    public float enemyFireDelay;
    void Start()
    {
        enemyCurrentHealth = enemyHealth;
        StartCoroutine(AutoFire());
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
    private IEnumerator AutoFire()
    {
        while (true)
        {
            Instantiate(enemyBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(enemyFireDelay);
        }
    }
}