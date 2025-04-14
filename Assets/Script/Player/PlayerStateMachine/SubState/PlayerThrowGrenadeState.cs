
using System.IO;
using Unity.Cinemachine;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem; // Add this for mouse input

public class PlayerThrowGrenadeState : PlayerAbilityState
{
    private NewCamera newCamera;
    private CinemachinePositionComposer positionComposer;
    private float originalHorizontalScreenValue; // Store the original ScreenValue
    private FieldInfo verticalGuideField;
    private FieldInfo horizontalGuideField;
    private float originalVerticalGuideValue; // Store the original VerticalGuide value
    private float originalHorizontalGuideValue; // Store the original HorizontalGuide value
    // Add a mouse variable
    private Mouse mouse;

    public PlayerThrowGrenadeState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
        // Initialize the mouse variable in the constructor
        mouse = Mouse.current;
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isGrenadeState = true;

        if (CameraManager.instance.newCamera != null)
        {
            newCamera = CameraManager.instance.newCamera;
            positionComposer = newCamera.playerCamera.GetComponent<CinemachinePositionComposer>();
            // verticalGuideField = typeof(CinemachinePositionComposer).GetField("m_VerticalGuide", BindingFlags.NonPublic | BindingFlags.Instance);
            // horizontalGuideField = typeof(CinemachinePositionComposer).GetField("m_HorizontalGuide", BindingFlags.NonPublic | BindingFlags.Instance);
            //
            // if(horizontalGuideField != null)
            // {
            //     originalHorizontalGuideValue = (float)horizontalGuideField.GetValue(newCamera);
            // }
            // else
            // {
            //     Debug.LogError("m_HorizontalGuide field not found!");
            // }
            // if(verticalGuideField != null)
            // {
            //     originalVerticalGuideValue = (float)verticalGuideField.GetValue(newCamera);
            // }
            // else
            // {
            //     Debug.LogError("m_VerticalGuide field not found!");
            // }
           
            
        }

        player.skill.grenadeSkill.DotsActive(true);

        if (!playerData.isAiming && playerData.grenadeCanceled)
        {
            Debug.Log("grenade canceled not aiming");
            player.skill.grenadeSkill.DotsActive(false);
            if (player.anim.GetBool("AimGrenade"))
            {
                Debug.Log("grenade canceled not aiming");
                player.anim.SetBool("AimGrenade", false);
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector2.zero; // Use rb.velocity instead of rb.linearVelocity

        if (mouse.rightButton.isPressed)
        {
            if (playerData.isAiming)
            {
                Debug.Log("right button pressed from throw grenade");
                UpdateTargetScreenX();
                SmoothCameraMove();
                // Use the CinemachinePositionComposer directly
                if (positionComposer != null)
                {
                    
                    newCamera.temporaryScreenX = positionComposer.Composition.ScreenPosition.x; // This line is removed
                    
                }
            }
        }

        if (mouse.rightButton.wasReleasedThisFrame && playerData.isAiming)
        {
            Debug.Log("right button releaseed isaim");
            return;
        }
        if (mouse.rightButton.wasReleasedThisFrame && !playerData.isAiming)
        {
            Debug.Log("right mouse button was released change to idle state");
            stateMachine.ChangeState(player.idleState);
        }

            Vector2 mousePositon = mouse.position.ReadValue();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePositon);
        if (player.transform.position.x > mouseWorldPosition.x && player.facingDirection == 1)
           {
              player.Flip();
           }else if (player.transform.position.x < mouseWorldPosition.x && player.facingDirection == -1)
           {
               player.Flip();
           }
        

        
    }


    private void UpdateTargetScreenX()
    {
        // 根据玩家面向调整目标 ScreenX
        if (player.facingDirection >0.1f)
        {
            positionComposer.Composition.ScreenPosition.x = -0.4f;
            // newCamera.targetScreenX = 0.25f; // 向右偏移，玩家在屏幕左侧
        }
        else if(player.facingDirection < -0.1)
        {
            positionComposer.Composition.ScreenPosition.x = 0.4f;
            // newCamera.targetScreenX = 0.75f; // 向左偏移，玩家在屏幕右侧
        }
    }

    private void SmoothCameraMove()
    {
        // 平滑过渡到目标 ScreenX
        positionComposer.Composition.ScreenPosition.x = Mathf.SmoothDamp(
            newCamera.temporaryScreenX, 
            positionComposer.Composition.ScreenPosition.x, 
            ref newCamera.currentVelocity, 
            newCamera.smoothTime);
    }
   
