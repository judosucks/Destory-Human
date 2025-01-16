using Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public NewCamera newCamera;
    private CinemachineBrain brain;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        if (newCamera == null)
        {
            Debug.Log("newcamera is null");
            newCamera = GameObject.FindWithTag("Camera").GetComponent<NewCamera>();
            Debug.Log("newcamera"+newCamera);
        }else if(newCamera != null)
        {
            Debug.Log("newcamera is not null");
        }

        brain = newCamera.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("CinemachineBrain is null");
        }
    }
    public void AdjustGrenadeCameraScreenX(float targetScreenX, float smoothTime)
    {
        if (newCamera != null)
        {
            newCamera.SetScreenX(newCamera.grenadeCamera, targetScreenX, smoothTime);
        }
    }

    public void AdjustPlayerCameraScreenX(float targetScreenX, float smoothTime)
    {
        if (newCamera != null)
        {
            
            newCamera.SetScreenX(newCamera.playerCamera, targetScreenX, smoothTime);
        }
    }

    public CinemachineVirtualCamera GetCurrentActiveCamera()
    {
        if (brain != null && brain.ActiveVirtualCamera is CinemachineVirtualCamera activeVirtualCamera)
        {
            return activeVirtualCamera;
        }
        return null; // no active camera
    }
    // Method to shake the camera
    public void ShakeCamera(float intensity, float duration)
    {
        CinemachineVirtualCamera currentCamera = GetCurrentActiveCamera();

        if (currentCamera == null)
        {
            Debug.LogError("No active camera found for ShakeCamera!");
            return;
        }

        CinemachineBasicMultiChannelPerlin noise =
            currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
        {
            // Add the noise component if it doesn't exist
            noise = currentCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Set noise parameters (for shake effect)
        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = 2.0f; // Frequency controls the aggression of the shake

        // Stop shaking after the duration
        instance.StartCoroutine(StopShake(noise, duration));
    }

    private static IEnumerator StopShake(CinemachineBasicMultiChannelPerlin noise, float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

    
}
