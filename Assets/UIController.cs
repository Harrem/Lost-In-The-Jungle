using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject StartingPanel;
    public GameObject GameOverPanel;
    public GameObject WonPanel;


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        StartingPanel.SetActive(false);
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void Won()
    {
        WonPanel.SetActive(true);
    }
}
