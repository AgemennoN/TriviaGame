using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    private static DynamicBackground instance = null;

    public GameObject squarePrefab;
    private float spawnRangeX = 7;
    private float startDelay = 1;
    private float spawnInternal = 3;

    public static DynamicBackground Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(transform.gameObject);
        ToggledOn();
    }

    public void ToggledOn()
    {
        InvokeRepeating("SpawnSquareRandomly", startDelay, spawnInternal);
    }

    public void ToggledOff()
    {
        CancelInvoke("SpawnSquareRandomly");
    }

    void SpawnSquareRandomly()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), -10, 0);
        Instantiate(squarePrefab, spawnPos, squarePrefab.transform.rotation);
    }
}
