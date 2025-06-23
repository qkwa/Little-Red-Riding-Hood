using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float deadZone = 0.5f; // ����, ��� �� ������ �����������
    [SerializeField] private float minDirectionTime = 0.5f; // ����������� ����� �������� � ����� �����������

    private WolfPatrol patrol;
    private Rigidbody2D rb;
    private float currentDirection;
    private float lastDirectionChangeTime;
    private float lastDirection;

    private void Awake()
    {
        patrol = GetComponent<WolfPatrol>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector2 targetPosition)
    {
        float newDirection = Mathf.Sign(targetPosition.x - transform.position.x);

        // ���� ����� ����� ����� ��� ������ (� deadZone)
        if (Mathf.Abs(targetPosition.x - transform.position.x) < deadZone)
        {
            // ��������� ������� �����������
            newDirection = currentDirection != 0 ? currentDirection : lastDirection;
        }
        else
        {
            // ��������� ����������� ������ ���� ������ minDirectionTime
            if (Time.time - lastDirectionChangeTime > minDirectionTime ||
                Mathf.Sign(newDirection) != Mathf.Sign(currentDirection))
            {
                currentDirection = newDirection;
                lastDirection = newDirection;
                lastDirectionChangeTime = Time.time;
            }
        }

        Vector2 velocity = new Vector2(currentDirection * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = velocity;
        FlipSprite(currentDirection);
    }

    private void FlipSprite(float direction)
    {
        if (direction == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
        transform.localScale = scale;
    }
}