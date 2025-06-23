using UnityEngine;

public class UsingZone : MonoBehaviour
{
    public bool isInZone = false;
    public InteractItem currentObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<InteractItem>();
        if (item != null)
        {
            isInZone = true;
            currentObject = item;
            ShowPickupPrompt(true, collision);
        }
    }
    private void ShowPickupPrompt(bool show, Collider2D collision)
    {
        collision.transform.GetChild(0).gameObject.SetActive(show);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractItem"))
        {
            isInZone = false;
            currentObject = null;

            if (collision.transform.GetChild(0) != null)
            {
                ShowPickupPrompt(false, collision);
            }
        }
    }
}