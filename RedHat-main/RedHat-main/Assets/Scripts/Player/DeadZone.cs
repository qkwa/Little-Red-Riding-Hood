using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.GetComponent<PlayerHealth>() != null)
        {
            PlayerHealth player = collision.transform.GetComponent<PlayerHealth>();
            player.Die();
        }
    }
}
