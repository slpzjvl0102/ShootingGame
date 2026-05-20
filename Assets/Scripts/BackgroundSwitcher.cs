using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    [Header("Backgrounds")]
    public Sprite background1;
    public Sprite background2;

    [Header("Settings")]
    [Min(0.1f)]
    public float switchInterval = 3f;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool showingFirst = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.sprite = background1;
        FitToScreen();
        timer = switchInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            showingFirst = !showingFirst;
            spriteRenderer.sprite = showingFirst ? background1 : background2;
            FitToScreen(); // 스프라이트 교체 시 재계산
            timer = switchInterval;
        }
    }

    void FitToScreen()
    {
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize * 2f;
        float camWidth  = camHeight * cam.aspect;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = camWidth  / spriteSize.x;
        float scaleY = camHeight / spriteSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}