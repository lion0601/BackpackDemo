using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridCell : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int row;
    public int col;
    private ItemData item;
    private Image iconImage;
    private Text countText;
    private int currentCount = 0;

    private static GridCell draggedFrom;
    private Color originalColor;

    private string ItemKey => $"cell_{row}_{col}_item";
    private string CountKey => $"cell_{row}_{col}_count";

    void Start()
    {
        iconImage = GetComponent<Image>();
        if (iconImage == null) iconImage = gameObject.AddComponent<Image>();
        originalColor = iconImage.color;
        countText = GetComponentInChildren<Text>();

        int index = transform.GetSiblingIndex();
        int columns = 6;
        row = index / columns;
        col = index % columns;

        if (PlayerPrefs.HasKey(CountKey) && PlayerPrefs.GetInt(CountKey) > 0)
        {
            string savedItemName = PlayerPrefs.GetString(ItemKey, "");
            if (!string.IsNullOrEmpty(savedItemName))
            {
                ItemData loadedItem = Resources.Load<ItemData>($"Items/{savedItemName}");
                AssignItem(loadedItem);
            }
            currentCount = PlayerPrefs.GetInt(CountKey, 0);
            UpdateUI();
        }
        else
        {
            if (row == 0) AssignItem(Resources.Load<ItemData>("Items/Stone"));
            else if (row == 1) AssignItem(Resources.Load<ItemData>("Items/Wood"));
            else AssignItem(null);
            currentCount = (item != null) ? 1 : 0;
            UpdateUI();
            Save();
        }
    }

    private void AssignItem(ItemData newItem)
    {
        item = newItem;
        if (item != null && item.icon != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else if (item != null)
        {
            iconImage.color = Color.gray;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = (currentCount >= 2) ? currentCount.ToString() : "";
        }
    }

    private void Save()
    {
        if (item != null) PlayerPrefs.SetString(ItemKey, item.name);
        else PlayerPrefs.SetString(ItemKey, "");
        PlayerPrefs.SetInt(CountKey, currentCount);
        PlayerPrefs.Save();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 空格子：左键生成默认物品
        if (item == null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ItemData defaultItem = null;
                if (row == 0) defaultItem = Resources.Load<ItemData>("Items/Stone");
                else if (row == 1) defaultItem = Resources.Load<ItemData>("Items/Wood");
                if (defaultItem != null)
                {
                    WeightManager wm = FindObjectOfType<WeightManager>();
                    if (wm != null && !wm.TryAddItem(1))
                    {
                        Debug.Log("背包已满，无法拾取！");
                        return;
                    }
                    AssignItem(defaultItem);
                    currentCount = 1;
                    UpdateUI();
                    Save();
                    Debug.Log($"获得 {defaultItem.itemName}，数量：1");
                }
            }
            return;
        }

        // 有物品时的处理
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            WeightManager wm = FindObjectOfType<WeightManager>();
            if (wm != null && !wm.TryAddItem(1))
            {
                Debug.Log("背包已满，无法拾取！");
                return;
            }
            currentCount++;
            UpdateUI();
            Save();
            Debug.Log($"获得 {item.itemName}，当前数量：{currentCount}");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (currentCount <= 0) return;
            currentCount--;
            if (currentCount <= 0)
                ClearItem();  // 数量归零时清空格子
            else
            {
                UpdateUI();
                Save();
            }
            WeightManager wm = FindObjectOfType<WeightManager>();
            if (wm != null) wm.RemoveItem(1);
            Debug.Log($"丢弃 {item.itemName}，剩余数量：{currentCount}");
        }
    }

    // 拖拽相关（保持不变）
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        draggedFrom = this;
        Color c = iconImage.color;
        c.a = 0.5f;
        iconImage.color = c;
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedFrom != null) draggedFrom.iconImage.color = draggedFrom.originalColor;
        draggedFrom = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedFrom == null || draggedFrom == this) return;
        ItemData tempItem = draggedFrom.item;
        int tempCount = draggedFrom.currentCount;
        draggedFrom.SetItemAndCount(this.item != null ? this.item.name : "", this.currentCount);
        this.SetItemAndCount(tempItem != null ? tempItem.name : "", tempCount);
        WeightManager wm = FindObjectOfType<WeightManager>();
        if (wm != null) wm.RefreshWeightFromGrid();
        draggedFrom.iconImage.color = draggedFrom.originalColor;
        draggedFrom = null;
    }

    // 公共方法
    public bool HasItem() => item != null;
    public string GetItemName() => item == null ? "" : item.name;
    public int GetCount() => currentCount;
    public void ClearItem()
    {
        item = null;
        currentCount = 0;
        iconImage.enabled = false;
        UpdateUI();
        Save();
    }

    public void SetItemAndCount(string itemName, int count)
    {
        if (string.IsNullOrEmpty(itemName) || count <= 0)
        {
            AssignItem(null);
            currentCount = 0;
        }
        else
        {
            ItemData newItem = Resources.Load<ItemData>($"Items/{itemName}");
            if (newItem == null)
            {
                Debug.LogError($"找不到物品: {itemName}");
                return;
            }
            AssignItem(newItem);
            currentCount = count;
        }
        UpdateUI();
        Save();
    }
}