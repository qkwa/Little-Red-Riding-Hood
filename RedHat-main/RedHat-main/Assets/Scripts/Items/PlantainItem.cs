using UnityEngine;

public class PlantainItem : InteractItem
{
    [SerializeField] private int healAmount = 25;
    
    
    public override void Use(PlayerInput player)
    {
        if (player.TryGetComponent(out PlayerHealth health))
        {
            health.Heal(healAmount);
            
        }
    }
}