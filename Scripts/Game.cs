using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // esta es para crear escena

public class Game : MonoBehaviour
{
    public static Game obj;

    public int maxLives = 3;

    public bool gamePaused = false;
    public int score = 0;

    void Awake()
    {
        obj = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
        UIManager.obj.startGame();
    }

    public void addScore(int scoreGive)
    {
        score += scoreGive;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        obj = null;
        
    }
}
