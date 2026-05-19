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
    [SerializeField] private ParticleSystem fire;

    [SerializeField] private float fireRange;
    [SerializeField] private bool onFire;
    [SerializeField] private float catchOnTime;
    private float catchOnTimer;
    [SerializeField] private float deadAfterTime;
    private float deathTimer;

    private float growSize;
    private Transform tree;

    private void Start()
    {
        tree = Instantiate(gameObject.GetComponentInParent<TreeManager>().getTreeAsset(), transform).transform;
        trigger = tree.GetComponent<ClickTrigger>();
        fire.Stop();
        StartGrowing();
    }

    private void StartGrowing()
    {
        tree.gameObject.SetActive(true);
        growSize = Random.Range(growSizeEverySecond.x, growSizeEverySecond.y);
        tree.localScale = startSize;
        deathTimer = 0;
        catchOnTimer = 0;
        Extinguish();
    }

    private void Update()
    {
        if (GameManager.paused) { return; }
        if(!tree.gameObject.activeSelf)
        {
            return;
        }
        tree.localScale = tree.localScale + new Vector3(growSize * Time.deltaTime, growSize * Time.deltaTime, growSize * Time.deltaTime);
        if (trigger.isClicked)
        {
            switch (ModeManager.mode)
            {
                case Mode.None:
                    break;
                case Mode.Cut:
                    Cut();
                    tree.gameObject.SetActive(false);
                    break;
                case Mode.Water:
                    Extinguish();
                    break;
                default:
                    break;
            }
        }
        if (onFire)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer > deadAfterTime)
            {
                deathTimer = 0f;
                Cut();
                return;
            }
            catchOnTimer += Time.deltaTime;
            if (catchOnTimer > catchOnTime)
            {
                catchOnTimer = 0f;
                CatchOn();
            }
        }   
    }

    public void Cut()
    {
        Transform fallingTree = Instantiate(tree, tree.position, Quaternion.identity);
        fallingTree.gameObject.layer = 0;
        fallingTree.AddComponent<Rigidbody>().AddForceAtPosition(new Vector3(Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(0.5f, 1f) * Random.Range(knockForce.x, knockForce.y), Random.Range(-1f, 1f) * Random.Range(knockForce.x, knockForce.y)), fallingTree.position + Vector3.up * fallingTree.localScale.y, ForceMode.Impulse);
        Destroy(fallingTree.gameObject, 5f);
        tree.gameObject.SetActive(false);
        Extinguish();
        deathTimer = 0;
        catchOnTimer = 0;
        Invoke(nameof(StartGrowing), Random.Range(spawnDelay.x, spawnDelay.y));
    }

    public void Ignite()
    {
        if (onFire) return;

        onFire = true;

        if (!fire.isPlaying)
        {
            fire.Play();
        }
    }

    public void Extinguish()
    {
        onFire = false;

        deathTimer = 0;
        catchOnTimer = 0;

        if (fire.isPlaying)
        {
            fire.Stop();
        }
    }

    private void CatchOn()
    {
        Debug.Log("Catch On");
        Collider[] objects = Physics.OverlapSphere(transform.position, fireRange);
        foreach (Collider obj in objects)
        {
            if (obj.gameObject.GetComponent<TreeSystem>() != null)
            {
                //if (obj.gameObject.GetComponent<TreeSystem>() == this) { return; }
                obj.gameObject.GetComponent<TreeSystem>().Ignite();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}

