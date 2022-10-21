using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool _isGameOver;
    bool _isPausedGame = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P) && !_isPausedGame)
        {
            _isPausedGame = true;
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.P) && _isPausedGame)
        {
            _isPausedGame = false;
            Time.timeScale = 1f;
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
