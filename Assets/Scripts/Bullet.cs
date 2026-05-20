using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public int   damage   = 1;
    public float lifetime = 5f;

    private Vector3    direction;
    private float      speed;
    private GameObject owner;

    public void Initialize(Vector3 dir, float spd, GameObject ownerObj)
    {
        direction = dir.normalized;
        speed     = spd;
        owner     = ownerObj;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 플랫폼/벽 등 충돌 시 소멸
        if (other.GetComponent<PlayerController>() == null)
            Destroy(gameObject);
    }
}