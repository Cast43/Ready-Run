using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer_text;
    private int  min, sec, ms;
    // Start is called before the first frame update
    void Start()
    {
        timer_text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FormatTime();
    }

    private void FormatTime()
    {
        float time = Time.timeSinceLevelLoad;
        min = (int)(time / 60f) % 60;
        sec = (int)(time % 60f);
        ms = (int)(time * 100f) % 100;
        timer_text.text = min.ToString("D2") + ":" + sec.ToString("D2") + ":" + ms.ToString("D2");
    }


}
