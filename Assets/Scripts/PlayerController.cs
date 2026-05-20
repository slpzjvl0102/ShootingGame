using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Input - Move")]
    public KeyCode leftKey  = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey  = KeyCode.W;
    public KeyCode downKey  = KeyCode.S;    // 조준 시 아래 이동

    [Header("Input - Aim")]
    public KeyCode aimKey = KeyCode.Space;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.15f;
    public LayerMask platformLayer;

    // 기존 Aim & Shoot 헤더 전체를 아래로 교체
    [Header("Aim & Shoot")]
    public GameObject crosshairPrefab;
    public float crosshairMoveSpeed = 5f;
    public PlayerHealth targetHealth;  // Inspector에서 상대 플레이어 연결
    public float hitRadius = 0.5f;     // 조준점 판정 범위

    private Rigidbody2D rb;
    private bool isAlive  = true;
    private bool isAiming = false;
    private GameObject crosshairObj;

    public bool IsGrounded             { get; private set; }
    public Collider2D StandingPlatform { get; private set; }

    private Vector3 respawnPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
    }

    void Start()
    {
        if (crosshairPrefab != null)
        {
            crosshairObj = Instantiate(crosshairPrefab);
            crosshairObj.SetActive(false);
        }
    }

    void Update()
    {
        if (!isAlive) return;
        UpdateGroundCheck();

        if (isAiming) HandleAimMode();
        else          HandleMoveMode();
    }

    // ── 이동 모드 ──────────────────────────────────────────
    void HandleMoveMode()
    {
        float dir = 0f;
        if (Input.GetKey(leftKey))  dir -= 1f;
        if (Input.GetKey(rightKey)) dir += 1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(jumpKey) && IsGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (Input.GetKeyDown(aimKey)) EnterAim();
    }

    // ── 조준 모드 ──────────────────────────────────────────
    void HandleAimMode()
    {
        // 수평 이동 잠금 (수직은 중력 유지)
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        float h = 0f, v = 0f;
        if (Input.GetKey(leftKey))  h -= 1f;
        if (Input.GetKey(rightKey)) h += 1f;
        if (Input.GetKey(jumpKey))  v += 1f;
        if (Input.GetKey(downKey))  v -= 1f;

        if (crosshairObj != null)
            crosshairObj.transform.position +=
                new Vector3(h, v, 0f) * crosshairMoveSpeed * Time.deltaTime;

        // 조준키 재입력 → 발사
        if (Input.GetKeyDown(aimKey))
        {
            Fire();
            ExitAim();
        }
    }

    void EnterAim()
    {
        isAiming = true;
        if (crosshairObj != null)
        {
            // 조준점 초기 위치: 플레이어 바로 옆
            crosshairObj.transform.position = transform.position + Vector3.right;
            crosshairObj.SetActive(true);
        }
    }

    void ExitAim()
    {
        isAiming = false;
        if (crosshairObj != null) crosshairObj.SetActive(false);
    }

    void Fire()
    {
        if (crosshairObj == null || targetHealth == null) return;

        float dist = Vector2.Distance(
            crosshairObj.transform.position,
            targetHealth.transform.position);

        if (dist <= hitRadius)
            targetHealth.TakeDamage(1);
    }

    // ── 공통 ───────────────────────────────────────────────
    void UpdateGroundCheck()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            groundCheckPoint.position, groundCheckRadius, platformLayer);
        IsGrounded       = hit != null;
        StandingPlatform = hit;
    }

    public void Die()
    {
        if (!isAlive) return;
        ExitAim();
        isAlive           = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated      = false;
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
        rb.linearVelocity  = Vector2.zero;
        rb.simulated       = true;
        isAlive            = true;
        gameObject.SetActive(true);
    }

    public void SetRespawnPoint(Vector3 pos) => respawnPosition = pos;

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}