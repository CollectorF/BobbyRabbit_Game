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
    [SerializeField]
    [Tooltip("Use for dedug")]
    private bool enableKeyboardInput;

    [Space(20)]
    [SerializeField]
    private LevelLoader levelLoaderMain;
    [SerializeField]
    private Tilemap tilemapMain;

    [Space(20)]
    [SerializeField]
    private string NO_WAY_KEY = "CantGoThere";

    internal event Action<string> OnNoWay;

    private CharacterController characterController;
    private AnimatorClipInfo[] currentClipInfo;
    private float characterSpeedCurrent;
    private float currentClipLength;
    private float currentTime = 0f;
    private Vector2 walkDirection;
    internal Vector2 currentPositionVector;
    private MapTile nextPosition;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (levelLoaderMain.map != null)
        {
            currentPositionVector = GetCurrentPosInVector(tilemapMain);
            nextPosition = GetNextTile(levelLoaderMain, currentPositionVector);
            Walk(walkDirection, nextPosition);
        }
    }

    public void SetPlayerInitialPosition()
    {
        MapTile startTile = levelLoaderMain.map.GetSingleTileByType(TileType.StartPoint);
        Vector2 playerStartPoint = levelLoaderMain.map.GetTileCenter(tilemapMain, startTile);
        transform.position = playerStartPoint;
    }

    private void Walk(Vector2 direction, MapTile nextTile)
    {
        if (nextTile.Type != TileType.InteractiveObstacle && nextTile.Type != TileType.Obstacle)
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
            OnNoWay?.Invoke(NO_WAY_KEY);
        }
    }

    private Vector2 GetCurrentPosInVector(Tilemap tilemap)
    {
        currentPositionVector = new Vector2(tilemap.WorldToCell(transform.position).x, -tilemap.WorldToCell(transform.position).y);
        return currentPositionVector;
    }

    private MapTile GetNextTile(LevelLoader levelLoader, Vector2 position)
    {
        MapTile nextPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(position.x + walkDirection.x), Mathf.FloorToInt(position.y - walkDirection.y));
        return nextPosition;
    }

    internal MapTile GetCurrentTile(LevelLoader levelLoader, Vector2 position)
    {
        var currentPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        return currentPosition;
    }

    public void OnWalk(InputAction.CallbackContext value)
    {
        walkDirection = value.ReadValue<Vector2>();
    }

    public void OnWalkDebug(InputAction.CallbackContext value)
    {
        if (enableKeyboardInput)
        {
            walkDirection = value.ReadValue<Vector2>();
        }
    }
}
