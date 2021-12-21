using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//point update 
public class pointUpdate : MonoBehaviour
{
    Text point;

    private void Start() {
        point = GetComponent<Text>();
    }
    private void Update() {
        point.text = "" + GameDisplay.Instance.points;
    }
}
