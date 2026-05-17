using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WireRepair : MonoBehaviour
{
    [SerializeField]
    private Vector2 firstBreakWaitTime;
    [SerializeField]
    private Vector2 breakTime;
    [SerializeField]
    private Vector2 knockForce;
    [SerializeField]
    private float repairTweenSpeed;
    private bool breakable;

    private bool broken = false;
    private ClickTrigger trigger;
    private Rigidbody physics;
    private float timeToWait = 0;
    private float waitedFor = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        trigger = GetComponent<ClickTrigger>();
        physics = GetComponent<Rigidbody>();
        //Rigidbody anschalten wenn kaputt, sonst net
    }

    private void Start()
    {
        timeToWait = Random.Range(firstBreakWaitTime.x,firstBreakWaitTime.y);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!breakable)
            return;
        waitedFor += Time.deltaTime;
        if (waitedFor < timeToWait)
            return;

        
    }

    private void Break()
    {

    }
}
