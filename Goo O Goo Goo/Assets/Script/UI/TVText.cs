using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVText : MonoBehaviour
{
    public Text x2Indicator;
    public Text x2Timer;
    public int x2Counter;
    float startTime;
    bool countRunning = false;
    //Queue<Coroutine> countBacks;
    private void Start() {
        GameEvents.current.onX2TriggerEnter += OnX2TimerStart;
        GameEvents.current.onX2TriggerExit += OnX2TimerEnds;
        x2Counter = 1;
    }

    private void Update() {
        if(startTime == 0) {
            x2Indicator.gameObject.SetActive(false);
            x2Timer.gameObject.SetActive(false);
        } else {
            x2Indicator.gameObject.SetActive(true);
            x2Timer.gameObject.SetActive(true);
        }
    }

    private void OnX2TimerStart() {
        x2Counter *= 2;
        x2Indicator.text = "x" + x2Counter;
        startTime = 10.0f;
        if (countRunning == false) {
            Coroutine countBack = StartCoroutine(CountBack());
        }
    }

    private void OnX2TimerEnds() {
        x2Counter /= 2;
        x2Indicator.text = "x" + x2Counter;
    }

    IEnumerator CountBack() {
        countRunning = true;
        while (startTime >= 0) {
            yield return new WaitForSeconds(0.1f);
            startTime -= 0.1f;
            x2Timer.text = startTime.ToString("0.0");
        }
        startTime = 0;
        countRunning = false;
    }
}
