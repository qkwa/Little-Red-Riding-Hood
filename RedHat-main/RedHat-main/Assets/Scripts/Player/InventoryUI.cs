using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemSlot
    {
        public Image iconImage;
        public GameObject arrow;
        public Text countText;
        public ItemID itemType;
        [SerializeField] private Sprite itemSprite; // Добавляем поле для спрайта в инспекторе
        public Sprite Sprite => itemSprite;
    }

    [Header("Settings")]
    [SerializeField] private List<ItemSlot> itemSlots;
    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private Color selectedColor = Color.white;

    private Inventory inventory;
    private ItemID? currentSelectedItem = null;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory not found!");
            return;
        }

        // Подписываемся на событие изменения инвентаря
        inventory.OnInventoryChanged += UpdateAllSlots;

        UpdateAllSlots();
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateAllSlots;
        }
    }

    public void UpdateAllSlots()
    {
        foreach (var slot in itemSlots)
        {
            UpdateSlot(slot);
        }
    }

    private void UpdateSlot(ItemSlot slot)
    {
        int itemCount = inventory.GetItemCount(slot.itemType);
        bool hasItem = itemCount > 0;
        bool isSelected = currentSelectedItem == slot.itemType;

        // Обновляем иконку
        slot.iconImage.sprite = hasItem ? GetItemSprite(slot) : emptySlotSprite;
        slot.iconImage.color = hasItem ? (isSelected ? selectedColor : Color.white) : normalColor;

        // Обновляем счетчик
        if (slot.countText != null)
        {
            slot.countText.text = hasItem ? itemCount.ToString() : "";
            slot.countText.gameObject.SetActive(hasItem);
        }

        // Обновляем стрелку
        if (slot.arrow != null)
        {
            slot.arrow.SetActive(isSelected && hasItem);
        }
    }

    public void SelectItem(ItemID itemType)
    {
        currentSelectedItem = itemType;
        UpdateAllSlots();
    }

    private Sprite GetItemSprite(ItemSlot slot)
    {
        // Сначала проверяем спрайт назначенный в инспекторе
        if (slot.Sprite != null)
            return slot.Sprite;

        // Если нет - пробуем загрузить из ресурсов
        var loadedSprite = Resources.Load<Sprite>($"Items/{slot.itemType}");
        if (loadedSprite != null)
            return loadedSprite;

        Debug.LogWarning($"Sprite for {slot.itemType} not found!");
        return emptySlotSprite;
    }
}