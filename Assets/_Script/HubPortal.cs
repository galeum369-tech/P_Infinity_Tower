using UnityEngine;

public class HubPortal : MonoBehaviour
{
    private bool isPlayerNearby = false;

    void Start()
    {
        InputHandler.Instance.OnInteractPressed += CheckAndEnterTower;
    }

    void OnDestroy()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractPressed -= CheckAndEnterTower;
        }
    }

    private void CheckAndEnterTower()
    {
        if (isPlayerNearby)
        {
            Debug.Log("포탈 가동. 탑 내부 전장으로 시퀀스 전환.");
            GameManager.Instance.EnterTower();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("플레이어 포탈 접근. [E] 키로 탑 진입 가능.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("플레이어 포탈에서 멀어짐.");
        }
    }
}