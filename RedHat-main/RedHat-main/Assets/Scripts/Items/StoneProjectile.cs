using UnityEngine;

public class StoneProjectile : MonoBehaviour
{
    [Header("Stone Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float destroyDelayAfterHit = 0.1f;
    [SerializeField] private GameObject hitEffect;
    private SoundManager soundManager;
    private Rigidbody2D rb;
    private bool hasHit = false;

    private void Awake()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime); // ��������������� ����� �����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

       
        if (collision.gameObject.tag == ("Enemy"))
        {

            if (collision.transform.GetComponent<Stun>() != null)
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

        
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject, destroyDelayAfterHit);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        
        if (collision.gameObject.tag ==("Enemy"))
        {
            var health = collision.gameObject.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject, destroyDelayAfterHit);
    }

    
    public void Throw(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}