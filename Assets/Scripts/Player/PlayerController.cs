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

    [Space(20)]
    [SerializeField]
    private LevelLoader levelLoader;
    [SerializeField]
    private Tilemap tilemapBackground;

    [Space(20)]
    [SerializeField]
    private string NOWAY_KEY = "CantGoThere";

    internal event Action<string> OnNoWay;

    private CharacterController characterController;
    private Camera playerCamera;
    private AnimatorClipInfo[] currentClipInfo;
    private float characterSpeedCurrent;
    private float currentClipLength;
    private float currentTime = 0f;
    private Vector2 walkDirection;
    internal Vector2 currentPositionVector;
    private MapTile nextPosition;
    internal MapTile currentPosition;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        currentPositionVector = GetCurrentPosInVector(tilemapBackground);
        nextPosition = GetNextTile(tilemapBackground);
        Walk(walkDirection);
    }

    // Player movement logics
    private void Walk(Vector2 direction)
    {
        direction = walkDirection;
        if (nextPosition.Type == TileType.Walkable)
        {
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Horizontal", direction.x);
            Vector3 movementDirection = transform.up * direction.y;
            movementDirection += transform.right * direction.x;
            characterController.Move(movementDirection * characterSpeedCurrent * Time.deltaTime);
            if (direction == Vector2.zero ^ currentTime >= currentClipLength)
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
            OnNoWay?.Invoke(NOWAY_KEY);
        }
    }

    private Vector2 GetCurrentPosInVector(Tilemap tilemap)
    {
        currentPositionVector = new Vector2(tilemap.WorldToCell(transform.position).x, -tilemap.WorldToCell(transform.position).y);
        return currentPositionVector;
    }

    private MapTile GetNextTile(Tilemap tilemap)
    {
        //currentPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(currentPositionVector.x), Mathf.FloorToInt(currentPositionVector.y));
        nextPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(currentPositionVector.x + walkDirection.x), Mathf.FloorToInt(currentPositionVector.y - walkDirection.y));
        return nextPosition;
    }

    public void OnWalk(InputAction.CallbackContext value)
    {
        walkDirection = value.ReadValue<Vector2>();
    }


}