    public override void Exit()
    {
        base.Exit();
        Debug.LogWarning("exit grenade state");

        player.skill.grenadeSkill.DotsActive(false);
        player.skill.grenadeSkill.ResetGrenadeState();
        CameraManager.instance.newCamera.ResetZoom();
        //Reset aiming values when exiting
            player.anim.SetBool("AimGrenade", false);
         
        player.StartCoroutine(player.BusyFor(0.5f));
        playerData.isGrenadeState = false;
        // stateMachine.ChangeState(player.idleState);
        
        
        
       
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.SetIsBusy(true);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.SetIsBusy(false);
    }
}
// using Unity.Cinemachine;
// using System.Reflection;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class PlayerThrowGrenadeState : PlayerAbilityState
// {
//     private NewCamera newCamera;
//     private CinemachinePositionComposer positionComposer;
//     private float originalHorizontalScreenValue;
//     private Mouse mouse;
//
//     // Reflection cache
//     private static PropertyInfo horizontalGuideProperty;
//     private static PropertyInfo screenValueProperty;
//
//     public PlayerThrowGrenadeState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) 
//         : base(_player, _stateMachine, _playerData, _animBoolName)
//     {
//         mouse = Mouse.current;
//     }
//
//     public override void Enter()
//     {
//         base.Enter();
//         playerData.isGrenadeState = true;
//
//         if (CameraManager.instance.newCamera != null)
//         {
//             newCamera = CameraManager.instance.newCamera;
//
//             if (newCamera.TryGetComponent<CinemachinePositionComposer>(out positionComposer))
//             {
//                 Debug.Log("Found CinemachinePositionComposer!");
//                 
//                 // Initialize reflection properties if needed
//                 if (horizontalGuideProperty == null)
//                     horizontalGuideProperty = typeof(CinemachinePositionComposer).GetProperty("HorizontalGuide");
//                 
//                 if (horizontalGuideProperty != null)
//                 {
//                     var horizontalGuide = horizontalGuideProperty.GetValue(positionComposer);
//                     
//                     if (screenValueProperty == null)
//                         screenValueProperty = horizontalGuide.GetType().GetProperty("ScreenValue");
//
//                     if (screenValueProperty != null)
//                     {
//                         originalHorizontalScreenValue = (float)screenValueProperty.GetValue(horizontalGuide);
//                         newCamera.temporaryScreenX = originalHorizontalScreenValue;
//                     }
//                     else
//                     {
//                         Debug.LogError("ScreenValue property not found!");
//                     }
//                 }
//                 else
//                 {
//                     Debug.LogError("HorizontalGuide property not found!");
//                 }
//             }
//             else
//             {
//                 Debug.LogError("CinemachinePositionComposer not found on NewCamera!");
//             }
//         }
//
//         player.skill.grenadeSkill.DotsActive(true);
//
//         if (!playerData.isAiming && playerData.grenadeCanceled)
//         {
//             player.skill.grenadeSkill.DotsActive(false);
//             if (player.anim.GetBool("AimGrenade"))
//             {
//                 player.anim.SetBool("AimGrenade", false);
//                 stateMachine.ChangeState(player.idleState);
//             }
//         }
//     }
//
//     public override void Update()
//     {
//         base.Update();
//         rb.linearVelocity = Vector2.zero;
//
//         if (mouse.rightButton.isPressed)
//         {
//             if (playerData.isAiming)
//             {
//                 UpdateTargetScreenX();
//                 SmoothCameraMove();
//                 
//                 if (positionComposer != null && horizontalGuideProperty != null && screenValueProperty != null)
//                 {
//                     var horizontalGuide = horizontalGuideProperty.GetValue(positionComposer);
//                     screenValueProperty.SetValue(horizontalGuide, newCamera.temporaryScreenX);
//                     horizontalGuideProperty.SetValue(positionComposer, horizontalGuide);
//                 }
//             }
//         }
//
//         if (mouse.rightButton.wasReleasedThisFrame)
//         {
//             if (playerData.isAiming)
//             {
//                 // Handle aiming release
//             }
//             else
//             {
//                 stateMachine.ChangeState(player.idleState);
//             }
//         }
//
//         Vector2 mousePosition = mouse.position.ReadValue();
//         Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
//         if (player.transform.position.x > mouseWorldPosition.x && player.facingDirection == 1)
//         {
//             player.Flip();
//         }
//         else if (player.transform.position.x < mouseWorldPosition.x && player.facingDirection == -1)
//         {
//             player.Flip();
//         }
//     }
//
//     // Rest of the methods remain the same...
//     private void UpdateTargetScreenX()
//      {
//          // 根据玩家面向调整目标 ScreenX
//          if (player.facingDirection == 1)
//          {
//              
//              newCamera.targetScreenX = 0.25f; // 向右偏移，玩家在屏幕左侧
//          }
//          else if(player.facingDirection == -1)
//          {
//              
//              newCamera.targetScreenX = 0.75f; // 向左偏移，玩家在屏幕右侧
//          }
//      }
//
//      private void SmoothCameraMove()
//      {
//          // 平滑过渡到目标 ScreenX
//          newCamera.temporaryScreenX = Mathf.SmoothDamp(
//              newCamera.temporaryScreenX, 
//              newCamera.targetScreenX, 
//              ref newCamera.currentVelocity, 
//              newCamera.smoothTime);
//      }
//     
//     public override void Exit()
//     {
//         base.Exit();
//
//         // Reset using reflection
//         if (positionComposer != null && horizontalGuideProperty != null && screenValueProperty != null)
//         {
//             var horizontalGuide = horizontalGuideProperty.GetValue(positionComposer);
//             screenValueProperty.SetValue(horizontalGuide, originalHorizontalScreenValue);
//             horizontalGuideProperty.SetValue(positionComposer, horizontalGuide);
//         }
//
//         player.skill.grenadeSkill.DotsActive(false);
//         player.skill.grenadeSkill.ResetGrenadeState();
//         CameraManager.instance.newCamera.ResetZoom();
//         player.anim.SetBool("AimGrenade", false);
//         player.StartCoroutine(player.BusyFor(0.5f));
//         playerData.isGrenadeState = false;
//     }

   
