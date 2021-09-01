using System.Collections;
using System.Collections.Generic;

public class Level
{
	public string Name;
	public string Difficulty;
	public float Timer;
    public bool[] Obstacles;

    public Level(string name, string difficulty, float timer, bool[] obstacles)
    {
        Name = name;
        Difficulty = difficulty;
        Timer = timer;
        Obstacles = obstacles;
    }
}
