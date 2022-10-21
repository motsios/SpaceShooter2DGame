using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText, _bestScoreText;
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _livesImage;
    [SerializeField] TMP_Text _gameOverText;
    [SerializeField] TMP_Text _restartText;
    GameManager gameManager;
    int bestScore;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreText.text = "Score: " + bestScore;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void CheckForBestScore(int playerScore)
    {
        if (playerScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", playerScore);
            _bestScoreText.text = "Best: " + playerScore;
        }
    }


    public void UpdateLives(int currentLives)
    {
        if (currentLives <= 0)
        {
            _livesImage.sprite = _liveSprites[0];
        }
        else
        {
            _livesImage.sprite = _liveSprites[currentLives];
        }
        if (currentLives <= 0)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            gameManager.GameOver();
            StartCoroutine(GameOverFlickerRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
