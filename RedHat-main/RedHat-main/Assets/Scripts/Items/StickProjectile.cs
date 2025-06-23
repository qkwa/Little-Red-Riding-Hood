using UnityEngine;

public class StickProjectile : MonoBehaviour
{
    [Header("Stick Settings")]
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float destroyDelayAfterHit = 0.1f;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private int damage;
    private SoundManager soundManager;
    private Rigidbody2D rb;
    private bool hasHit = false;

    private void Awake()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void Throw(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        // �� ��������� �� �������� � ������ ������
        //if (collision.gameObject.tag ==("Player"))
        //return;

        //hasHit = true;

        // ������� ���� ���� ��� ����
        if (collision.gameObject.tag == ("Enemy"))
        {
            if(collision.transform.GetComponent<Stun>() != null)
            {
                Stun stun = collision.transform.GetComponent<Stun>();
                stun.OnStun();
                return;
            }
            var health = collision.gameObject.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            soundManager.PlayHit();
            Destroy(gameObject);
        }

        // ������� ������ ���������
        //if (hitEffect != null)
        {
            //Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject, destroyDelayAfterHit);
    }
}