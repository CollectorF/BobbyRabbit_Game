using System.Collections;
using System.Collections.Generic;

public class Level
{
	public string Name;
	public string Difficulty;
	public float Timer;
    public bool IsLocked;
    public bool[] Obstacles;

    public Level(string name, string difficulty, float timer, bool isLocked, bool[] obstacles)
    {
        Name = name;
        Difficulty = difficulty;
        Timer = timer;
        IsLocked = isLocked;
        Obstacles = obstacles;
    }
}
