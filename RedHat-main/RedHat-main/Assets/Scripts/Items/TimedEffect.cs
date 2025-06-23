// Базовый класс эффекта
public abstract class TimedEffect
{
    public float Duration { get; protected set; }
    public bool IsActive => Duration > 0;

    public virtual void ApplyEffect(PlayerInput player)
    {
        // Применяем эффект
    }

    public virtual void RemoveEffect(PlayerInput player)
    {
        // Отменяем эффект
    }

    public void Update(float deltaTime)
    {
        if (IsActive)
        {
            Duration -= deltaTime;
        }
    }
}

// Конкретные эффекты
public class SpeedEffect : TimedEffect
{
    private readonly float multiplier;
    private float originalSpeed;

    public SpeedEffect(float multiplier, float duration)
    {
        this.multiplier = multiplier;
        Duration = duration;
    }

    public override void ApplyEffect(PlayerInput player)
    {
        originalSpeed = player.moveSpeed;
        player.moveSpeed *= multiplier;
    }

    public override void RemoveEffect(PlayerInput player)
    {
        player.moveSpeed = originalSpeed;
    }
}

public class JumpEffect : TimedEffect
{
    private float multiplier;
    private float originalJumpForce;

    public JumpEffect(float multiplier, float duration)
    {
        this.multiplier = multiplier;
        Duration = duration;
    }

    public override void ApplyEffect(PlayerInput player)
    {
        //originalJumpForce = player.jumpForce;
        //player.jumpForce *= multiplier;
        player.ModifyJump(multiplier);
    }

    public override void RemoveEffect(PlayerInput player)
    {
        //player.jumpForce = originalJumpForce;
        player.ModifyJump(1f/multiplier);
    }
}

public class DamageEffect : TimedEffect
{
    private readonly float multiplier;
    

    public DamageEffect(float multiplier, float duration)
    {
        this.multiplier = multiplier;
        Duration = duration;
    }

    public override void ApplyEffect(PlayerInput player)
    {
        
    }

    public override void RemoveEffect(PlayerInput player)
    {
        
    }
}

