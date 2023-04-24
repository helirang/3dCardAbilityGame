using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI timeTmp;
    [SerializeField]
    private float baseTime = 180f;
    float checkTime = 180f;
    float lookTime = 180f;
    float timeSpan = 0f;

    private bool isPause = false;
    private bool isEnd = false;
    public bool IsPause { set { isPause = value; } }

    public UnityEvent onTimeOVer;
    public UnityEvent reStart;

    public float LookTime
    {
        get { return lookTime; }
    }

    private void Start()
    {
        if(timeTmp == null)
        {
            timeTmp =this.GetComponent<TextMeshProUGUI>();
        }
        checkTime = baseTime;
        lookTime = baseTime;
        timeTmp.text = System.TimeSpan.FromSeconds(lookTime).ToString("mm\\:ss");
    }

    public void Initialized()
    {
        isPause = false;
        lookTime = checkTime;
        timeSpan = 0f;
        isEnd = false;
        timeTmp.text = System.TimeSpan.FromSeconds(lookTime).ToString("mm\\:ss");
    }

    private void Update()
    {
        if (isEnd) return;

        if(!isPause) timeSpan += Time.deltaTime;
        if(lookTime != Mathf.Round(checkTime - timeSpan))
        {
            lookTime = Mathf.Round(checkTime - timeSpan);
            timeTmp.text = System.TimeSpan.FromSeconds(lookTime).ToString("mm\\:ss");
        }
        if(timeSpan > checkTime)
        {
            isEnd = true;
            TimeOver();
        }
    }

    public void TimeOver()
    {
        isEnd = true;
        onTimeOVer.Invoke();
    }

    public void Restart()
    {
        reStart.Invoke();
    }

    public void Leave()
    {
        Application.Quit();
        //ConnectLobby.Mapint = 4;
        //PhotonNetwork.LeaveRoom();
        //SceneManager.LoadScene("LOADING");
    }
}
