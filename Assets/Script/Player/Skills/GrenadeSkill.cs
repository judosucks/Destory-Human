//
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Yushan.Enums;
//
// public class GrenadeSkill : Skill
// {
//     public GrenadeType grenadeType = GrenadeType.Frag;
//     private Mouse mouse;
//
//     [Header("skill info")] [SerializeField]
//     private GameObject grenadePrefab;
//
//     [SerializeField] private float grenadeGravity;
//     [SerializeField] private Vector2 launchForce;
//
//     private Vector2 finalDirection;
//
//     [Header("Aim dots")] [SerializeField] private int numberOfDots;
//     [SerializeField] private float spaceBetweenDots;
//     [SerializeField] private GameObject dotPrefab;
//     [SerializeField] private Transform dotsParent;
//
//     private GameObject[] dots;
//     [SerializeField] private Transform handPoint;
//
//     [Header("grende info")] [SerializeField]
//     private float ExplosionTimer; //editable in inspector
//
//     private GameObject spawnedGrenade; // 用于记录已生成的手榴弹
//     [SerializeField] private float handPointOffsetY; //spwan grenade offset y
//     public bool mouseIsTriggered;
//     public bool rightButtonIsPressed;
//     public float explosionTimer => ExplosionTimer; //read-only
//
//     [Header("Debounce info")] 
//     [SerializeField] private bool isRightButtonLocked = false;
//
//     [SerializeField] private float rightButtonCooldown = 0.2f;
//     [SerializeField] private float lastRightButtonTime = -1f;
//     private void Awake()
//     {
//         mouse = Mouse.current;
//     }
//
//     protected override void Start()
//     {
//         base.Start();
//         GenerateDots();
//         SetupGravity();
//         if (mouse == null)
//         {
//             Debug.LogError("Mouse is null");
//             mouse = Mouse.current;
//             Debug.LogWarning("setting mouse again");
//         }
//         else
//         {
//             Debug.LogWarning("mouse is not null");
//         }
//     }
//
//     protected override void Update()
//     {
//
//         if (mouse.rightButton.wasReleasedThisFrame)
//         {
//             if (Time.time >= lastRightButtonTime + rightButtonCooldown)
//             {
//                 isRightButtonLocked = true;
//                 lastRightButtonTime = Time.time;
//                 mouseIsTriggered = true;
//                 Debug.Log("right button triggered");
//             }
//             ResetGrenadeState();
//             player.skill.grenadeSkill.DotsActive(false);
//             DestroyGrenadeSpwawn();//clean up grenade spawned
//             if (!player.grenadeCanceled)
//             {
//                 //trigger throwing animation
//                 player.anim.SetTrigger("ThrowGrenade");
//                 if (player.anim.GetBool("AimGrenade"))
//                 {
//                   player.anim.SetBool("AimGrenade",false);
//                 }
//                 // Calculate final throw direction and launch grenade
//                 finalDirection = new Vector2(
//                     AimDirection().normalized.x * launchForce.x,
//                     AimDirection().normalized.y * launchForce.y);
//
//             }
//             else
//             {
//                 if (player.grenadeCanceled)
//                 {
//                     Debug.LogWarning("grenade canceled, aborting operation");
//                     player.skill.grenadeSkill.DotsActive(false);
//                     // Ensure animator transitions cleanly
//                     if (player.anim.GetBool("AimGrenade"))
//                     {
//                         Debug.LogWarning("trigger aim abort");
//                         player.anim.SetBool("AimGrenade", false);
//                         player.anim.SetTrigger("AimAbort"); // Transition to abort state
//                     }
//                     player.stateMachine.ChangeState(player.idleState);
//                     
//
//                 }
//             }
//             player.isAiming = false;
//             player.isAimCheckDecide = false;
//
//         }
//         if(!mouse.rightButton.isPressed)
//         if (Mouse.current.rightButton.isPressed && !mouseIsTriggered)
//         {
//             rightButtonIsPressed = true;
//
//             // Prevent any further functionality if grenade was canceled
//             if (mouseIsTriggered)
//             {
//                 Debug.Log("right button pressed return");
//                 return;
//             }
//
//             if (!isRightButtonLocked)
//             {
//                 isRightButtonLocked = true;
//                 //start aiming logic
//                 player.skill.grenadeSkill.DotsActive(true);
//                 SpawnGrenade();
//                 player.anim.SetBool("AimGrenade", true);
//             }
//
//             // Aiming logic
//             if (player.isAiming)
//             {
//                 Debug.Log("player is aiming, updating grenade trajectory");
//                 // Update dots position for grenade trajectory
//                 UpdateDotsPosition();
//             }
//         }
//
//         // Cancel grenade if left button is pressed
//                 if (mouse.leftButton.wasPressedThisFrame)
//                 {
//                     // Block further input and cancel grenade state
//                     ResetGrenadeState();
//                     Debug.Log("left button pressed, canceling grenade");
//
//                     // Cancel grenade logic
//                     player.CancelThrowGrenade();
//                     DestroyGrenadeSpwawn(); // Destroy the spawned grenade
//
//                     // Animator state management
//                     if (player.anim.GetBool("AimGrenade"))
//                     {
//                         Debug.Log("aim abort triggered");
//                         player.anim.SetBool("AimGrenade", false);
//                         player.anim.SetTrigger("AimAbort"); // Transition to abort state
//                     }
//
//                     return;
//                 }
//
//                 
//
//                 
//             
// // Right button released logic
//         if (player.isAimCheckDecide && mouse.rightButton.wasReleasedThisFrame)
//         {
//             Debug.Log("right button released");
//             ResetGrenadeState();
//             DestroyGrenadeSpwawn();
//
//             // Cleanup if grenade was canceled and right button was released
//             if (player.grenadeCanceled && !player.isAiming)
//             {
//                 
//                 Debug.LogError(player.anim.GetBool("AimGrenade"));
//                 player.isAiming = false;
//                 player.isAimCheckDecide = false;
//                 
//                 
//             }
//
//             // Throw grenade logic
//             Debug.LogWarning("triggering throw grenade");
//             player.anim.SetTrigger("ThrowGrenade");
//             player.isAiming = false;
//             player.isAimCheckDecide = false;
//             
//
//            
//             
//
//             CameraManager.instance.newCamera.ResetZoom();
//             player.stateMachine.ChangeState(player.idleState);
//             return;
//         }
//
//
//         if (player.isAiming)
//         {
//             UpdateDotsPosition();
//         }
//         else
//         {
//             DotsActive(false);
//         }
//         
//
//          if (spawnedGrenade != null)
//          { 
//              spawnedGrenade.transform.position = handPoint.position;
//          }
//          
//          
//     }
//
//     private void UpdateDotsPosition()
//     {
//         for (int i = 0; i < dots.Length; i++)
//         {
//             Vector3 handPoint3D = handPoint.position;
//             Vector2 handPoint2D = new Vector2(handPoint3D.x, handPoint3D.y);//convert handpoint transform from vector3 to vector2
//             
//             dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
//         }
//     }
//     public void ResetGrenadeState()
//     {
//         mouseIsTriggered = false;
//         player.rightButtonLocked = false;
//         rightButtonIsPressed = false;
//     }
//     private void SpawnGrenade()
//     {
//         if (spawnedGrenade != null)
//         {
//             return;
//         }
//         spawnedGrenade = Instantiate(grenadePrefab,new Vector2(handPoint.position.x,handPointOffsetY), Quaternion.identity);
//         spawnedGrenade.transform.SetParent(handPoint);
//     }
//
//     private void DestroyGrenadeSpwawn()
//     {
//         if (spawnedGrenade != null)
//         {
//             spawnedGrenade.transform.parent = null;
//             Destroy(spawnedGrenade);
//         }
//     }
//    
//     public void CreateGrenade()
//     {
//         if (player.grenade != null)
//         {
//             Debug.LogWarning("Cannot create grenade! A grenade is already active.");
//             return;
//         }
//
//         Debug.Log($"[CreateGrenade] Creating a {grenadeType} grenade...");
//         
//         GameObject newGrenade = Instantiate(grenadePrefab, handPoint.position, handPoint.rotation);
//         // 确保实例化的手榴弹处于激活状态
//         if (!newGrenade.activeSelf)
//         {
//             newGrenade.SetActive(true);
//             Debug.Log("newgrenade and grenadePrefab"+" "+newGrenade.activeSelf+" "+grenadePrefab.activeSelf);
//         }
//         GrenadeSkillController newGrenadeScript = newGrenade.GetComponent<GrenadeSkillController>();
//         if (newGrenade == null)
//         {
//             Debug.LogError("failed to create grenade");
//         }
//         // }else if (newGrenade != null)
//         // {
//         //     
//         //     StartCoroutine(newGrenadeScript.GrenadeFlashFx(newGrenade));
//         // }
//         
//             Debug.Log($"grenade created: {newGrenade.name}");
//         
//         
//         
//         switch (grenadeType)
//         {
//             case GrenadeType.Frag:
//                 Debug.Log("[CreateGrenade] Applying Frag Grenade setup");
//                 newGrenadeScript.SetupFragGrenade(true);
//                 break;
//
//             case GrenadeType.Flash:
//                 Debug.Log("[CreateGrenade] Applying Flash Grenade setup");
//                 newGrenadeScript.SetupFlashGrenade(true);
//                 break;
//
//             default:
//                 Debug.LogError("[CreateGrenade] Unknown grenade type. No setup applied.");
//                 break;
//         }
//         newGrenadeScript.SetupGrenade(finalDirection, grenadeGravity,player);
//         player.AssignNewGrenade(newGrenade);
//         DotsActive(false);
//         
//     }
//
//     private void SetupGravity()
//     {
//         Debug.Log("SetupGravity");
//     }
//
//     private void GenerateDots()
//     {
//         
//         Vector3 handPointPosition = new Vector3(handPoint.position.x, handPoint.position.y, handPoint.position.z); // 确保使用 Vector3
//         dots = new GameObject[numberOfDots];
//         for (int i = 0; i < numberOfDots; i++)
//         {
//             dots[i] = Instantiate(dotPrefab, handPoint.position, Quaternion.identity, dotsParent);
//             dots[i].SetActive(false);
//         }
//     }
//    
//
//     public void DotsActive(bool _isActive)
//     {
//        
//         for (int i = 0; i < dots.Length; i++)
//         {
//             dots[i].SetActive(_isActive);
//         }
//     }
//
//     private Vector2 DotsPosition(float t)
//     {
//        Vector3 position = (Vector2)player.transform.position + //initial position
//                            AimDirection().normalized * launchForce * t    //initial velocity*t
//                            + 0.5f * Physics2D.gravity * grenadeGravity * (t * t); // 1/2(gravity*t^2)
//        
//         return position;
//     }
//     
//
//     public Vector2 AimDirection()
//     {
//         Vector2 playerPosition = player.transform.position;
//         Vector2 mousePos = Mouse.current.position.ReadValue();
//        
//         Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
//         Vector2 direction = mouseWorldPosition - playerPosition;
//       
//         
//         return direction;
//     }
//     
// }
using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;

