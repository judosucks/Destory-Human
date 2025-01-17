using UnityEngine;
using Yushan.Enums;
using System.Text;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    public ItemnType itemType;
    public string itemName;
    public Sprite icon;
    
    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}
