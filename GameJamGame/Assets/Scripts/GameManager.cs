using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    public static GameManager instance;

    private void Awake()
    {
        GameManager.instance = this;
    }

    private void Start()
    {
        Debug.Log(paused);
        //DialogManager.instance.StartDialogue(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
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