public class GrenadeSkill : Skill
{
    public GrenadeType grenadeType = GrenadeType.Frag;
    private Mouse mouse;

    [Header("skill info")] [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField] private float grenadeGravity;
    [SerializeField] private Vector2 launchForce;

    private Vector2 finalDirection;

    [Header("Aim dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;
    [SerializeField] private Transform handPoint;

    [Header("grende info")] [SerializeField]
    private GameObject spawnedGrenade; // 用于记录已生成的手榴弹
    [SerializeField] private float handPointOffsetY; //spwan grenade offset y
    public float explosionTimer => ExplosionTimer; //read-only
    [SerializeField]private float ExplosionTimer; //editable in inspector

    private void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
        
        mouse = Mouse.current;
        
    }

    protected override void Update()
    {
        base.Update();
        if (mouse.rightButton.isPressed&& playerData.mouseButttonIsInUse)
        {
            Debug.Log("right from grenade skill");
            
            // Aiming logic
            if (playerData.isAiming && !playerData.grenadeCanceled)
            {
                Debug.Log("player is aiming with grenade not canceled");
                
                // Cancel grenade if left button is pressed
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    // Block further input and cancel grenade state
                    ResetGrenadeState();
                    Debug.Log("left button pressed, canceling grenade");

                    // Cancel grenade logic
                    player.CancelThrowGrenade();
                    
                    DestroyGrenadeSpwawn(); // Destroy the spawned grenade
                    
                    player.anim.SetBool("AimGrenade", false);
                    player.anim.SetTrigger("AimAbort"); // Transition to abort state
                
                    return;
                }
                SpawnGrenade();
            }
        }

// Right button released logic
        if (playerData.isAimCheckDecided && mouse.rightButton.wasReleasedThisFrame)
        {
            Debug.Log("right button released");
            ResetGrenadeState();
            DestroyGrenadeSpwawn();

            // Cleanup if grenade was canceled and right button was released
            if (playerData.grenadeCanceled && !playerData.isAiming)
            {
                // Ensure animator transitions cleanly
                player.anim.SetBool("AimGrenade", false);
                player.anim.SetTrigger("AimAbort"); // Transition to abort state
                    
                playerData.isAiming = false;
                playerData.isAimCheckDecided = false;
                
                return;
            } 
            // Throw grenade logic
            player.anim.SetTrigger("ThrowGrenade");
            playerData.isAiming = false;
            playerData.isAimCheckDecided = false;
            
            // Calculate final throw direction and launch grenade
            finalDirection = new Vector2(
                AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y
            );
            return;
        }
        
        if (playerData.isAiming)
        {
            UpdateDotsPosition();
        }
        else
        {
            DotsActive(false);
        }

        if (spawnedGrenade != null)
        { 
            spawnedGrenade.transform.position = handPoint.position;
        }
         
    }

