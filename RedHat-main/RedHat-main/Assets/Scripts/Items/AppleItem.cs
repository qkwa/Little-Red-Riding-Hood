using UnityEngine;

public class AppleItem : InteractItem
{
    [SerializeField] private float speedMultiplier = 1.3f;
    [SerializeField] private float jumpMultiplier = 1.2f;
    [SerializeField] private float damageMultiplier = 2f;
    [SerializeField] private float duration = 20f;

    public override void Use(PlayerInput player)
    {
        var effects = player.GetComponent<PlayerEffects>();
        if (effects != null)
        {
            effects.ApplyEffect(new SpeedEffect(speedMultiplier, duration));
            effects.ApplyEffect(new JumpEffect(jumpMultiplier, duration));
            effects.ApplyEffect(new DamageEffect(damageMultiplier, duration));
            
        }
        
    }
    
}