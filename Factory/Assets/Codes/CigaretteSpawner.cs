
using UnityEngine;
using System.Collections;

public class CigaretteSpawner : MonoBehaviour
{
    public GameObject cigarettePrefab;

    [Header("Waves")]
    public int minCigarettes = 5;
    public int maxCigarettes = 8;
    public float waveInterval = 4f;
    public float rollingSpeed = 3f;
    public int maxCigarettesOnScreen = 60;

    [Header("Spawn Zones")]
    public float leftEdge = -10f;
    public float rightEdge = 10f;

    public Vector2 topY = new Vector2(3.5f, 4f);
    public Vector2 bottomY = new Vector2(-4f, -3.5f);

    public float targetMinX = -5f;
    public float targetMaxX = 5f;

    private int currentCigarettes;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    void SpawnWave()
    {
        int amount = Random.Range(minCigarettes, maxCigarettes + 1);

        for (int i = 0; i < amount; i++)
        {
            if (currentCigarettes >= maxCigarettesOnScreen)
                break;

            bool fromLeft = Random.value > 0.5f;
            bool top = Random.value > 0.5f;

            Vector2 yRange = top ? topY : bottomY;

            float y = Random.Range(yRange.x, yRange.y);
            float startX = fromLeft ? leftEdge : rightEdge;

            Vector3 startPos = new Vector3(startX, y, 0f);
            Vector3 targetPos = new Vector3(
                Random.Range(targetMinX, targetMaxX),
                y,
                0f
            );

            GameObject cigarette = Instantiate(
                cigarettePrefab,
                startPos,
                Quaternion.Euler(0, 0, Random.Range(0f, 360f))
            );

            CigaretteRolling rolling =
                cigarette.GetComponent<CigaretteRolling>();

            if (rolling != null)
                rolling.Initialize(targetPos, rollingSpeed);

            currentCigarettes++;
        }
    }

    public void CigaretteUsed()
    {
        currentCigarettes = Mathf.Max(0, currentCigarettes - 1);
    }
}
