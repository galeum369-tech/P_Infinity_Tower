using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("기초 물리 부품")]
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    public float moveSpeed = 8f;

    [Header("수직 기동 스탯")]
    public float jumpForce = 14f;
    public float fastFallSpeed = 18f;

    [Header("지면/발판 레이어 세팅")]
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    [Header("전투 모듈 링크")]
    [SerializeField] private GameObject weaponModule;

    private bool isWeaponActive = false;
    private bool isGrounded;
    private bool isOnPlatform;
    private bool isFastFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        rb.freezeRotation = true;

        if (GameManager.Instance != null && GameManager.Instance.currentZone == GameManager.GameZone.InTower)
        {
            ActivateBattleMode();
        }
        else
        {
            DeactivateBattleMode();
        }

        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractPressed += TryInteract;
            InputHandler.Instance.OnJumpPressed += HandleJump;
            InputHandler.Instance.OnFallPressed += HandleFall;
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.6f, 0), 0.15f, groundLayer);
        isOnPlatform = Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.6f, 0), 0.15f, platformLayer);

        if (isGrounded || isOnPlatform)
        {
            isFastFalling = false;
        }
    }

    void FixedUpdate()
    {
        if (isFastFalling)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            Vector2 input = InputHandler.Instance.MoveInput;
            rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleJump()
    {
        if ((isGrounded || isOnPlatform) && !isFastFalling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleFall()
    {
        if (isOnPlatform)
        {
            StartCoroutine(PassThroughPlatform());
        }
        else if (!isGrounded && !isOnPlatform)
        { // ★ 누락되었던 여는 중괄호 복구하여 컴파일 에러 완전 차단
            isFastFalling = true;
            rb.linearVelocity = new Vector2(0f, -fastFallSpeed);
        }
    }

    private IEnumerator PassThroughPlatform()
    {
        Collider2D platformCollider = Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.6f, 0), 0.15f, platformLayer);

        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }

    private void ActivateBattleMode()
    {
        isWeaponActive = true;
        if (weaponModule != null) weaponModule.SetActive(true);
        Debug.Log("오토마톤 전투 모드 가동. 연산 슬롯 동기화 시작.");
    }

    private void DeactivateBattleMode()
    {
        isWeaponActive = false;
        if (weaponModule != null) weaponModule.SetActive(false);
        Debug.Log("세이프존 진입. 정비 모드 가동.");
    }

    private void TryInteract()
    {
        if (!isWeaponActive)
        {
            Debug.Log("주변 오브젝트와 상호작용 시도.");
        }
    }

    void OnDestroy()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractPressed -= TryInteract;
            InputHandler.Instance.OnJumpPressed -= HandleJump;
            InputHandler.Instance.OnFallPressed -= HandleFall;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -0.6f, 0), 0.15f);
    }
}