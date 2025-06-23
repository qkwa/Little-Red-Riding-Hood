using UnityEngine;

public class StickItem : InteractItem
{
    [Header("Throwing Settings")]
    [SerializeField] private float throwForce = 12f; // Меньше чем у камня
    [SerializeField] private GameObject stickProjectilePrefab;
    [SerializeField] private float throwOffset = 0.5f;
    [SerializeField] private int damage = 5; // Меньше урон чем у камня

    public override void Use(PlayerInput player)
    {
        if (stickProjectilePrefab == null)
        {
            Debug.LogError("Stick projectile prefab is not assigned!");
            return;
        }

        // Определяем направление броска
        Vector3 throwDirection = player.isFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = player.transform.position + throwDirection * throwOffset;

        // Создаем снаряд
        GameObject stick = Instantiate(
            stickProjectilePrefab,
            spawnPosition,
            Quaternion.identity
        );
        Destroy(stick, 1f);  // <-- Добавь эту строку

        // Инициализируем снаряд
        StickProjectile projectile = stick.GetComponent<StickProjectile>();
        if (projectile != null)
        {
            projectile.SetDamage(damage); // Устанавливаем урон
            projectile.Throw(throwDirection, throwForce);
        }
        else
        {
            Debug.LogError("StickProjectile component missing on prefab!");
        }
    }
}