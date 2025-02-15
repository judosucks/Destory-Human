using Cinemachine;
using UnityEngine;

public class PlayerThrowGrenadeState : PlayerState
{
    
    private NewCamera newCamera;
   
    public PlayerThrowGrenadeState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine,_playerData, _animBoolName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isGrenadeState = true;
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
        
        if (!playerData.isAiming && playerData.grenadeCanceled)
        {
            Debug.Log("grenade canceled not aiming");
            player.skill.grenadeSkill.DotsActive(false);
            if (player.anim.GetBool("AimGrenade"))
            {
                Debug.Log("grenade canceled not aiming");
             player.anim.SetBool("AimGrenade",false);
             stateMachine.ChangeState(player.idleState);
            }
            
        }
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();

        if (mouse.rightButton.isPressed && playerData.isAiming)
        {
            Debug.Log("right button pressed from throw grenade");
            UpdateTargetScreenX();
            SmoothCameraMove();
            CameraManager.instance.AdjustPlayerCameraScreenX(newCamera.temporaryScreenX, newCamera.smoothTime);
        }

        if (mouse.rightButton.wasReleasedThisFrame)
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

        player.skill.grenadeSkill.DotsActive(false);
        player.skill.grenadeSkill.ResetGrenadeState();
        CameraManager.instance.newCamera.ResetZoom();
        //Reset aiming values when exiting
            player.anim.SetBool("AimGrenade", false);
         
        player.StartCoroutine(player.BusyFor(0.5f));
        playerData.isGrenadeState = false;
        // stateMachine.ChangeState(player.idleState);
        
        
        
       
    }
}
