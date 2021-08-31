using System.Collections;
using System.Collections.Generic;

public class Level
{
	public string name;
	public string difficulty;
	public float timer;
    public bool[] obstacles;

    public Level(string name, string difficulty, float timer, bool[] obstacles)
    {
        this.name = name;
        this.difficulty = difficulty;
        this.timer = timer;
        this.obstacles = obstacles;
    }
}
