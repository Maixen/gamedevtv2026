using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeSystem : MonoBehaviour
{
    [SerializeField] private Transform tree;
    [SerializeField] private Vector3 startSize;
    [SerializeField] private Vector2 spawnDelay;
    [SerializeField] private Vector2 growSizeEverySecond;
    [SerializeField] private Vector2 knockForce;
    private float growSize;

    private void Start()
    {
        Grow();
    }

    private void Grow()
    {
        growSize = Random.Range(growSizeEverySecond.x, growSizeEverySecond.y);
        tree.gameObject.SetActive(true);
        tree.localScale = startSize;
    }

    private void Update()
    {
        tree.localScale = tree.localScale + new Vector3(growSize * Time.deltaTime, growSize * Time.deltaTime, growSize * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape)) { Cut(); }
    }

    public void Cut()
    {
        Transform fallingTree = Instantiate(tree, tree.position, Quaternion.identity);
        fallingTree.AddComponent<Rigidbody>().AddForceAtPosition(new Vector3(Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(0.5f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y)), fallingTree.position + Vector3.up * fallingTree.localScale.y, ForceMode.Impulse);
        Destroy(fallingTree.gameObject, 5f);
        tree.gameObject.SetActive(false);
        Invoke(nameof(Grow), Random.Range(spawnDelay.x, spawnDelay.y));
    }
}

