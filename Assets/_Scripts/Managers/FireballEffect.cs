using UnityEngine;

public class FireballEffect : AbilityEffect
{
    public float speed = 20f;
    private Vector3 direction;
    private bool hasHit = false;

    protected override void Execute()
    {
        direction = transform.forward;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(data.damage);
            }
        }

        hasHit = true;
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;
        Destroy(gameObject);
    }
}