using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] private float deceleration = 5f; // Скорость замедления (5 - нормально, 10 - резко)

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool drawDebugRays = true;
    [SerializeField] Inventory inventory;
    [SerializeField] UsingZone usingZone;
    [SerializeField] InventoryUI inventoryUI;
    public Vector2 CurrentVelocity => rb.linearVelocity;
    private int currentItemSelected = -1; // Начинаем с -1 (ничего не выбрано)
    private Rigidbody2D rb;
    private SoundManager soundManager;
    private float horizontalInput;
    public bool isFacingRight = true;
    public bool isGrounded;
    public bool isPushed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundManager = FindAnyObjectByType<SoundManager>();
    }

    private void Update()
    {
        TakeItem();
        HandleItemSelection(); // Теперь это просто обновляет currentItemSelected
        UseItem(currentItemSelected); // Используем сохраненное значение

        // Остальной код без изменений...
        CheckGrounded();
        horizontalInput = Input.GetAxisRaw("Horizontal");
        

        if (Mathf.Abs(horizontalInput) > 0.1f && isGrounded)
        {
            soundManager.PlayFootsteps();
        }
        else
        {
            soundManager.StopFootsteps();
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public void TakeItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && usingZone.isInZone && usingZone.currentObject != null)
        {
            ItemID itemId = GetItemId(usingZone.currentObject);
            if (inventory.AddItem(itemId, usingZone.currentObject))
            {
                usingZone.currentObject.Pickup();
                usingZone.currentObject = null;
            }
        }
    }

    private ItemID GetItemId(InteractItem item)
    {
        if (item is StickItem) return ItemID.Stick;
        if (item is StoneItem) return ItemID.Stone;
        if (item is PlantainItem) return ItemID.Plantain;
        if (item is AppleItem) return ItemID.Apple;
        throw new System.Exception("Unknown item type");
    }

    public void UseItem(int num)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ItemID itemId = (ItemID)num;
            if (inventory.HasItem(itemId))
            {
                inventory.UseItem(itemId);
                soundManager.PlayThrow();
            }
        }
    }

    private void HandleItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentItemSelected = (int)ItemID.Stick;
            inventoryUI.SelectItem(ItemID.Stick);
        }
            
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentItemSelected = (int)ItemID.Stone;
            inventoryUI.SelectItem(ItemID.Stone);
        }
            
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentItemSelected = (int)ItemID.Plantain;
            inventoryUI.SelectItem(ItemID.Plantain);
        }
            
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentItemSelected = (int)ItemID.Apple;
            inventoryUI.SelectItem(ItemID.Apple);
        }
            
    }



    public void ModifySpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }

    public void ModifyJump(float multiplier)
    {
        jumpForce *= multiplier;
    }

    private void FixedUpdate()
    {
        if (isPushed) return; // Не управляем во время толчка

        // Сохраняем текущую Y-скорость (гравитация, прыжки)
        float yVelocity = rb.linearVelocity.y;

        // Мгновенно устанавливаем X-скорость при вводе
        float xVelocity = horizontalInput * moveSpeed;

        // Плавное затухание скорости при отсутствии ввода
        if (horizontalInput == 0)
        {
            xVelocity = Mathf.Lerp(rb.linearVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            if (Mathf.Abs(xVelocity) < 0.1f) xVelocity = 0; // Полная остановка
        }

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer);

        isGrounded = hit.collider != null;

        if (drawDebugRays)
        {
            Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance,
                         isGrounded ? Color.green : Color.red);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}