    private void UpdateDotsPosition()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Vector2 handPoint2D = handPoint.position;
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    public void ResetGrenadeState()
    {
        playerData.mouseButttonIsInUse = false;
        playerData.rightButtonLocked = false;
    }
    private void SpawnGrenade()
    {
        if (spawnedGrenade != null)
        {
            return;
        }
        spawnedGrenade = Instantiate(grenadePrefab,new Vector2(handPoint.position.x,handPointOffsetY),grenadePrefab.transform.rotation);
        spawnedGrenade.transform.SetParent(handPoint);
    }

    private void DestroyGrenadeSpwawn()
    {
        if (spawnedGrenade != null)
        {
            spawnedGrenade.transform.parent = null;
            Destroy(spawnedGrenade);
        }
    }
   
    public void CreateGrenade()
    {
        if (player.grenade != null)
        {
            return;
        }

        Debug.Log($"[CreateGrenade] Creating a {grenadeType} grenade...");
        
        GameObject newGrenade = Instantiate(grenadePrefab, handPoint.position, handPoint.rotation);
        // 确保实例化的手榴弹处于激活状态
        if (!newGrenade.activeSelf)
        {
            newGrenade.SetActive(true);
        }
        GrenadeSkillController newGrenadeScript = newGrenade.GetComponent<GrenadeSkillController>();
        
        if (newGrenade == null)
        {
            Debug.LogError("failed to create grenade");
        }
        
        Debug.Log($"grenade created: {newGrenade.name}");

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
        newGrenadeScript.SetupGrenade(finalDirection, grenadeGravity,player);
        player.AssignNewGrenade(newGrenade);
        DotsActive(false);
        
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
           dots[i] = Instantiate(dotPrefab, handPoint.position, Quaternion.identity, dotsParent);
           dots[i].SetActive(false);
        }
    }
   

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
       Vector3 position = (Vector2)handPoint.position + //initial position
                           AimDirection().normalized * launchForce * t    //initial velocity*t
                           + 0.5f * Physics2D.gravity * grenadeGravity * (t * t); // 1/2(gravity*t^2)
       
        return position;
    }
    

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
       
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = mouseWorldPosition - playerPosition;

        return direction;
    }
    
}