using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//display total points on pause menu
public class totalPointDisplay : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + GameDisplay.Instance.points;
    }
}
