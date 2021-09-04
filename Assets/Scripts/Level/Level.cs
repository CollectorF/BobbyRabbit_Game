using System.Collections;
using System.Collections.Generic;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class Level
{
    public Difficulty Difficulty;
    public string DifficultyString;
	public float Timer;
    public bool IsOpen;
    public bool[] Obstacles;

    public Level(string difficultyString, float timer, bool isOpen, bool[] obstacles)
    {
        DifficultyString = difficultyString;
        Timer = timer;
        IsOpen = isOpen;
        Obstacles = obstacles;
        switch (DifficultyString)
        {
            case "Easy":
                Difficulty = Difficulty.Easy;
                break;
            case "Medium":
                Difficulty = Difficulty.Medium;
                break;
            case "Hard":
                Difficulty = Difficulty.Hard;
                break;
            default:
                break;
        }
    }
}
