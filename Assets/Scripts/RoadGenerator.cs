using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] RoadPrefabs;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 1;
    private float speed = 0;
    public int maxRoadCount = 3;
    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
        StartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed == 0) return;
        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (roads[0].transform.position.x < -15)
        {
            Destroy(roads[0].gameObject);
            roads.RemoveAt(0);
            CreateNewRoad();
        }
    }

    public void StartLevel()
    {
        speed = maxSpeed;
    }

    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0].gameObject);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNewRoad();
        }
    }

    private void CreateNewRoad()
    {
        Vector3 pos = Vector3.zero;
        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(20, 0, 0);
        }
        GameObject go = Instantiate(RoadPrefabs[Random.Range(0, RoadPrefabs.Length)], pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }
}
