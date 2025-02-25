using UnityEngine;

public class LedgeManager : MonoBehaviour
{
    public static LedgeManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Vector2 ActiveLedgePosition { get; private set; }

    public void UpdateLedgePosition(Vector2 position)
    {
        ActiveLedgePosition = position; // Updates when player touches a new ledge
        Debug.Log("Active Ledge: " + ActiveLedgePosition);
    }

    public void ClearLedge()
    {
        ActiveLedgePosition = Vector2.zero; // Clear ledge when player leaves
    }
}
