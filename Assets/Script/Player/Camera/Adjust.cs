using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class Adjust : MonoBehaviour
{
    protected Player player;
    public NewCamera newCamera;
    public GameObject blackholePrefab;
    public CinemachineCamera playerCamera;
    public CinemachineCamera grenadeCamera;
    public CinemachineCamera blackholeCamera;
    public CinemachineCamera thunderCamera;
    public CinemachineCamera grenadeExplodeFxCamera;
    public CinemachinePositionComposer positionComposer;
    [Header("camera info")]
    public NoiseSettings noiseProfile; // 设置一个 Noise Profile 的引用
    public CinemachineBasicMultiChannelPerlin noise;
    public float zoomSpeed = 10f;
    public float smoothTime = .01f; // 平滑过渡的时间
    public float temporaryScreenX; // 用于临时储存平滑过渡值
    [SerializeField] private float originalScreenX = 0f;
    public float targetScreenX;
    [SerializeField] private float blackholeScreenY = 0.34f;
    private float originalScreenY = 0f;
    public float currentVelocity;
    [SerializeField] private float blackholeOrthoSize = 4f;

    [SerializeField] private float grenadeOrthoSize = 1f;
    [SerializeField] private float playerOrthoSize = 2.28f;
    [SerializeField] private float thunderOrthoSize = 2.28f;
    [SerializeField] private float orthoSizeVelocity = 0f;
    [SerializeField] private float grenadeExplodeOrthoSize = 1f;
    
    protected virtual void Awake()
    {
        grenadeCamera = GameObject.FindGameObjectWithTag("GrenadeCamera").GetComponent<CinemachineCamera>();
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineCamera>();
        blackholeCamera = GameObject.FindGameObjectWithTag("BlackholeCamera").GetComponent<CinemachineCamera>();
        thunderCamera = GameObject.FindGameObjectWithTag("ThunderCamera").GetComponent<CinemachineCamera>();
        grenadeExplodeFxCamera = GameObject.FindGameObjectWithTag("GrenadeExplodeFxCamera")
            .GetComponent<CinemachineCamera>();
        
    }

    protected virtual void Start()
    {
        
        player = PlayerManager.instance.player;
        newCamera = CameraManager.instance.newCamera;
        if (player == null)
        {
            Debug.LogError("Player object is null. Ensure PlayerManager is correctly assigning the player reference.");
        }
        if (newCamera != null)
        {
            Debug.Log("newCamera is not null");
            positionComposer = CameraManager.instance.GetCurrentActiveCamera().GetComponent<CinemachinePositionComposer>();
            if(positionComposer == null)
            {
                Debug.LogError("CinemachinePositionComposer not found on playerCamera!");
            }
           
        }
        originalScreenX = positionComposer.Composition.ScreenPosition.x;
        originalScreenY = positionComposer.Composition.ScreenPosition.y;
    }

    protected virtual void Update()
    {

    }






    private void SetCameraPriority(CinemachineCamera mainCam, CinemachineCamera secondaryCam)
    {
        Debug.Log("set camera priority");
        if (mainCam == secondaryCam)
        {
            Debug.LogWarning("same camera found");
            return;
        }
        mainCam.Priority = 20;
        if (secondaryCam != null)
        {
            secondaryCam.Priority = 10;
        }
    }
    public virtual void SmoothZoom(CinemachineCamera cam, float _targetOrthoSize)
    {
        // Correct way to access OrthographicSize
        float currentOrthoSize = cam.Lens.OrthographicSize;
        cam.Lens.OrthographicSize = Mathf.SmoothDamp(
            currentOrthoSize,
            _targetOrthoSize,
            ref orthoSizeVelocity,
            zoomSpeed);
    }

    public void ResetZoom()
    {
        // You need to get the CinemachinePositionComposer

        if (CameraManager.instance.GetCurrentActiveCamera().TryGetComponent<CinemachinePositionComposer>(out CinemachinePositionComposer composer))
        {
            if (CameraManager.instance.GetCurrentActiveCamera().name == newCamera.name)
            {
                if (positionComposer != null)
                {
                    positionComposer.Composition.ScreenPosition.x = originalScreenX;
                    positionComposer.Composition.ScreenPosition.y = originalScreenY;
                    SmoothZoom(CameraManager.instance.GetCurrentActiveCamera(), CameraManager.instance.GetCurrentActiveCamera().Lens.OrthographicSize);
                }
                Debug.Log("reset zoom");
            }
            
        }
        else
        {
            Debug.LogError("CinemachinePositionComposer not found on playerCamera!");
        }
    }
    public void FollowGrenade()
    {
        CinemachineCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();

        Debug.Log("follow grenade" + currentCam.name);
        if (grenadeCamera != currentCam)
        {
            SetCameraPriority(grenadeCamera, currentCam);
            SmoothZoom(grenadeCamera, grenadeOrthoSize);
        }
    }
    public virtual void FollowPlayer()
    {
        CinemachineCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        Debug.Log("follow player" + " " + currentCam.name);
        if (currentCam != playerCamera)
        {
            SetCameraPriority(playerCamera, currentCam);
            SmoothZoom(playerCamera, playerOrthoSize);
        }

    }
    public virtual void FollowThunder()
    {
        CinemachineCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        Debug.Log("follow thunder" + currentCam.name);
        if (thunderCamera != currentCam)
        {
            SetCameraPriority(thunderCamera, currentCam);
            SmoothZoom(thunderCamera, thunderOrthoSize);
        }

    }


    public virtual void SetScreenX(CinemachineCamera cam, float _targetScreenX, float smoothTime)
    {
        // You need to get the CinemachinePositionComposer
        if (cam.TryGetComponent<CinemachinePositionComposer>(out CinemachinePositionComposer composer))
        {
            if (composer != null)
            {
                float currentScreenX = composer.Composition.ScreenPosition.x;
                float velocity = 0f;
                composer.Composition.ScreenPosition.x = Mathf.SmoothDamp(
                    currentScreenX,
                    _targetScreenX,
                    ref velocity, smoothTime);
            }
            

        }
        else
        {
            Debug.LogError("CinemachinePositionComposer not found on the camera!");
        }
    }

    public virtual void SetScreenY(CinemachineCamera cam, float _targetScreenY, float smoothTime)
    {
        // You need to get the CinemachinePositionComposer
        if (cam.TryGetComponent<CinemachinePositionComposer>(out CinemachinePositionComposer composer))
        {
            if (composer != null)
            {
                float currentScreenY = composer.Composition.ScreenPosition.y;
                float velocity = 0f;
                composer.Composition.ScreenPosition.y = Mathf.SmoothDamp(
                    currentScreenY,
                    _targetScreenY,
                    ref velocity, smoothTime);
            }

            else
            {
                Debug.LogError("CinemachinePositionComposer not found on the camera!");
            }

        }
    }

    public virtual void FollowBlackhole()
    {
        CinemachineCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        Debug.Log("follow blackhole" + currentCam.name);
        if (blackholeCamera != currentCam)
        {
            SetCameraPriority(blackholeCamera, currentCam);
            SmoothZoom(blackholeCamera, blackholeOrthoSize);
        }

    }

    public virtual void FollowGrenadeExplosion()
    {
        CinemachineCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        Debug.Log("follow grenade explosionfx" + currentCam.name);
        if (grenadeExplodeFxCamera != currentCam)
        {
            SetCameraPriority(grenadeExplodeFxCamera, currentCam);
            SmoothZoom(grenadeExplodeFxCamera, grenadeExplodeOrthoSize);
        }
    }
    public void ShakeCamera(float intensity, float duration)
    {
        Debug.Log("shake camera");
        CinemachineCamera currentCamera = CameraManager.instance.GetCurrentActiveCamera();
        Debug.Log(currentCamera.name);
        if (currentCamera == null)
        {
            Debug.LogError("No active camera found for ShakeCamera!");
            return;
        }

        CinemachineBasicMultiChannelPerlin noise =
            currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
        {
            // Add the noise component if it doesn't exist
            noise = currentCamera.AddComponent<CinemachineBasicMultiChannelPerlin>();
        }

       noise.NoiseProfile = noiseProfile; // Set the noise profile
       noise.AmplitudeGain = intensity;
        noise.FrequencyGain = 1f; // Set the frequency gain

        // Stop shaking after the duration
        StartCoroutine(StopShake(noise, duration));
    }

    private IEnumerator StopShake(CinemachineBasicMultiChannelPerlin noise, float duration)
    {
        Debug.Log("stop shake");
        yield return new WaitForSeconds(duration);
        noise.AmplitudeGain = 0f; // Reset the amplitude to stop shaking
        noise.FrequencyGain = 0f; // Reset the frequency to stop shaking

       
    }

}