using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacle;
    public float zSpawn = 20;
    private float obstacleLength = 15;
    private int numberOfObstacle = 6;
    public Transform playerTransform;
    private int obstacleType;

    private List<GameObject> activeObstacle = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < numberOfObstacle; i++)
        {
            SpawnObstacle(Random.Range(0, obstacle.Length));
        }

    }

    // Update is called once per frame
    void Update()
    {
        ConditionTile();

    }

    public void SpawnObstacle(int obstacleIndex)
    {
        float[] obsPositionX = { -2f, 0f, 2f };
        int indexObs = Random.Range(0, obsPositionX.Length);
        Vector3 obsPosition = new Vector3(obsPositionX[indexObs], 0, zSpawn);

        GameObject go = Instantiate(obstacle[obstacleIndex], obsPosition, transform.rotation);
        activeObstacle.Add(go);
        zSpawn += obstacleLength;
    }

    private void DeleteTile()
    {
        Destroy(activeObstacle[0], 2);
        activeObstacle.RemoveAt(0);
    }

    private void ConditionTile()
    {
        if (playerTransform.position.z - obstacleLength > zSpawn - (numberOfObstacle * obstacleLength))
        {
            SpawnObstacle(Random.Range(0, obstacle.Length));
            DeleteTile();
        }
    }
}