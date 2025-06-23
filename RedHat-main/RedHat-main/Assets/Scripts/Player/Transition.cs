using UnityEngine;

public class Transition : MonoBehaviour
{
    public Transform Player;
    public Transform nextPoint;
    [SerializeField] private Blackout blackout;

    private bool isActiveTransition = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActiveTransition)
        {
            isActiveTransition = true;
            blackout.StartBlackout(true, this); // Передаем текущий Transition
        }
    }

    public void Transfer()
    {
        Debug.Log("Teleporting to: " + nextPoint.position);
        Player.position = new Vector3(
            nextPoint.position.x,
            nextPoint.position.y,
            -6f
        );
        isActiveTransition = false;
    }
}