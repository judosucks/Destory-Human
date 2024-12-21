using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Adjust : MonoBehaviour
{
    private Player player;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera grenadeCamera;
    public CinemachineVirtualCamera blackholeCamera;
    public CinemachineVirtualCamera thunderCamera;
    public CinemachineVirtualCamera grenadeExplodeFxCamera;
    public CinemachineFramingTransposer framingTransposer;
    [Header("camera info")]
    
    public float zoomSpeed = 10f;
    public float smoothTime = .01f; // 平滑过渡的时间
    public float temporaryScreenX; // 用于临时储存平滑过渡值
    [SerializeField]private float originalScreenX = 0.5f;
    public float targetScreenX;
    [SerializeField] private float blackholeScreenY =0.34f;
    private float originalScreenY = 0.5f;
    public float currentVelocity;
    [SerializeField]private float blackholeOrthoSize=4f;
    
    [SerializeField] private float grenadeOrthoSize = 1f;
    [SerializeField] private float playerOrthoSize = 2.28f;
    [SerializeField] private float thunderOrthoSize = 2.28f;
    [SerializeField] private float orthoSizeVelocity = 0f;
    [SerializeField] private float grenadeExplodeOrthoSize = 1f;
    
    
    
    protected void Awake()
    {
        grenadeCamera = GameObject.FindGameObjectWithTag("GrenadeCamera").GetComponent<CinemachineVirtualCamera>();
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        blackholeCamera = GameObject.FindGameObjectWithTag("BlackholeCamera").GetComponent<CinemachineVirtualCamera>();
        thunderCamera = GameObject.FindGameObjectWithTag("ThunderCamera").GetComponent<CinemachineVirtualCamera>();
        grenadeExplodeFxCamera = GameObject.FindGameObjectWithTag("GrenadeExplodeFxCamera")
            .GetComponent<CinemachineVirtualCamera>();
    }

    protected void Start()
    {
        player = PlayerManager.instance.player;
        if (player == null)
        {
            Debug.LogError("Player object is null. Ensure PlayerManager is correctly assigning the player reference.");
        }
        if (playerCamera != null)
        {
            framingTransposer = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            
        }
    }

    protected void Update()
    {
        
    }


    private void FollowPlayerWithSameCamera()
    {
        Debug.LogWarning("same camera found");
        playerCamera.Priority = 20;
        SmoothZoom(playerCamera, playerOrthoSize);
    }
    private void SetCameraPriority(CinemachineVirtualCamera mainCam, CinemachineVirtualCamera secondaryCam)
    {
        mainCam.Priority = 20;
        secondaryCam.Priority = 10;
    }
    public virtual void SmoothZoom(CinemachineVirtualCamera cam,float _targetOrthoSize)
    {
        float currentOrthoSize = cam.m_Lens.OrthographicSize;
        cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(
            currentOrthoSize, 
            _targetOrthoSize, 
            ref orthoSizeVelocity, 
            zoomSpeed);
    }

    public void ResetZoom()
    {
        
        framingTransposer.m_ScreenX = originalScreenX;
        framingTransposer.m_ScreenY = originalScreenY;
    }
    public void FollowGrenade()
    {
        
       
        SetCameraPriority(grenadeCamera, playerCamera);
        SmoothZoom(grenadeCamera, grenadeOrthoSize);
    }
    public virtual void FollowPlayer(CinemachineVirtualCamera _targetCam)
    {
        Debug.Log("follow player");
        if (_targetCam == playerCamera)
        {
            FollowPlayerWithSameCamera();
        }else if (_targetCam != playerCamera)
        {
          SetCameraPriority(playerCamera,_targetCam);
          SmoothZoom(playerCamera, playerOrthoSize);
        }
        
    }
    public virtual void FollowThunder()
    {
        Debug.Log("follow thunder");
        SetCameraPriority(thunderCamera, playerCamera);
        SmoothZoom(thunderCamera, thunderOrthoSize);
        
    }

    
    public virtual void SetScreenX(CinemachineVirtualCamera cam, float _targetScreenX, float smoothTime)
    {
        
        float currentScreenX = framingTransposer.m_ScreenX;
        float velocity = 0f;
        framingTransposer.m_ScreenX = Mathf.SmoothDamp(
            currentScreenX,
            _targetScreenX,
            ref velocity,
            smoothTime);
    }

    public virtual void SetScreenY(CinemachineVirtualCamera cam, float _targetScreenY, float smoothTime)
    {
     
        float currentScreenY = framingTransposer.m_ScreenY;
        float velocity = 0f;
        framingTransposer.m_ScreenY = Mathf.SmoothDamp(
            currentScreenY,
            _targetScreenY,
            ref velocity,smoothTime);
        
    }
    public virtual void FollowBlackhole()
    {
       Debug.Log("follow blackhole");
        SetCameraPriority(blackholeCamera, playerCamera);
        SmoothZoom(blackholeCamera, blackholeOrthoSize);
        
    }

    public virtual void FollowGrenadeExplosion()
    {
        Debug.Log("follow grenade explosionfx");
        CinemachineVirtualCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        if (currentCam != null)
        {
            Debug.Log($"current cam: {currentCam.name}");
        }
        else
        {
            Debug.LogWarning("no active camera");
        }
        SetCameraPriority(grenadeExplodeFxCamera, currentCam);
        SmoothZoom(grenadeExplodeFxCamera, grenadeExplodeOrthoSize);
    }
}
