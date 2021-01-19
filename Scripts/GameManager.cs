using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfAsteroids;
    public int levelNumber = 1;
    public GameObject asteroid;

    public void UpdateNumberOfAsteroids(int change)
    {
        numberOfAsteroids += change;
        if(numberOfAsteroids <= 0)
        {
            Invoke("StartNewLevel",2f);
        }
    }

    void StartNewLevel()
    {
        levelNumber ++;

        for (int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-12f,12f),10f);
            Instantiate(asteroid,spawnPosition,Quaternion.identity);
            numberOfAsteroids ++;
        }
    }

    public bool CheckForHighScore(int score)
    {
        int highScore = PlayerPrefs.GetInt("highscore");
        if(score > highScore)
        {
            return true;
        }
        return false;
    }
}
