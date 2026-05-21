using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WireRepair : MonoBehaviour
{
    [SerializeField] private enum Stages
    {
        Intact,Broken,Dislocated
    }

    [SerializeField]
    private Vector2 firstBreakWaitTime;
    [SerializeField]
    private Vector2 breakTime;
    [SerializeField] private float dislocateTime;
    [SerializeField]
    private Vector2 knockForce;
    [SerializeField]
    private float repairTweenSpeed;
    [SerializeField]
    private bool breakable;
    [SerializeField] float maxHeight;

    [SerializeField] private Stages breakStage = Stages.Intact;
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
        helpfulSprite.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!breakable)
            return;
        if (GameManager.paused == true) { return; }
        switch(breakStage)
        {
            case Stages.Intact:
                physics.Sleep();
                
                if (Vector3.Distance(transform.position, startPos) > 1)
                {
                    transform.position = Vector3.Lerp(transform.position, startPos, repairTweenSpeed * Time.deltaTime);
                    return;
                }
                if (waitedFor < timeToWait)
                {
                    waitedFor += Time.deltaTime;
                    return;
                }
                breakStage = Stages.Broken;
                print("End of Intact");
                waitedFor = 0;
                helpfulSprite.enabled = true;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                gameObject.layer = clickableLayer;
                break;

            case Stages.Broken:
                
                if (waitedFor < dislocateTime)
                {
                    waitedFor += Time.deltaTime;
                    if(trigger.isClicked && ModeManager.mode == Mode.Screw)
                    {
                        breakStage = Stages.Intact;
                        waitedFor = 0;
                        helpfulSprite.enabled = false;
                        gameObject.GetComponent<SphereCollider>().enabled = false;
                        gameObject.layer = 0;
                        print("Fixed");
                    }
                    return;
                }
                print("End of Broken");
                waitedFor = 0;
                breakStage = Stages.Dislocated;
                ChaosManager.instance.AddProblem(ChaosType.Pole);
                Vector3 breakForce = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(knockForce.x, knockForce.y);
                physics.AddForce(breakForce, ForceMode.Impulse);
                transform.position += breakForce * Time.deltaTime;
                gameObject.layer = clickableLayer;
                break;

            case Stages.Dislocated:
                physics.WakeUp();
                if (!trigger.isClicked)
                {
                    return;
                }
                if (ModeManager.mode != Mode.Screw)
                {
                    return;
                }
                physics.Sleep();
                transform.position = MouseCaster.collisionPos + Vector3.up * 3;
                if (transform.position.y > maxHeight)
                {
                    transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
                }
                if (Vector3.Distance(transform.position, startPos) > 5)
                {
                    return;
                }
                print("Saved");
                breakStage = Stages.Intact;
                physics.Sleep();
                gameObject.layer = 0;
                helpfulSprite.enabled = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                timeToWait = Random.Range(breakTime.x, breakTime.y);
                ChaosManager.instance.FixProblem(ChaosType.Pole);
                break;

        }
    }
}
