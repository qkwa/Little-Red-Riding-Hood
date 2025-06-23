using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private WolfState state;
    private EndGame endGame;
    [Header("Health Bar Settings")]
    [SerializeField] private Transform healthBarParent; // Родительский объект для полоски
    [SerializeField] private Transform healthBarFill;   // Красная заполняемая часть
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    private Vector3 initialScale;

    private void Awake()
    {
        endGame = FindAnyObjectByType<EndGame>();
        currentHealth = maxHealth;

        if (healthBarFill != null)
        {
            initialScale = healthBarFill.localScale;
            UpdateHealthBarPosition();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

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
            healthBarFill.localScale = new Vector3(
                initialScale.x * healthPercent,
                initialScale.y,
                initialScale.z
            );
        }
    }

    private void UpdateHealthBarPosition()
    {
        if (healthBarParent != null)
        {
            healthBarParent.position = transform.position + healthBarOffset;
            healthBarParent.rotation = Quaternion.identity;
        }
    }

    private void Die()
    {
        if (healthBarParent != null)
        {
            Destroy(healthBarParent.gameObject);
        }
        if(gameObject.name == "WolfBoss")
        {
            endGame.ShowWinMessage();
        }
        state.Die();
    }

    private void LateUpdate()
    {
        UpdateHealthBarPosition();
    }
}