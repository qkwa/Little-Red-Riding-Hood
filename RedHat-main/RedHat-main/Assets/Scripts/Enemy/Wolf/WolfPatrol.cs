using UnityEngine;

public class WolfPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform leftBound;
    [SerializeField] private Transform rightBound;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private float waitTimer;
    private bool movingRight = true;
    public bool isWaiting = false;
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0) isWaiting = false;
            return;
        }

        PatrolMovement();
    }

    private void PatrolMovement()
    {
        float targetX = movingRight ? rightBound.position.x : leftBound.position.x;
        float direction = Mathf.Sign(targetX - transform.position.x);
        
        // Используем MoveTo из EnemyMovement для согласованного движения
        enemyMovement.MoveTo(new Vector2(targetX, transform.position.y));

        // Проверка достижения границы
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            movingRight = !movingRight;
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (leftBound && rightBound)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                new Vector2(leftBound.position.x, transform.position.y),
                new Vector2(rightBound.position.x, transform.position.y));
        }
    }
}