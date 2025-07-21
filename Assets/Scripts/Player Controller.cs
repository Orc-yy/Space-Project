using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigid;

    [SerializeField]
    private float moveSpeed;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public float bulletFireDelay;
    

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();

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


    private IEnumerator AutoFireBullet()
    {
        while (true)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(bulletFireDelay);
        }
    }
}
