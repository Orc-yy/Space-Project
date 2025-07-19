using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    

    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
