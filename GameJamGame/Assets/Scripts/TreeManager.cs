using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trees;
    [SerializeField] private GameObject[] poleTops;

    public GameObject GetTreeAsset()
    {
        return trees[Random.Range(0,trees.Count)];
    }

    public float CheckDistanceFromPoles(Vector3 basePos, Vector3 topPos)
    {
        float minDist = 1000000;
        for(int i = 0; i < poleTops.Length; i++)
        {
            float measuredDist = Vector3.Distance(poleTops[i].transform.position, basePos);
            if (measuredDist < minDist)
                minDist = measuredDist;
            measuredDist = Vector3.Distance(poleTops[i].transform.position, topPos);
            if (measuredDist < minDist)
                minDist = measuredDist;
        }
        return minDist;
    }
}
