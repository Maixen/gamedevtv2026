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
    [SerializeField]
    private bool breakable;
    [SerializeField] float maxHeight;

    [SerializeField] private bool broken = false;
    [SerializeField] private MeshRenderer helpfulSprite;
    private ClickTrigger trigger;
    private Rigidbody physics;
    private float timeToWait = 0;
    private float waitedFor = 0;
    private const int clickableLayer = 6;
    private Vector3 startPos;
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
        physics.Sleep();
        gameObject.layer = 0;
        startPos = transform.position;
        timeToWait = Random.Range(firstBreakWaitTime.x, firstBreakWaitTime.y);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!breakable)
            return;
        if (GameManager.paused == true) { return; } // Funktioniert wahrscheinlich nicht!!!
        if(!broken)
        {
            physics.Sleep();
            helpfulSprite.enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            if (Vector3.Distance(transform.position, startPos) > 1)
            {
                transform.position = Vector3.Lerp(transform.position, startPos, repairTweenSpeed * Time.deltaTime);
                return;
            }
            waitedFor += Time.deltaTime;
            if (waitedFor < timeToWait)
                return;
            waitedFor = 0;
            helpfulSprite.enabled = true;
            broken = true;
            transform.position = startPos;
            physics.WakeUp();
            Vector3 breakForce = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(knockForce.x,knockForce.y);
            physics.AddForce(breakForce,ForceMode.Impulse);
            transform.position += breakForce * Time.deltaTime;
            gameObject.layer = clickableLayer;
        }
        gameObject.layer = clickableLayer;
        helpfulSprite.enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        if (!trigger.isClicked)
        {
            return;
        }
        if(ModeManager.mode != Mode.Screw)
        {
            return;
        }
        physics.Sleep();
        transform.position = MouseCaster.collisionPos + Vector3.up * 3;
        if(transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
        print(Vector3.Distance(transform.position, startPos));
        if(Vector3.Distance(transform.position,startPos) > 5)
        {
            return;
        }
        print("Done");
        broken = false;
        physics.Sleep();
        gameObject.layer = 0;
        timeToWait = Random.Range(breakTime.x,breakTime.y);

    }

    private void Break()
    {

    }
}
