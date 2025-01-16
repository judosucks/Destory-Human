using System;
using UnityEngine;
using Yushan.Enums;
using TMPro;
public class UIStatSlot : MonoBehaviour
{
    [SerializeField]private string statName;
    [SerializeField]private StatType statType;
    [SerializeField]private TextMeshProUGUI statValue;
    [SerializeField]private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "stat -" + statName;
        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValue.text = playerStats.StatToModify(statType).GetValue().ToString();
        }
    }
}
