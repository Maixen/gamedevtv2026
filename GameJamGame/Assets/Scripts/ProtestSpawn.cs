using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProtestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject protester;
    [SerializeField] private Vector2Int spawnAmount;
    [SerializeField] private Vector2 spawnRange;
    [SerializeField] private Vector2 spawnTime;
    [SerializeField] private Vector3[] goals;
    [SerializeField] private bool theyAreComing;
    private float timePassed;
    private float timeToWait;

    private void Start()
    {
        timeToWait = Random.Range(spawnTime.x,spawnTime.y);
    }
    void Update()
    {
        if (GameManager.paused)
            return;
        if (!theyAreComing)
            return;
        if(timePassed < timeToWait)
        {
            timePassed += Time.deltaTime;
            return;
        }
        timePassed = 0;
        timeToWait = Random.Range(spawnTime.x,spawnTime.y);
        for(int i = 0; i < Random.Range(spawnAmount.x,spawnAmount.y); i++)
        {
            ProtestBehaviour prot = Instantiate(protester, transform.position + new Vector3(Random.Range(spawnRange.x, spawnRange.y), 0, Random.Range(spawnRange.x, spawnRange.y)),Quaternion.identity).GetComponent<ProtestBehaviour>();
            prot.SetGoal(goals.ToList<Vector3>());
        }
    }
}
