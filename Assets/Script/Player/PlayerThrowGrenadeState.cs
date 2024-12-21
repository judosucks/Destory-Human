using Cinemachine;
using UnityEngine;

public class PlayerThrowGrenadeState : PlayerState
{
    
    private NewCamera newCamera;
    
    public PlayerThrowGrenadeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();
        if (CameraManager.instance.newCamera != null)
        {
            newCamera = CameraManager.instance.newCamera;
            // newCamera.FollowPlayer();  // 确保摄像机跟随玩家并设置初始缩放
            // originalScreenX = CameraManager.instance.newCamera.GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
            if (CameraManager.instance.newCamera.GetComponentInChildren<CinemachineVirtualCamera>() != null)
            {
                Debug.Log("get cinemachinevirtualcamera");
                if (CameraManager.instance.newCamera.GetComponentInChildren<CinemachineVirtualCamera>()
                        .GetCinemachineComponent<CinemachineFramingTransposer>() != null)
                {
                    
                    newCamera.temporaryScreenX = CameraManager.instance.newCamera.GetComponentInChildren<CinemachineVirtualCamera>()
                        .GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
                  
                }
            } 
        }
        player.skill.grenadeSkill.DotsActive(true);
        
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        if (mouse.leftButton.wasPressedThisFrame && player.isAiming) // LMB cancels the throw
        {
            Debug.Log("left button pressed aimgrenade");
            
            
            player.CancelThrowGrenade(); // Destroy the grenade or cancel action
            Debug.Log("isaiming"+player.isAiming);
            return;
            // stateMachine.ChangeState(player.idleState); // Transition back to idle
            // ;
            // CameraManager.instance.newCamera.ResetZoom();
            
        }

        if (mouse.rightButton.wasReleasedThisFrame&&player.anim.GetBool("AimGrenade")) // RMB throws the grenade
        {
            Debug.Log("[PlayerThrowGrenadeState] Grenade thrown!");
            player.anim.SetTrigger("ThrowGrenade"); // Trigger throw animation
            player.skill.grenadeSkill.CreateGrenade(); // Executes grenade throwing logic
            // stateMachine.ChangeState(player.idleState); // Reset to idle
            // CameraManager.instance.newCamera.ResetZoom();
            player.anim.SetBool("AimGrenade", false);
            return;
        }

        if (player.isAiming)
        {
            Debug.Log("aiming ...changing camera if needed");
            UpdateTargetScreenX();
            SmoothCameraMove();
            CameraManager.instance.AdjustPlayerCameraScreenX(newCamera.temporaryScreenX, newCamera.smoothTime);
        
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
        else
        {
            Debug.Log("not aiming");
            player.isAiming = false;
        }
    }


    private void UpdateTargetScreenX()
    {
        // 根据玩家面向调整目标 ScreenX
        if (player.facingDirection == 1)
        {
            
            newCamera.targetScreenX = 0.25f; // 向右偏移，玩家在屏幕左侧
        }
        else if(player.facingDirection == -1)
        {
            
            newCamera.targetScreenX = 0.75f; // 向左偏移，玩家在屏幕右侧
        }
    }

    private void SmoothCameraMove()
    {
        // 平滑过渡到目标 ScreenX
        newCamera.temporaryScreenX = Mathf.SmoothDamp(
            newCamera.temporaryScreenX, 
            newCamera.targetScreenX, 
            ref newCamera.currentVelocity, 
            newCamera.smoothTime);
    }
   
    public override void Exit()
    {
        base.Exit();
        Debug.Log("exit throw sword state");
        player.StartCoroutine("BusyFor", .2f);
        
    }
}
