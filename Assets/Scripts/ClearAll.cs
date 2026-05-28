using UnityEngine;

public class ClearAll : MonoBehaviour
{
    public void ClearAllItems()
    {
        GridCell[] allCells = FindObjectsOfType<GridCell>();
        foreach (GridCell cell in allCells)
            cell.ClearItem();

        WeightManager wm = FindObjectOfType<WeightManager>();
        if (wm != null) wm.RefreshWeightFromGrid();

        Debug.Log("±³°üÒÑÇå¿Õ");
    }
}