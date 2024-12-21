using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;

public class GrenadeSkill : Skill
{
    public GrenadeType grenadeType = GrenadeType.Frag;
    [Header("skill info")] 
    [SerializeField]private GameObject grenadePrefab;
    [SerializeField] private float grenadeGravity;
    [SerializeField] private Vector2 launchForce;

    private Vector2 finalDirection;

    [Header("Aim dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    
    private GameObject[] dots;

    [Header("grende info")] 
    [SerializeField] private float ExplosionTimer;//editable in inspector

    public float explosionTimer => ExplosionTimer;//read-only
    private void Awake()
    {
        
    }
    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();

    }
    protected override void Update()
    {
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Mouse.current.rightButton.isPressed)
        { 
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }  
    public void CreateGrenade()
    {
        if (player.grenade != null)
        {
            Debug.LogWarning("Cannot create grenade! A grenade is already active.");
            return;
        }

        Debug.Log($"[CreateGrenade] Creating a {grenadeType} grenade...");
        
        GameObject newGrenade = Instantiate(grenadePrefab, player.transform.position, transform.rotation);
        // 确保实例化的手榴弹处于激活状态
        if (!newGrenade.activeSelf)
        {
            newGrenade.SetActive(true);
            Debug.Log("newgrenade and grenadePrefab"+" "+newGrenade.activeSelf+" "+grenadePrefab.activeSelf);
        }
        
        
        GrenadeSkillController newGrenadeScript = newGrenade.GetComponent<GrenadeSkillController>();
        switch (grenadeType)
        {
            case GrenadeType.Frag:
                Debug.Log("[CreateGrenade] Applying Frag Grenade setup");
                newGrenadeScript.SetupFragGrenade(true);
                break;

            case GrenadeType.Flash:
                Debug.Log("[CreateGrenade] Applying Flash Grenade setup");
                newGrenadeScript.SetupFlashGrenade(true);
                break;

            default:
                Debug.LogError("[CreateGrenade] Unknown grenade type. No setup applied.");
                break;
        }
        // if (grenadeType == GrenadeType.Frag)
        // {
        //     Debug.Log("[CreateGrenade] Applying Frag Grenade setup");
        //     newGrenadeScript.SetupFragGrenade(true);
        // }else if (grenadeType == GrenadeType.Flash)
        // {
        //     Debug.Log("[CreateGrenade] Applying Flash Grenade setup");
        //     newGrenadeScript.SetupFlashGrenade(true);
        // }
        newGrenadeScript.SetupGrenade(finalDirection, grenadeGravity,player);
        Debug.Log(transform.name+" transform is not null");
        player.AssignNewGrenade(newGrenade);
        DotsActive(false);
        // CameraManager.instance.newCamera.FollowGrenade(newGrenade);
    }

    private void SetupGravity()
    {
        Debug.Log("SetupGravity");
    }

    private void GenerateDots()
    {
        
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position,Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }
    }
   

    public void DotsActive(bool _isActive)
    {
        Debug.Log("DotsActive");
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
       Vector2 position = (Vector2)player.transform.position + //initial position
                           AimDirection().normalized * launchForce * t    //initial velocity*t
                           + 0.5f * Physics2D.gravity * grenadeGravity * (t * t); // 1/2(gravity*t^2)
        return position;
    }
    

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Debug.Log("camera main is"+Camera.main);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = mouseWorldPosition - playerPosition;
      
        
        return direction;
    }
    
}
