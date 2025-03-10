using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class UI : MonoBehaviour
{
    private Keyboard keyboard;
   
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    public UISkillTreeTooltip skillTreeTooltip;
    public UIItemTooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UICraftWindow craftWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }

    void Start()
    {
        SwitchTo(inGameUI);
        keyboard = Keyboard.current;
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(keyboard.cKey.wasPressedThisFrame)
            SwitchWithKeyTo(characterUI);
        if(keyboard.bKey.wasPressedThisFrame)
            SwitchWithKeyTo(craftingUI);
        if(keyboard.kKey.wasPressedThisFrame)
            SwitchWithKeyTo(skillTreeUI);
        if(keyboard.oKey.wasPressedThisFrame)
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
            SwitchTo(inGameUI);
        }
    }
}
