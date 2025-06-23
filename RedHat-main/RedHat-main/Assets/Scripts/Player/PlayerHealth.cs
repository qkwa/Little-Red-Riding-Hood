using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("UI Settings")]
    [SerializeField] private Image healthBarFill; // Ссылка на Image компонент полоски здоровья
    [SerializeField] private bool isHealthBarRightToLeft = false; // Направление заполнения
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField, HideInInspector] private PlayerInput playerInput;
    [Header("Push Settings")]
    [SerializeField] private float pushRecoveryTime = 0.5f;
    [SerializeField] private float pushDeceleration = 15f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private AudioClip hitSound;

    private Rigidbody2D rb;
    private Vector2 pushVelocity;
    private bool isPushed;
    private float pushEndTime;
    private SoundManager soundManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        InitializeHealthBar();
        UpdateHealthBar();
        soundManager = FindAnyObjectByType<SoundManager>();
    }

    public void TakeDamageWithPush(int damage, Vector2 pushDirection, float pushForce)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        // Визуальные эффекты
        if (hitParticles != null)
        {
            hitParticles.transform.position = transform.position;
            hitParticles.Play();
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        ApplyPush(pushDirection, pushForce);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = healthPercent;
            if (healthPercent < 0.3f) // Порог низкого HP
            {
                soundManager.PlayHeartbeat();
            }
        }
        

        
    }

    private void ApplyPush(Vector2 direction, float force)
    {
        playerInput.isPushed = true;
        pushVelocity = direction.normalized * force;
        isPushed = true;
        pushEndTime = Time.time + pushRecoveryTime;
    }

    private void InitializeHealthBar()
    {
        if (healthBarFill != null)
        {
            // Настройка якорей и пивота с небольшим смещением вправо
            float xOffset = 0.01f; // Значение от 0 до 1 (10% смещение)
            healthBarFill.rectTransform.anchorMin = new Vector2(xOffset, 0.5f);
            healthBarFill.rectTransform.anchorMax = new Vector2(xOffset, 0.5f);
            healthBarFill.rectTransform.pivot = new Vector2(0, 0.5f);

            // Дополнительное смещение через localPosition
            healthBarFill.rectTransform.anchoredPosition = new Vector2(6f, 0); // Пиксельное смещение

            // Для заполнения слева направо 
            healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }

    private void FixedUpdate()
    {
        if (isPushed)
        {
            rb.linearVelocity = pushVelocity;
            pushVelocity = Vector2.MoveTowards(pushVelocity, Vector2.zero, pushDeceleration * Time.fixedDeltaTime);

            if (Time.time >= pushEndTime)
            {
                isPushed = false;
                playerInput.isPushed = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    public void Die()
    {
        playerAnimationController.Dead();
        playerInput.enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        
    }
}