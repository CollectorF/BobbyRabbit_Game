using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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
    [SerializeField]
    private LevelLoader levelLoader;
    [SerializeField]
    private Tilemap tilemapBackground;
    [SerializeField]
    private Tilemap tilemapMain;
    [SerializeField]
    private CaptionLibrary captionLibrary;

    internal event Action<string> OnNoWay;

    private CharacterController characterController;
    private Camera playerCamera;
    private AnimatorClipInfo[] currentClipInfo;
    private float characterSpeedCurrent;
    private float currentClipLength;
    private float currentTime = 0f;
    private Vector2 walkDirection;
    private Vector2 currentPositionVector;
    private MapTile currntPosition;
    private MapTile nextPosition;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {

        // Calcuating of current player position in grid coordinates and player's next position

        currentPositionVector = new Vector2(tilemapBackground.WorldToCell(transform.position).x, -tilemapBackground.WorldToCell(transform.position).y);
        currntPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(currentPositionVector.x), Mathf.FloorToInt(currentPositionVector.y));
        nextPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(currentPositionVector.x + walkDirection.x), Mathf.FloorToInt(currentPositionVector.y - walkDirection.y));

        // Player movement logics

        if (nextPosition.Type == TileType.Walkable)
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
        }
        else
        {
            animator.Play("Player_Idle", 0);
            captionLibrary.captionDictionary.TryGetValue("CantGoThere", out string msg);
            Debug.Log(msg);
            OnNoWay?.Invoke(msg);
        }
        //playerCamera.transform.LookAt(playerCameraTarget);
    }

    public void OnWalk(InputAction.CallbackContext value)
    {
        walkDirection = value.ReadValue<Vector2>();
    }
}
