using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Players")]
    public PlayerHealth player1Health;
    public PlayerHealth player2Health;

    [Header("HP Bar Images (Fill)")]
    public Image player1Bar;
    public Image player2Bar;

    void Update()
    {
        if (player1Health != null && player1Bar != null)
            player1Bar.fillAmount = (float)player1Health.CurrentHP / player1Health.maxHP;

        if (player2Health != null && player2Bar != null)
            player2Bar.fillAmount = (float)player2Health.CurrentHP / player2Health.maxHP;
    }
}