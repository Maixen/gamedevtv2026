using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trees;

    public GameObject getTreeAsset()
    {
        return trees[Random.Range(0,trees.Count)];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
