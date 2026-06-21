using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Настройки пули")]
    public float speed = 100f;       // Скорость полета
    public float damage = 10f;       // Урон (будет перезаписан оружием)
    public float lifetime = 2f;      // Время жизни 

    void Start()
    {

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, во что врезались
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        // Уничтожаем пулю при любом столкновении 
        Destroy(gameObject);
    }
}