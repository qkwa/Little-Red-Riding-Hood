using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerEffects effects;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Blackout blackout;
    private PauseMenu pauseMenu;
    private EndGame endgame;

    [Header("Animation Parameters")]
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string isGroundedParam = "IsGrounded";
    [SerializeField] private string verticalSpeedParam = "VerticalSpeed";
    [SerializeField] private string healthStateParam = "HealthState";
    [SerializeField] private string isBuffedParam = "IsBuffed";
    [SerializeField] private string isDeadParam = "IsDead";

    [Header("Settings")]
    [SerializeField] private float lowHPThreshold = 0.5f;

    private void Awake()
    {
        endgame = FindAnyObjectByType<EndGame>();
        pauseMenu = FindAnyObjectByType<PauseMenu>();
        // Автоматическое получение компонентов, если не заданы в инспекторе
        if (!animator) animator = GetComponent<Animator>();
        if (!health) health = GetComponent<PlayerHealth>();
        if (!effects) effects = GetComponent<PlayerEffects>();
        if (!playerInput) playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (animator == null || playerInput == null) return;
        
        UpdateMovementAnimations();

        if (health != null)
        {
            UpdateHealthState();
        }

        if (effects != null)
        {
            UpdateBuffedState();
        }
    }

    private void UpdateMovementAnimations()
    {
        // Обновляем параметры движения
        animator.SetFloat(speedParam, Mathf.Abs(playerInput.CurrentVelocity.x));
        animator.SetBool(isGroundedParam, playerInput.isGrounded);
        animator.SetFloat(verticalSpeedParam, playerInput.CurrentVelocity.y);
    }

    private void UpdateHealthState()
    {
        // 0 = FullHP, 1 = LowHP
        bool isLowHP = health.currentHealth < health.maxHealth * lowHPThreshold;
        animator.SetInteger(healthStateParam, isLowHP ? 1 : 0);
        if (health.currentHealth <= 0)
        {
            Dead();
        }
    }

    private void UpdateBuffedState()
    {
        animator.SetBool(isBuffedParam, effects.HasBuff);
    }

    
    public void Dead()
    {
        endgame.ShowLoseMessage();
        animator.SetBool(isDeadParam, true);
        blackout.StartBlackout(false);
    }
    public void RestartGame()
    {
        pauseMenu.LoadMainMenu();
    }
}