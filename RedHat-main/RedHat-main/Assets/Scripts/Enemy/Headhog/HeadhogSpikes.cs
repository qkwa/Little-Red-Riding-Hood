using UnityEngine;
using System.Collections;

public class HeadhogSpikes : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private float minVerticalPush = 0.3f;
    [SerializeField] private float horizontalPushMultiplier = 1.5f;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerInput playerMovement = collision.gameObject.GetComponent<PlayerInput>();

            if (playerHealth != null && playerMovement != null)
            {
                // Реверс направления: от игрока к ежу (теперь будет отталкивать правильно)
                Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

                // Усиливаем горизонтальную составляющую (новый способ)
                pushDirection.x = Mathf.Sign(-pushDirection.x) * Mathf.Max(Mathf.Abs(pushDirection.x), 0.5f);

                // Гарантируем толчок вверх
                pushDirection.y = Mathf.Max(pushDirection.y, 0.3f);
                pushDirection.Normalize();

                // Наносим урон с отталкиванием
                playerHealth.TakeDamageWithPush(damage, pushDirection, pushForce);
                soundManager.PlayHedgehogAttack();
                SetAtackAnimation(true);
                StartCoroutine(Attacking(0.1f));

                Debug.DrawRay(collision.transform.position, pushDirection * 3, Color.red, 2f); // Визуализация
            }
        }
    }

    IEnumerator Attacking(float time)
    {
        yield return new WaitForSeconds(time);
        SetAtackAnimation(false);
    }

    public void SetAtackAnimation(bool isAttacking)
    {
        if (animator != null)
            animator.SetBool("Atack", isAttacking);
    }
}