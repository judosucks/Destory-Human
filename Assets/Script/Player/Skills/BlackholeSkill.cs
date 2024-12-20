using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    
    public float blackholeDuration;
    FireOrbitController fireOrbitController;
    BlackholeSkillController currentBlackhole;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        GameObject newBlackhole = Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);
        
        currentBlackhole = newBlackhole.GetComponent <BlackholeSkillController>();
        
        fireOrbitController = newBlackhole.GetComponent<FireOrbitController>();
        
        currentBlackhole.SetupBlackhole(maxSize,growSpeed,shrinkSpeed,amountOfAttacks,cloneCooldown,blackholeDuration);
        // 啟動火焰公轉效果（只需初始化一次）
        fireOrbitController.Initialize(currentBlackhole.transform);
        // 啟用火焰
        fireOrbitController.SetFireObjectsActive(true);
           
        
    }

    public bool BlackholeSkillCompleted()
    {
        if (!currentBlackhole||currentBlackhole == null)
        {
            
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            
            currentBlackhole = null;
            return true;
        }
        
        return false;
    }
}
