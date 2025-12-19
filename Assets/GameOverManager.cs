using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    public GameObject gameOverPanel;
    public GameObject victoryText;
    public GameObject defeatText;
    public GameObject playAgainButton;

    void Awake()
    {
        Instance = this;

        gameOverPanel.SetActive(false);
        victoryText.SetActive(false);
        defeatText.SetActive(false);
        playAgainButton.SetActive(false);
    }

    public void ShowVictory()
    {
        Debug.Log("Victory!");
        gameOverPanel.SetActive(true);
        victoryText.SetActive(true);
        defeatText.SetActive(false);
        playAgainButton.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ShowDefeat()
    {
        gameOverPanel.SetActive(true);
        victoryText.SetActive(false);
        defeatText.SetActive(true);
        playAgainButton.SetActive(true);

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
