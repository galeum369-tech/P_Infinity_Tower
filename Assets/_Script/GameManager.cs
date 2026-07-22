using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameZone { Title, Hub, InTower }

    [Header("게임 상태 데이터")]
    public GameZone currentZone = GameZone.Title;
    public int currentFloor = 0; // ★ 기본값을 0으로 세팅하여 안정성 확보
    public const int maxFloor = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("마도 시스템 가동 중지: 게임을 나갑니다.");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void LoadHubScene()
    {
        currentZone = GameZone.Hub;
        currentFloor = 0;
        SceneManager.LoadScene("HubScene");
        Debug.Log("허브 마을 진입 완료.");
    }

    public void EnterTower()
    {
        currentZone = GameZone.InTower;
        currentFloor = 0; // ★ 1에서 0으로 수정! 허브 포탈 타면 무조건 대기실(0층)부터 시작하게 조율
        SceneManager.LoadScene("TowerScene");
        Debug.Log("타워 로비 대기실 진입 완료.");
    }

    public void MoveToNextFloor()
    {
        currentZone = GameZone.InTower;
        currentFloor++;
        Debug.Log($"현재 {currentFloor}층 진입 시퀀스.");
        SceneManager.LoadScene("TowerScene");
    }

    public void ReturnToHub()
    {
        currentZone = GameZone.Hub;
        SceneManager.LoadScene("HubScene");
        Debug.Log("작전 종료. 허브 마을로 귀환했습니다.");
    }
}