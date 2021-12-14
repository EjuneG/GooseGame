using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDisplay : MonoBehaviour
{
    public static GameDisplay Instance;
    public int eggCount;
    public List<GameObject> eggList = new List<GameObject>(); //a list to store eggs' gameObject for us to increase their speed
    public bool gameOver;
    [SerializeField] public int points = 0;

    //score addition UI
    public Text addScore;
    private Animator addScoreAnim;

    private void Awake() {
        Instance = this;
        gameOver = false;
        eggCount = 1;
    }

    //add point that plays the animation
    public void addPoint(string gooseName, int point) {
        //text color: mage goose - blue, big goose - magenta, quick goose - white
        switch (gooseName) {
            case "MageGoose":
                addScore.color = Color.blue;
                break;
            case "BigGoose":
                addScore.color = Color.magenta;
                break;
            case "QuickGoose":
                addScore.color = Color.white;
                break;
            default:
                Debug.Log("Egg Hit Bonus But No Goose Found");
                break;
        }

        this.points += point;

    }
}
