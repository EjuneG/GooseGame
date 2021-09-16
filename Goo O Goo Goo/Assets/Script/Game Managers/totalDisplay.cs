using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totalDisplay : MonoBehaviour
{
    Text point;
    // Start is called before the first frame update
    private void Start() {
        point = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "" + highestScore.theScore;
    }
}
