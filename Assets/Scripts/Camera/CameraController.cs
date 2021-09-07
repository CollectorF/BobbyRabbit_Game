using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Range(1,5)]
    private float smoothFactor;
    [SerializeField]
    private Transform target;
    [SerializeField]
    [Range(-3.3f,-2f)]
    private float cameraZPosition = -3.2f;

    private Camera cameraMain;

    private void Awake()
    {
        cameraMain = Camera.main;
    }
    
    private void FixedUpdate()
    {
        FollowTarget();
    }

    internal void SetInitialCameraPosition()
    {
        cameraMain.transform.position = new Vector3(target.position.x, target.position.y, cameraZPosition);
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, cameraZPosition);
        Vector3 smoothPosition = Vector3.Lerp(cameraMain.transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        cameraMain.transform.position = smoothPosition;
    }
}
