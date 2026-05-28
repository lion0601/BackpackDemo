using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BackpackSorter : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(SortBackpack);
    }

    public void SortBackpack()
    {
        GridCell[] cells = FindObjectsOfType<GridCell>();
        System.Array.Sort(cells, (a, b) =>
        {
            if (a.row != b.row) return a.row.CompareTo(b.row);
            return a.col.CompareTo(b.col);
        });

        List<ItemInfo> items = new List<ItemInfo>();
        foreach (var cell in cells)
            if (cell.HasItem())
                items.Add(new ItemInfo(cell.GetItemName(), cell.GetCount()));

        if (items.Count == 0) return;

        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        List<ItemInfo> merged = new List<ItemInfo>();
        for (int i = 0; i < items.Count; i++)
        {
            string name = items[i].itemName;
            int total = items[i].count;
            while (i + 1 < items.Count && items[i + 1].itemName == name)
            {
                total += items[i + 1].count;
                i++;
            }
            merged.Add(new ItemInfo(name, total));
        }

        foreach (var cell in cells) cell.ClearItem();

        int idx = 0;
        foreach (var info in merged)
        {
            if (idx < cells.Length)
                cells[idx].SetItemAndCount(info.itemName, info.count);
            idx++;
        }

        WeightManager wm = FindObjectOfType<WeightManager>();
        if (wm != null) wm.RefreshWeightFromGrid();
    }

    private class ItemInfo
    {
        public string itemName;
        public int count;
        public ItemInfo(string name, int c) { itemName = name; count = c; }
    }
}