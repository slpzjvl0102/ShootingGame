using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Players")]
    public PlayerHealth player1Health;
    public PlayerHealth player2Health;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnPlayerDied(PlayerHealth deadPlayer)
    {
        if (isGameOver) return;
        isGameOver = true;

        string winner = (deadPlayer == player1Health) ? "P2" : "P1";
        winnerText.text = $"{winner} Win!";
        gameOverPanel.SetActive(true);

        // 양쪽 입력 중단
        player1Health.GetComponent<PlayerController>()?.Die();
        player2Health.GetComponent<PlayerController>()?.Die();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}