using UnityEngine;
using System;

public abstract class InteractItem : MonoBehaviour
{
    [Header("Base Item Settings")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStack = 1;

    [Header("Respawn Settings")]
    [SerializeField] private bool respawnable = true;
    [SerializeField] private Transform customRespawnPoint;
    private Vector3 originalPosition;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public int MaxStack => maxStack;
    private void Start()
    {
        originalPosition = transform.position;

        if (respawnable)
            GlobalItemManager.Instance?.RegisterItem(this, originalPosition, customRespawnPoint);
    }
    // Событие подбора предмета
    public event Action OnPickup = delegate { };

    public abstract void Use(PlayerInput player);

    public virtual void Pickup()
    {
        OnPickup?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void Drop(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
}

// Интерфейс для сброса состояния
public interface IResettable
{
    void ResetState();
}