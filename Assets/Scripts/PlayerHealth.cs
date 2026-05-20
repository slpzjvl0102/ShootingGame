using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 3;

    public int CurrentHP { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor  = spriteRenderer.color; // 원래 색 저장
        CurrentHP      = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHP <= 0) return;

        CurrentHP -= amount;
        StartCoroutine(HitFlash());

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            GameManager.Instance?.OnPlayerDied(this);
        }
    }

    public void ResetHP() => CurrentHP = maxHP;

    IEnumerator HitFlash()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = originalColor;
    }
}