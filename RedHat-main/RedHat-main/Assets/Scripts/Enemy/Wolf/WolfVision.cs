using UnityEngine;

public class WolfVision : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float visionWidth = 3f;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask targetLayer;

    public bool CanSeeTarget(Transform target)
    {
        Vector2 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Проверка расстояния
        if (distanceToTarget > visionRange) return false;

        // Проверка ширины поля зрения (проще чем угол в 2D)
        if (Mathf.Abs(directionToTarget.y) > visionWidth) return false;

        // Проверка препятствий
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToTarget,
            distanceToTarget,
            obstacleLayers);

        if (hit.collider != null) return false;

        return ((1 << target.gameObject.layer) & targetLayer) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 size = new Vector2(visionRange, visionWidth * 2);
        Gizmos.DrawWireCube(
            transform.position + transform.right * visionRange / 2,
            size);
    }
}