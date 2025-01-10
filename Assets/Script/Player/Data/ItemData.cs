using UnityEngine;
using Yushan.Enums;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    public ItemnType itemType;
    public string itemName;
    public Sprite icon;
}
