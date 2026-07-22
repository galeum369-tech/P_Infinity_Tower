using UnityEngine;

public class TowerStartPortal : MonoBehaviour
{
    private bool isPlayerNearby = false;

    void Start()
    {
        // 인풋 핸들러 상호작용 키 이벤트 구독
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractPressed += TryStartRun;
        }
    }

    private void TryStartRun()
    {
        // 플레이어가 포탈 근처에 있고 E키를 누르면 진짜 1층 전장으로 진입
        if (isPlayerNearby)
        {
            Debug.Log("방어벽 해제. 무한의 탑 1층 공략 시퀀스를 가동합니다.");

            // 0층 대기실을 종료하고 다음 층(1층)으로 빌드업 명령
            GameManager.Instance.MoveToNextFloor();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("탑 진입 동력원 감지. [E] 키로 공략 시작 가능.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void OnDestroy()
    {
        // 메모리 누수 방지 이벤트 해제
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractPressed -= TryStartRun;
        }
    }
}