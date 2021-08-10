using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float characterSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationCurve speedCurve;
    //[SerializeField]
    //private Transform playerCameraTarget;

    private CharacterController characterController;
    private Camera playerCamera;
    private Vector2 walkDirection;
    private float characterSpeedCurrent;
    private AnimatorClipInfo[] currentClipInfo;
    private float currentClipLength;
    private float currentTime = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        animator.SetFloat("Vertical", walkDirection.y);
        animator.SetFloat("Horizontal", walkDirection.x);
        Vector3 movementDirection = transform.up * walkDirection.y;
        movementDirection += transform.right * walkDirection.x;
        characterController.Move(movementDirection * characterSpeedCurrent * Time.deltaTime);
        if (walkDirection == Vector2.zero ^ currentTime >= currentClipLength)
        {
            currentTime = 0f;
        }
        else
        {
            currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            currentClipLength = currentClipInfo[0].clip.length;
            currentTime = Mathf.Clamp(currentTime, 0, currentClipLength);
            characterSpeedCurrent = speedCurve.Evaluate(currentTime) * characterSpeed;
            currentTime += Time.deltaTime;
        }

        //playerCamera.transform.LookAt(playerCameraTarget);
    }

    private void LateUpdate()
    {

    }

    public void OnWalk(InputAction.CallbackContext value)
    {
        walkDirection = value.ReadValue<Vector2>();
    }
}
