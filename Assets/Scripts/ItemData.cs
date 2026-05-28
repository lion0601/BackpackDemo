using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "背包系统/物品数据")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStack;
}