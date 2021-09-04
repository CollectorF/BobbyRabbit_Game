using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private TilemapRenderer tilemap;

    private Camera cameraMain;
    private Vector2 mooveDirection;
    private Vector2 cameraBounds;
    public Vector2 tilemapBounds;

    private void Awake()
    {
        cameraMain = Camera.main;
        cameraBounds = cameraMain.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        tilemapBounds = new Vector2(tilemap.bounds.size.x / 2, tilemap.bounds.size.y / 2);
    }
    
    private void Update()
    {
        Look(mooveDirection);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mooveDirection = context.ReadValue<Vector2>();
    }

    private void Look(Vector2 direction)
    {
        Vector3 movementDirection = transform.up * direction.y;
        movementDirection += transform.right * direction.x;
        movementDirection.x = Mathf.Clamp(movementDirection.x, cameraBounds.x + tilemapBounds.x, cameraBounds.x * -1 - tilemapBounds.x);
        movementDirection.y = Mathf.Clamp(movementDirection.y, cameraBounds.y + tilemapBounds.y, cameraBounds.y * -1 - tilemapBounds.y);
        cameraMain.transform.position += movementDirection * moveSpeed * Time.deltaTime;
    }

    public Vector2 GetTilemapBounds()
    {
        tilemapBounds = new Vector2(tilemap.bounds.size.x / 2, tilemap.bounds.size.y / 2);
        return tilemapBounds;
    }
}
