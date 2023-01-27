using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tiles;
    public float zSpawn = 0;
    private float tileLength = 165;
    private int numberOfTiles = 2;
    public Transform playerTransform;
    private int mapType = 1;

    private List<GameObject> activeTiles = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(i % 2);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ConditionTile();
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0], 2);
        activeTiles.RemoveAt(0);
    }

    private void ConditionTile()
    {
        if (playerTransform.position.z - tileLength + 5 > zSpawn - (numberOfTiles * tileLength))
        {
            if (mapType == 1)
            {
                SpawnTile(0);
                mapType = 2;
            }
            else
            {
                SpawnTile(1);
                mapType = 1;
            }
            DeleteTile();
        }
    }

}
