using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProtestBehaviour : MonoBehaviour
{
    bool alive = true;
    bool atEnd = false;
    [SerializeField] private List<Vector3> goals;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private ClickTrigger trigger;
    [SerializeField] private float deathTimer;
    [SerializeField] private float minDist;
    int goalIndex = 0;

    private void Update()
    {
        if (GameManager.paused)
            return;
        if (!alive)
            return;
        if(goalIndex < goals.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, goals[goalIndex],Time.deltaTime * walkSpeed);
            transform.rotation = Quaternion.LookRotation(goals[goalIndex] - transform.position,Vector3.up);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            if(Vector3.Distance(transform.position, goals[goalIndex]) < minDist)
            {
                goalIndex++;
                if (goalIndex < goals.Count)
                {
                    atEnd = true;
                    GameManager.instance.NewProblem();
                }
            }
        }
        
        if (!trigger.isClicked)
            return;
        if(ModeManager.mode != Mode.Gun)
            return;
        if(atEnd)
            GameManager.instance.SolvedProblem();
        gameObject.layer = 0;
        alive = false;
        anim.SetBool("die",true);
        Destroy(gameObject,deathTimer);
    }

    public void SetGoal(List<Vector3> pos)
    {
        goals = pos;
    }
}
