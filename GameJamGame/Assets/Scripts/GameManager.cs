using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    public static GameManager instance;

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
}
