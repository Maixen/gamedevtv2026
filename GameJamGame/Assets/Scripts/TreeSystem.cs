using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeSystem : MonoBehaviour
{
    [SerializeField] private ClickTrigger trigger;
    [SerializeField] private Vector3 startSize;
    [SerializeField] private Vector2 spawnDelay;
    [SerializeField] private Vector2 growSizeEverySecond;
    [SerializeField] private Vector2 knockForce;

    private float growSize;
    private Transform tree;

    private void Start()
    {
        tree = Instantiate(gameObject.GetComponentInParent<TreeManager>().getTreeAsset(), transform).transform;
        trigger = tree.GetComponent<ClickTrigger>();
        StartGrowing();
    }

    private void StartGrowing()
    {
        tree.gameObject.SetActive(true);
        growSize = Random.Range(growSizeEverySecond.x, growSizeEverySecond.y);
        tree.localScale = startSize;
    }

    private void Update()
    {
        if(!tree.gameObject.activeSelf)
        {
            return;
        }
        tree.localScale = tree.localScale + new Vector3(growSize * Time.deltaTime, growSize * Time.deltaTime, growSize * Time.deltaTime);
        if (trigger.isClicked)
        {
            Cut();
            tree.gameObject.SetActive(false);
        }
            
    }

    public void Cut()
    {
        Transform fallingTree = Instantiate(tree, tree.position, Quaternion.identity);
        fallingTree.gameObject.layer = 0;
        fallingTree.AddComponent<Rigidbody>().AddForceAtPosition(new Vector3(Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(0.5f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y)), fallingTree.position + Vector3.up * fallingTree.localScale.y, ForceMode.Impulse);
        Destroy(fallingTree.gameObject, 5f);
        tree.gameObject.SetActive(false);
        Invoke(nameof(StartGrowing), Random.Range(spawnDelay.x, spawnDelay.y));
    }
}

