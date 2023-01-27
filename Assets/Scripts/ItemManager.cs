using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] item;
    public float zSpawn = 20;
    private float itemLength = 15;
    private int numberOfitem = 6;
    public Transform playerTransform;
    private int itemType;

    private List<GameObject> activeitem = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < numberOfitem; i++)
        {
            Spawnitem(Random.Range(0, item.Length));
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        ConditionTile();

    }

    public void Spawnitem(int itemIndex)
    {
        float[] obsPositionX = { -2f, 0f, 2f };
        int indexObs = Random.Range(0, obsPositionX.Length);
        Vector3 obsPosition = new Vector3(obsPositionX[indexObs], 0, zSpawn + 5);

        GameObject go = Instantiate(item[itemIndex], obsPosition, transform.rotation);
        activeitem.Add(go);
        zSpawn += itemLength;
    }

    private void DeleteTile()
    {
        Destroy(activeitem[0], 2);
        activeitem.RemoveAt(0);
    }

    private void ConditionTile()
    {
        if (playerTransform.position.z - itemLength > zSpawn - (numberOfitem * itemLength))
        {
            Spawnitem(Random.Range(0, item.Length));
            DeleteTile();
        }
    }

}
