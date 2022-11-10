using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void OnEnable()
    {
        GameManager.OnCompleted += Detach;
    }

    private void OnDisable()
    {
        GameManager.OnCompleted -= Detach;
    }

    private void Detach()
    {
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;
    }
}