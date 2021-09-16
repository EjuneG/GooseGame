using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplay : MonoBehaviour
{
    public static GameDisplay Instance;
    public EggControl theEgg;
    public bool holdingEgg;
    public bool gameOver;
    [SerializeField] public int points = 0;

    private void Awake() {
        Instance = this;
        gameOver = false;
        holdingEgg = false;
    }
}
