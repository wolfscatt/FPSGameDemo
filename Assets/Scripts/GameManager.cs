using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion
    public float killCount = 0;
    public TextMeshProUGUI killCountText;
    public bool isGamePaused = false;
    public bool isLevelCleared = false;
    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject gamePausePanel;
    public TextMeshProUGUI winPanelKillCountText;

    private void Start() 
    {
        killCountText.text = "0";
        isLevelCleared = false;
        isGamePaused = false;
        Time.timeScale = 1;
    }
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isLevelCleared)
        {
            if(isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        var enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemyObjects.Length == 0 && !isLevelCleared)
        {
            LevelCleared();
        }
    }
    private void PauseGame()
    {
        gamePausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isGamePaused = true;
    }
    private void ResumeGame()
    {
        gamePausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isGamePaused = false;
    }
    public void LevelCleared()
    {
        gameWinPanel.SetActive(true);
        winPanelKillCountText.text = $"{killCount} aliens neutralized.";
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isLevelCleared = true;
    }

    // Button Functions
    #region Button Functions
    public void CreateNewLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
    public void AddKill()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
