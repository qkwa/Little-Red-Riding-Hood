using UnityEngine;

public class WolfState : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, AFK }
    public State currentState = State.Patrol;

    [Header("References")]
    [SerializeField] private WolfVision vision;
    [SerializeField] private WolfPatrol patrol;
    [SerializeField] private WolfAttack attack;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private WolfAnimationController animationController;

    [Header("Settings")]
    [SerializeField] private float chaseRange = 7f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float visionCheckInterval = 0.3f;

    private Transform player;
    private float lastVisionCheckTime;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
            enabled = false;
        }

        // Автоматическое получение ссылок на компоненты
        if (vision == null) vision = GetComponent<WolfVision>();
        if (patrol == null) patrol = GetComponent<WolfPatrol>();
        if (attack == null) attack = GetComponent<WolfAttack>();
        if (movement == null) movement = GetComponent<EnemyMovement>();
        if (animationController == null) animationController = GetComponent<WolfAnimationController>();
    }

    private void Update()
    {
        if (player == null) return;

        if (Time.time - lastVisionCheckTime > visionCheckInterval)
        {
            lastVisionCheckTime = Time.time;
            UpdateState();
        }

        ExecuteState();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Attack:
                if (distanceToPlayer > attackRange * 1.1f)
                    SwitchState(State.Chase);
                break;

            case State.Patrol:
                if (patrol != null && patrol.isWaiting)
                {
                    SwitchState(State.AFK);
                }
                else if (distanceToPlayer <= chaseRange && vision.CanSeeTarget(player))
                {
                    SwitchState(State.Chase);
                }
                break;

            case State.AFK:
                if (patrol == null || !patrol.isWaiting)
                {
                    SwitchState(State.Patrol);
                }
                else if (distanceToPlayer <= chaseRange && vision.CanSeeTarget(player))
                {
                    SwitchState(State.Chase);
                }
                break;

            case State.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    SwitchState(State.Attack);
                }
                else if (distanceToPlayer > chaseRange * 1.2f)
                {
                    SwitchState(State.Patrol);
                }
                break;
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case State.Patrol:
                patrol.enabled = true;
                movement.enabled = false;
                break;

            case State.Chase:
                patrol.enabled = false;
                movement.enabled = true;
                movement.MoveTo(player.position);
                break;

            case State.Attack:
                patrol.enabled = false;
                movement.enabled = false;
                attack.StartAttack();
                if (animationController != null)
                {
                    animationController.TriggerAttackAnimation();
                }
                break;

            case State.AFK:
                patrol.enabled = true;
                movement.enabled = false;
                break;
        }
    }

    private void SwitchState(State newState)
    {
        if (currentState == newState) return;

        switch (currentState)
        {
            case State.Attack:
                attack.StopAttack();
                break;
        }

        currentState = newState;
        Debug.Log($"Wolf state changed to: {newState}");
    }

    public void Die()
    {
        if (!animationController.IsDead()) // Проверяем, не умер ли уже волк
        {
            animationController.TriggerDeathAnimation();
        }
    }
}