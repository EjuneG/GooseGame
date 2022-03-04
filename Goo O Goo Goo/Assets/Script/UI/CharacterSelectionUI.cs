using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionUI : MonoBehaviour
{
    int player1ArrowPlace; //1 = row 1 first goose, 3 = row 1 last goose
    int player2ArrowPlace;
    bool player1Selected;
    bool player2Selected;
    string player1Goose;
    string player2Goose;
    [SerializeField] private Transform[] ArrowPosition;
    [SerializeField] private Transform player1Arrow;
    [SerializeField] private Transform player2Arrow;
    private Dictionary<int, string> gooseNames = new Dictionary<int, string>();
    [SerializeField] private GameObject startIndication;
    [SerializeField] private GameObject title;

    //animators
    [SerializeField]Animator quickAnim;
    [SerializeField]Animator mageAnim;
    [SerializeField]Animator bigAnim;
    Animator player1Anim;
    Animator player2Anim;


    private void Awake() {
        gooseNames.Add(1, "QuickGoose");
        gooseNames.Add(2, "MageGoose");
        gooseNames.Add(3, "BigGoose");

        player1Selected = false;
        player2Selected = false;
        player1ArrowPlace = 1;
        player2ArrowPlace = 3;
    }

    private void Update() {
        updateSelectionArrow();
        updateSelectedGoose();
        startGame();
    }

    private void updateSelectionArrow() {
        if (player1Selected == false) {
            if (Input.GetKeyDown(KeyCode.A) && player1ArrowPlace != 1) {
                AudioManager.Instance.Play("menuClick");
                if (player1ArrowPlace - 1 != player2ArrowPlace) {
                    player1ArrowPlace -= 1;
                } else if (player1ArrowPlace == 3 && player2ArrowPlace == 2) {
                    player1ArrowPlace = 1;
                }
                Debug.Log("Player 1 Arrow Place: " + player1ArrowPlace);
                moveArrowTo(player1Arrow, player1ArrowPlace);
                //player 1 active
            } else if (Input.GetKeyDown(KeyCode.D) && player1ArrowPlace != 3) {
                AudioManager.Instance.Play("menuClick");
                if (player1ArrowPlace + 1 != player2ArrowPlace) {
                    player1ArrowPlace += 1;
                } else if (player1ArrowPlace == 1 && player2ArrowPlace == 2) {
                    player1ArrowPlace = 3;
                }
                Debug.Log("Player 1 Arrow Place: " + player1ArrowPlace);
                //player 1 active
                moveArrowTo(player1Arrow, player1ArrowPlace);
            }
        }

        //player 2
        if (player2Selected == false) {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && player2ArrowPlace != 1) {
                AudioManager.Instance.Play("menuClick");
                if (player2ArrowPlace - 1 != player1ArrowPlace) {
                    player2ArrowPlace -= 1;
                } else if (player2ArrowPlace == 3 && player1ArrowPlace == 2) {
                    player2ArrowPlace = 1;
                }
                //player 2 active
                Debug.Log("Player 2 Arrow Place: " + player2ArrowPlace);
                moveArrowTo(player2Arrow, player2ArrowPlace);
            } else if (Input.GetKeyDown(KeyCode.RightArrow) && player2ArrowPlace != 3) {
                AudioManager.Instance.Play("menuClick");
                if (player2ArrowPlace + 1 != player1ArrowPlace) {
                    player2ArrowPlace += 1;
                } else if (player2ArrowPlace == 1 && player1ArrowPlace == 2) {
                    player2ArrowPlace = 3;
                }
                //player 2 active
                Debug.Log("Player 2 Arrow Place: " + player2ArrowPlace);
                moveArrowTo(player2Arrow, player2ArrowPlace);
            }
        }
    }

    private void updateSelectedGoose() {
        //player 1
        if (Input.GetKeyDown(KeyCode.W)){
            player1Anim = getGooseAnimator(player1ArrowPlace);
            if (player1Selected == false) { //update the current goose as selected goose, set selected to true
                player1Selected = true;
                player1Goose = gooseNames[player1ArrowPlace];
                player1Anim.Play("head");
                playerGooseHeadSound(player1Goose);
                //play animation
            } else if (player1Selected == true) {//unselect the current goose
                player1Selected = false;
                player1Goose = null;
                player1Anim.Play("idle");
            }
        }else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            player2Anim = getGooseAnimator(player2ArrowPlace);
            if (player2Selected == false) { //update the current goose as selected goose, set selected to true
                player2Selected = true;
                player2Goose = gooseNames[player2ArrowPlace];
                player2Anim.Play("head");
                playerGooseHeadSound(player2Goose);
                //play animation
            } else if (player2Selected == true) {//unselect the current goose
                player2Selected = false;
                player2Goose = null;
                player2Anim.Play("idle");
            }
        }
    }

    private void moveArrowTo(Transform arrow, int position) {
        arrow.position = ArrowPosition[position-1].position;

        //need to restart activating goose
    }

    //returns if the player has selected 
    private bool checkSelected(PlayerID id) {
        if(id == PlayerID.player1) {
            return player1Selected;
        } else {
            return player2Selected;
        }
    }

    //return goose name that each player selects, Quick Goose, Mage Goose, Big Goose
    public string getPlayer1Goose() {
        if(player1Goose != null){
            return player1Goose;
        } else {
            Debug.Log("Player1 Character is not set, check selection code");
            return "Quick Goose";
        }
    }

    public string getPlayer2Goose() {
        if(player2Goose != null) {
            return player2Goose;
        } else {
            Debug.Log("Player2 Character is not set, check selection code");
            return "Quick Goose";
        }
    }

    public void startGame() {
        if (player1Selected && player2Selected) {
            if (title.activeSelf) {
                title.SetActive(false);
                startIndication.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                CharacterSelection.Instance.saveGooseNames(player1Goose, player2Goose);
                AudioManager.Instance.StopPlay("startBGM");
                AudioManager.Instance.Play("bgm");
                SceneManager.LoadScene("GameScene");
            }
        } else {
            if (startIndication.activeSelf) {
                startIndication.SetActive(false);
                title.SetActive(true);
            }
        }
    }

    private void setGooseToPlayer(PlayerID id, string gooseName) {
        if(id == PlayerID.player1) {
            player1Goose = gooseName;
        } else if(id == PlayerID.player2) {
            player2Goose = gooseName;
        }
    }

    private Animator getGooseAnimator(int arrowPosition) {
        switch (arrowPosition) {
            case 1:
                return quickAnim;
            case 2:
                return mageAnim;
            case 3:
                return bigAnim;
            default:
                Debug.Log("Goose Anim GG: plz check");
                return null;
        }
    }

    private void playerGooseHeadSound(string gooseName) {
        switch (gooseName) {
            case "QuickGoose":
                AudioManager.Instance.Play("quickHead");
                break;
            case "MageGoose":
                AudioManager.Instance.Play("mageShoot");
                break;
            case "BigGoose":
                AudioManager.Instance.Play("bigHead");
                break;
            default:
                Debug.Log("Play Head Sound No Name: Check");
                break;
        }
    }

    
}

enum PlayerID {
    player1 = 0,
    player2 = 1
};
