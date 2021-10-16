using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplay : MonoBehaviour
{
    public static GameDisplay Instance;
    public int eggCount;
    public List<GameObject> eggList = new List<GameObject>(); //a list to store eggs' gameObject for us to increase their speed
    public bool gameOver;
    [SerializeField] public int points = 0;

    private void Awake() {
        Instance = this;
        gameOver = false;
        eggCount = 1;
    }
}
