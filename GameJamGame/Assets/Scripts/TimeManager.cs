using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    [SerializeField] private TextMeshProUGUI timeDisplay;
    private float time;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        if(GameManager.paused)
        {
            return;
        }
        time += Time.deltaTime;
        int seconds = (int)time % 60;
        int minutes = (((int)time) / 60);
        string secString;
        string minString;
        if (seconds < 10)
        {
            secString = "0" + seconds;
        }
        else 
        {
            secString= seconds.ToString();
        }
        if(minutes < 10)
        {
            minString = "0" + minutes;
        }
        else
        {
            minString = minutes.ToString();
        }
        timeDisplay.text = minString + ":" + secString;
    }

    public string getTime()
    {
        return timeDisplay.text;
    }
}
