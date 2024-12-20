
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dashSkill { get; private set;}
    public CloneSkill cloneSkill { get; private set;}
    
    public GrenadeSkill grenadeSkill { get; private set;}
    
    public BlackholeSkill blackholeSkill { get; private set;}
    
    public CrystalSkill crystalSkill { get; private set;}
    private void Awake()
    {
        Debug.Log("SkillManager Awake");
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
            return;
        }
        instance = this;
        if (dashSkill == null)
        {
            
          dashSkill = GetComponent<DashSkill>();
          Debug.Log("dashSkill is set");
            
        }
    }

    private void Start()
    {
        cloneSkill = GetComponent<CloneSkill>();
        grenadeSkill = GetComponent<GrenadeSkill>();
        blackholeSkill = GetComponent<BlackholeSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }
}
