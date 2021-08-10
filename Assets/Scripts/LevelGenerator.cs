using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

//[Serializable]
//public struct Tiles
//{
//    public string Name;
//    public GameObject TilePrefab;
//}

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Tile roadAngleLeftTile;
    [SerializeField]
    private Tile roadAngleRightTile;
    [SerializeField]
    private Tile roadEndTile;
    [SerializeField]
    private Tile roadPassTile;

    private const string roadAngleLeftCode = "0";
    private const string roadAngleRightCode = "1";
    private const string roadEndCode = "2";
    private const string roadPassCode = "3";

    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();

        string[][] jagged = readFile("Level1");

        for (int y = 0; y < jagged.Length; y++)
        {
            for (int x = 0; x < jagged[0].Length; x++)
            {
                switch (jagged[y][x])
                {
                    case roadAngleLeftCode:
                        tilemap.SetTile(new Vector3Int(x, 0, y), roadAngleLeftTile);
                        break;
                    case roadAngleRightCode:
                        tilemap.SetTile(new Vector3Int(x, 0, y), roadAngleRightTile);
                        break;
                    case roadEndCode:
                        tilemap.SetTile(new Vector3Int(x, 0, y), roadEndTile);
                        break;
                    case roadPassCode:
                        tilemap.SetTile(new Vector3Int(x, 0, y), roadPassTile);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetTile(Sprite roadAngleLeftPrefab, Vector3 vector3, Quaternion identity)
    {
        throw new NotImplementedException();
    }

    private string[][] readFile(string file)
    {
        string text = Resources.Load(file).ToString();
        //string text = textAsset.text;
        string[] lines = Regex.Split(text, "\r\n");
        int rows = lines.Length;

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], " ");
            levelBase[i] = stringsOfLine;
        }
        return levelBase;
    }
}
