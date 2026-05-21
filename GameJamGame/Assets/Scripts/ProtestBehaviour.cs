using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProtestBehaviour : MonoBehaviour
{
    bool alive = true;
    [SerializeField] private Vector3 goal;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private ClickTrigger trigger;
    [SerializeField] private float deathTimer;


    private void Start()
    {
        transform.rotation.SetLookRotation((goal - transform.position).normalized, Vector3.up);
    }

    private void Update()
    {
        if (GameManager.paused)
            return;
        if (!alive)
            return;
        transform.position = Vector3.Lerp(transform.position,goal,walkSpeed * Time.deltaTime);
        if (!trigger.isClicked)
            return;
        if(ModeManager.mode != Mode.Gun)
            return;
        gameObject.layer = 0;
        alive = false;
        anim.SetBool("die",true);
        Destroy(gameObject,deathTimer);
    }

    public void SetGoal(Vector3 pos)
    {
        goal = pos;
    }
}
