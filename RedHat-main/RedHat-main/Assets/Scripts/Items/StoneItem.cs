using UnityEngine;

public class StoneItem : InteractItem
{
    [Header("Throwing Settings")]
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private GameObject stoneProjectilePrefab;
    [SerializeField] private float throwOffset = 0.5f;

    public override void Use(PlayerInput player)
    {
        if (stoneProjectilePrefab == null)
        {
            Debug.LogError("Stone projectile prefab is not assigned!");
            return;
        }

        // Определяем направление броска
        Vector3 throwDirection = player.isFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = player.transform.position + throwDirection * throwOffset;

        // Создаем снаряд
        GameObject stone = Instantiate(
            stoneProjectilePrefab,
            spawnPosition,
            Quaternion.identity
        );
        Destroy(stone, 1f);  // <-- Добавь эту строку

        // Инициализируем снаряд
        StoneProjectile projectile = stone.GetComponent<StoneProjectile>();
        if (projectile != null)
        {
            projectile.Throw(throwDirection, throwForce);
        }
        else
        {
            Debug.LogError("StoneProjectile component missing on prefab!");
        }

        
    }
}