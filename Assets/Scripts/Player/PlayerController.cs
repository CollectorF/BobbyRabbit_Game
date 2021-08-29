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
    private LevelLoader levelLoaderBackground;
    [SerializeField]
    private LevelLoader levelLoaderMain;
    [SerializeField]
    private Tilemap tilemapBackground;
    [SerializeField]
    private Tilemap tilemapMain;
    [SerializeField]
    [Tooltip("Vector Comaration Tolerance")]
    private float tolerance = 0.1f;

    [Space(20)]
    [SerializeField]
    private string NO_WAY_KEY = "CantGoThere";

    internal event Action<string> OnNoWay;

    private CharacterController characterController;
    private Camera playerCamera;
    private AnimatorClipInfo[] currentClipInfo;
    private float characterSpeedCurrent;
    private float currentClipLength;
    private float currentTime = 0f;
    private Vector2 walkDirection;
    internal Vector2 currentPositionVector;
    private MapTile nextPositionBackground;
    private MapTile nextPositionMain;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        currentPositionVector = GetCurrentPosInVector(tilemapBackground);
        if (walkDirection == Vector2.zero)
        {
            nextPositionBackground = GetNextTile(levelLoaderBackground, currentPositionVector);
        }
        else
        {
            MapTile currentTile = GetCurrentTile(levelLoaderMain, currentPositionVector);
            Vector2 currentTileCenter = levelLoaderBackground.map.GetTileCenter(tilemapBackground, currentTile);
            Vector2 currentPlayerPos = characterController.transform.position;
            //if (CompareVectorsWithTolerance(currentTileCenter, currentPlayerPos, tolerance))
            if ((currentTileCenter.x - currentPlayerPos.x) < 0.01 & (currentTileCenter.y - currentPlayerPos.y) < 0.05)
            {
                nextPositionBackground = GetNextTile(levelLoaderBackground, currentPositionVector);
                nextPositionMain = GetNextTile(levelLoaderMain, currentPositionVector);
            }
        }
        Walk(walkDirection);
    }

    private void Walk(Vector2 direction)
    {
        if (nextPositionBackground.Type == TileType.Walkable && nextPositionMain.Type != TileType.Obstacle)
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
        var nextPosition = levelLoader.map.GetTileAt(Mathf.FloorToInt(position.x + walkDirection.x), Mathf.FloorToInt(position.y - walkDirection.y));
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
        if (walkDirection.x != 0 && walkDirection.y != 0)
        {
            walkDirection = Vector2.zero;
        }
    }

    public bool CompareVectorsWithTolerance(Vector2 a, Vector2 b, float tolerance)
    {
        return Vector2.SqrMagnitude(a - b) < tolerance;
    }
}
