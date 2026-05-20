using UnityEngine;
using TMPro;

public class SeparationManager : MonoBehaviour
{
    [Header("Players")]
    public PlayerController player1;
    public PlayerController player2;

    [Header("Settings")]
    public float countdownDuration = 3f;
    public float respawnDelay      = 1f;

    [Header("UI")]
    private bool  isRespawning;
    // KillZone(트랩/낙사)에서 호출
    public void OnInstantKill()
    {
        if (!isRespawning) TriggerDeath();
    }

    void TriggerDeath()
    {
        isRespawning = true;

        player1.Die();
        player2.Die();

        Invoke(nameof(RespawnBoth), respawnDelay);
    }

    void RespawnBoth()
    {
        player1.Respawn();
        player2.Respawn();

        // HP 초기화 추가
        player1.GetComponent<PlayerHealth>()?.ResetHP();
        player2.GetComponent<PlayerHealth>()?.ResetHP();

        isRespawning = false;
    }
}