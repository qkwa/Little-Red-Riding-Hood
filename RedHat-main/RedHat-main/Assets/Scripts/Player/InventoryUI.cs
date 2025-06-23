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
        [SerializeField] private Sprite itemSprite; // ��������� ���� ��� ������� � ����������
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

        // ������������� �� ������� ��������� ���������
        inventory.OnInventoryChanged += UpdateAllSlots;

        UpdateAllSlots();
    }

    private void OnDestroy()
    {
        // ������������ ��� ����������� �������
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

        // ��������� ������
        slot.iconImage.sprite = hasItem ? GetItemSprite(slot) : emptySlotSprite;
        slot.iconImage.color = hasItem ? (isSelected ? selectedColor : Color.white) : normalColor;

        // ��������� �������
        if (slot.countText != null)
        {
            slot.countText.text = hasItem ? itemCount.ToString() : "";
            slot.countText.gameObject.SetActive(hasItem);
        }

        // ��������� �������
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
        // ������� ��������� ������ ����������� � ����������
        if (slot.Sprite != null)
            return slot.Sprite;

        // ���� ��� - ������� ��������� �� ��������
        var loadedSprite = Resources.Load<Sprite>($"Items/{slot.itemType}");
        if (loadedSprite != null)
            return loadedSprite;

        Debug.LogWarning($"Sprite for {slot.itemType} not found!");
        return emptySlotSprite;
    }
}