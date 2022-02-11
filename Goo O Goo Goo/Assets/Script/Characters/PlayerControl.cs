using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl Instance;
    [SerializeField]Transform mageEggPosition;
    private ControlState currentControlState = ControlState.game; //set it to game for now
    private CharacterSelection characters;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentControlState == ControlState.characterSelection) {
            //TODO
        }else if(currentControlState == ControlState.game) {
            Player1Control();
            Player2Control();
        }
    }
    void gameInput() {
        Player1Control();
        Player2Control();
    }
    void Player1Control() {
        if (Input.GetKey(KeyCode.A)) {
            CharacterSelection.Instance.player1Goose.setKey(CommandKey.left);
        } else if (Input.GetKey(KeyCode.D)) {
            CharacterSelection.Instance.player1Goose.setKey(CommandKey.right);
        } else if (Input.GetKey(KeyCode.W)) {
            CharacterSelection.Instance.player1Goose.setKey(CommandKey.up);
        } else {
            CharacterSelection.Instance.player1Goose.setKey(CommandKey.none);
        }
    }

    void Player2Control() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            CharacterSelection.Instance.player2Goose.setKey(CommandKey.left);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            CharacterSelection.Instance.player2Goose.setKey(CommandKey.right);
        } else if (Input.GetKey(KeyCode.UpArrow)) {
            CharacterSelection.Instance.player2Goose.setKey(CommandKey.up);
        } else {
            CharacterSelection.Instance.player2Goose.setKey(CommandKey.none);
        }
    }


    //void mageGooseControl() {
    //    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    if(mousePosition.x < mageGoose.transform.position.x - 0.5 && notOnLeftEdge(mageGoose.transform)) {
    //        mageGoose.transform.Translate(Vector2.left * mageVelocity * Time.deltaTime, Space.Self);
    //        flipLeft(mageGoose.transform);
    //        mageWalk = true;
    //    } else if(mousePosition.x > mageGoose.transform.position.x + 0.5 && notOnRightEdge(mageGoose.transform)) {
    //        mageGoose.transform.Translate(Vector2.right * mageVelocity * Time.deltaTime, Space.Self);
    //        flipRight(mageGoose.transform);
    //        mageWalk = true;
    //    } else {
    //        mageWalk = false;
    //    }
    //    if (GameDisplay.Instance.holdingEgg && Input.GetMouseButton(0)){
    //        GameDisplay.Instance.theEgg.eggLaunch();
    //        GameDisplay.Instance.holdingEgg = false;
    //    }
    //}

    
}

enum ControlState{ 
    characterSelection,
    game
}
