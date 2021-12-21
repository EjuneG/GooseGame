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
    public Text addScoreText;
    public Text ScoreText;

    //animator
    protected Animator addScoreAnim;

    private void Awake() {
        Instance = this;
        gameOver = false;
        eggCount = 1;
        addScoreAnim = ScoreText.GetComponent<Animator>();
    }

    private void Update() {
        ScoreText.text = "" + points;
    }

    //add point asks who scored the point and the point amount, adds the point to total point and plays an animation
    public void addPoint(int point, string gooseName) {
        Color addScoreTextColor = Color.white;
        //text color: mage goose - blue, big goose - magenta, quick goose - white
        switch (gooseName) {
            case "MageGoose":
                addScoreTextColor = Color.blue;
                break;
            case "BigGoose":
                addScoreTextColor = Color.magenta;
                break;
            case "QuickGoose":
                addScoreTextColor = Color.white;
                break;
            default:
                Debug.Log("Egg Hit Bonus But No Goose Found");
                break;
        }
        Text newAddScoreText = Instantiate(addScoreText);

        newAddScoreText.transform.SetParent(GameObject.Find("StatView").transform);
        newAddScoreText.color = addScoreTextColor;
        newAddScoreText.text = "+" + point;

        this.points += point;

    }
    IEnumerator playAddAnimation(int pointToAdd, Color textColor) {
        Debug.Log("This worked");
        Text newAddScoreText = Instantiate(addScoreText);
        newAddScoreText.text = "" + pointToAdd;
        newAddScoreText.color = textColor;
        Animator addAnimator = newAddScoreText.GetComponent<Animator>();
        addAnimator.Play("ScoreAdd");
        yield return new WaitForSeconds(0.5f);
        //Destroy(newAddScoreText.gameObject);
    }
}
