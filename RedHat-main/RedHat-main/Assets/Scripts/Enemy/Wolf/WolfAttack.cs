using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float pushForce = 5f; // Сила отталкивания
    SoundManager soundManager;
    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private float lastAttackTime;
    private bool isAttacking;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
    }

    public void StartAttack()
    {
        if (!CanAttack() || isAttacking) return;

        isAttacking = true;
        lastAttackTime = Time.time;
        Invoke(nameof(PerformAttack), attackDelay);
    }

    private void PerformAttack()
    {
        if (IsPlayerInRange())
        {
            Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if (player != null)
            {
                soundManager.PlayWolfAttack();
                Vector2 pushDirection = (player.transform.position - transform.position).normalized;
                player.GetComponent<PlayerHealth>()?.TakeDamageWithPush(damage, pushDirection, pushForce);
                Debug.Log($"Wolf attacked player! Damage: {damage}, Push: {pushForce}");
            }
        }
        isAttacking = false;
    }

    public void StopAttack()
    {
        CancelInvoke(nameof(PerformAttack));
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos || attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        // Визуализация направления атаки
        if (Application.isPlaying && IsPlayerInRange())
        {
            Gizmos.color = Color.yellow;
            Vector2 playerPos = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer).transform.position;
            Gizmos.DrawLine(attackPoint.position, playerPos);
        }
    }
}