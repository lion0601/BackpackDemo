using UnityEngine;
using UnityEngine.UI;

public class WeightManager : MonoBehaviour
{
    public Text weightDisplay;
    public int maxWeight = 50;
    private int currentWeight = 0;

    void Start()
    {
        UpdateUI();
        Invoke("RefreshWeightFromGrid", 0.02f);
    }

    public bool TryAddItem(int delta = 1)
    {
        if (currentWeight + delta > maxWeight) return false;
        currentWeight += delta;
        UpdateUI();
        return true;
    }

    public void RefreshWeightFromGrid()
    {
        GridCell[] allCells = FindObjectsOfType<GridCell>();
        int total = 0;
        foreach (var cell in allCells)
            total += cell.GetCount();
        currentWeight = total;
        UpdateUI();
    }

    public void RemoveItem(int delta = 1)
    {
        currentWeight = Mathf.Max(0, currentWeight - delta);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (weightDisplay != null)
        {
            weightDisplay.text = $"¸ºÖØ: {currentWeight} / {maxWeight}";
            weightDisplay.color = (currentWeight >= maxWeight) ? Color.red : Color.white;
        }
    }
}