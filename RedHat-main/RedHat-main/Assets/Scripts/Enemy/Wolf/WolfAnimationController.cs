using System.Collections;
using UnityEngine;


public class WolfAnimationController : MonoBehaviour
{
    [Header("Animation References")]
    [SerializeField] private Animator animator;
    [SerializeField] private WolfState wolfState;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private WolfPatrol patrol;
    [SerializeField] private WolfAttack attack;
    [SerializeField] private Collider2D wolfCollider; // ������� ��������� ��� ����������

    // ��������� ���������
    private const string IDLE_PARAM = "IsIdle";
    private const string WALK_PARAM = "IsWalking";
    private const string RUN_PARAM = "IsRunning";
    private const string ATTACK_PARAM = "Attack";
    private const string AFK_PARAM = "isAFK";
    private const string DEATH_PARAM = "Death";
    private const string DEATH_STATE_NAME = "WolfDied"; // ��� ��������� ������ � ���������

    private bool isDead = false;

    private void Update()
    {
        if (animator == null || wolfState == null || isDead) return;

        UpdateMovementAnimations();
    }

    private void UpdateMovementAnimations()
    {
        if (isDead) return;

        bool isMoving = false;
        bool isRunning = false;
        bool isAFK = false;

        switch (wolfState.currentState)
        {
            case WolfState.State.Patrol:
                isMoving = patrol != null && patrol.enabled && !patrol.isWaiting;
                break;

            case WolfState.State.AFK:
                isAFK = true;
                break;

            case WolfState.State.Chase:
                isMoving = true;
                isRunning = true;
                break;

            case WolfState.State.Attack:
                // � ����� - ��� ��������
                break;
        }

        if (animator != null)
        {
            animator.SetBool(IDLE_PARAM, !isMoving && !isRunning && !isAFK);
            animator.SetBool(WALK_PARAM, isMoving && !isRunning);
            animator.SetBool(RUN_PARAM, isRunning);
            animator.SetBool(AFK_PARAM, isAFK);
        }
    }

    public void TriggerAttackAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.SetTrigger(ATTACK_PARAM);
        }
    }

    public void TriggerDeathAnimation()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            // ��������� ��� ������ ���������
            animator.SetBool(IDLE_PARAM, false);
            animator.SetBool(WALK_PARAM, false);
            animator.SetBool(RUN_PARAM, false);

            // �������� ��������� �����
            if (attack != null) attack.StopAttack();

            // ��������� �������� ������
            animator.SetTrigger(DEATH_PARAM);

            // ��������� ��� ����������
            if (wolfState != null) wolfState.enabled = false;
            if (movement != null) movement.enabled = false;
            if (patrol != null) patrol.enabled = false;
            if (wolfCollider != null) wolfCollider.enabled = false;

            // �������� ������ ������� �� �����������
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject); // ���� ��� ��������� - ����� ����������
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // ���� ���� �������� ������ ��������
        yield return null; // ���������� ���� ���� ����� �������� ����� �������������

        // �������� ����� ������� �������� (������)
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // ���� ���������� ��������
        yield return new WaitForSeconds(animationLength);

        // ���������� ������
        Destroy(gameObject);
    }

    // ������� ����� ��� �������� ������ �� ������ ��������
    public bool IsDead()
    {
        return isDead;
    }
}