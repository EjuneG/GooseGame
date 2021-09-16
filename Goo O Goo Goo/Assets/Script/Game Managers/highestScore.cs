using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highestScore : MonoBehaviour
{
    public static int theScore = 0;
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
