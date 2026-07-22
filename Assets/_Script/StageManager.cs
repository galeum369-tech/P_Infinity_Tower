using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Å¸¿ö ·Îºñ ´ë±â½Ç ¸Ê ÇÁ¸®ÆÕ (0Ãþ)")]
    [SerializeField] private GameObject towerHubPrefab;

    [Header("°íÁ¤ Ãþ¼ö ¸Ê ÇÁ¸®ÆÕ")]
    [SerializeField] private GameObject floor1Prefab;
    [SerializeField] private GameObject floor5Prefab;

    [Header("2~4Ãþ ·£´ý ¸Ê ÇÁ¸®ÆÕ Ç®")]
    [SerializeField] private GameObject[] randomMapPrefabs;

    [Header("ÀÎ°ÔÀÓ ±âÃ¼ ¸µÅ©")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject dummyBossPrefab;

    void Start()
    {
        GenerateStage();
    }

    private void GenerateStage()
    {
        int currentFloor = GameManager.Instance.currentFloor;
        GameObject targetMapPrefab = null;

        if (currentFloor == 0)
        {
            targetMapPrefab = towerHubPrefab;
        }
        else if (currentFloor == 1)
        {
            targetMapPrefab = floor1Prefab;
        }
        else if (currentFloor == GameManager.maxFloor) // ¡Ú ½Ì±ÛÅæÀÇ »ó¼ö µ¥ÀÌÅÍ ¾ÈÀü ÂüÁ¶ È®ÀÎ
        {
            targetMapPrefab = floor5Prefab;
        }
        else
        {
            if (randomMapPrefabs != null && randomMapPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, randomMapPrefabs.Length);
                targetMapPrefab = randomMapPrefabs[randomIndex];
            }
        }

        if (targetMapPrefab == null) return;

        GameObject spawnedMap = Instantiate(targetMapPrefab, Vector3.zero, Quaternion.identity);

        Transform playerSpawnPoint = spawnedMap.transform.Find("PlayerSpawnPoint");
        Transform bossSpawnPoint = spawnedMap.transform.Find("BossSpawnPoint");

        if (playerSpawnPoint != null && playerPrefab != null)
        {
            Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        }

        if (currentFloor > 0 && bossSpawnPoint != null && dummyBossPrefab != null)
        {
            Instantiate(dummyBossPrefab, bossSpawnPoint.position, Quaternion.identity);
        }
    }
}