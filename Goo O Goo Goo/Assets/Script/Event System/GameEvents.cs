using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    
    void Awake()
    {
        current = this;
    }

    public event Action onX2TriggerEnter;
    public void X2TimeStart() {
        if (onX2TriggerEnter != null) {
            onX2TriggerEnter();
        }
    }

    public event Action onX2TriggerExit;
    public void X2TimeEnds() {
        if (onX2TriggerExit != null) {
            onX2TriggerExit();
        }
    }
}
