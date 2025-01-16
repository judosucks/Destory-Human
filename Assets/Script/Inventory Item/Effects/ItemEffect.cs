
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    public virtual void ExcutedEffect(Transform _enemyPosition)
    {
        Debug.Log("Item Effect");
    }
}
