using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    public static GameManager instance;
    [SerializeField] private int problemCount;

    private void Awake()
    {
        GameManager.instance = this;
    }

    public void GamePauseRequest()
    {
        ModeManager.instance.ChangeMode(0);
        paused = true;
    }

    public void GameUnpauseRequest(bool force)
    {
        if (force)
        {
            paused = false;
        }
        else
        {
            // GGs
        }
    }

    public void NewProblem()
    {
        problemCount++;
        ModeManager.instance.ShortCircuit();
    }

    public void SolvedProblem()
    {
        if(problemCount > 0)
            problemCount--;
    }

    public bool IsSafe()
    {
        return problemCount <= 0;
    }
}
