using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public Chunk[] RoadPrefabs;
    public Transform RoadStart;
    public Transform player;
    private List<Chunk> spawnedRoads = new List<Chunk>();

    void Start()
    {
        StartSpawn();
    }

    void Update()
    {
        if (player.position.x > spawnedRoads[spawnedRoads.Count - 1].end.position.x - 10)
        {
            SpawnRoad();
        }
    }

    public void StartSpawn()
    {
        for (int i = 0; i < spawnedRoads.Count; i++)
        {
            Destroy(spawnedRoads[i].gameObject);
        }
        spawnedRoads.Clear();
        SpawnRoad();
    }

    private void SpawnRoad()
    {
        Chunk newRoad = Instantiate(RoadPrefabs[Random.Range(0, RoadPrefabs.Length)]);
        newRoad.transform.SetParent(transform);
        if (spawnedRoads.Count != 0)
        {
            newRoad.transform.position = spawnedRoads[spawnedRoads.Count - 1].end.position - newRoad.begin.localPosition;
        }
        else
        {
            newRoad.transform.position = RoadStart.transform.position;
        }
        spawnedRoads.Add(newRoad);

        if (spawnedRoads.Count >= 3)
        {
            Destroy(spawnedRoads[0].gameObject);
            spawnedRoads.RemoveAt(0);
        }
    }
}
