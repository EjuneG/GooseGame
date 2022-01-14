using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    int player1ArrowPlace = 1; //1 = row 1 first goose, 3 = row 1 last goose, 6 = row 2 last goose
    int player2ArrowPlace = 3;
    bool player1Selected = false;
    bool player2Selected = false;

    private void Awake() {
        
    }

    private void Update() {
        
    }

    private void updateSelectionArrow() {

    }

    //returns if the player has selected 
    private bool checkSelected(PlayerID id) {
        if(id == PlayerID.player1) {
            return player1Selected;
        } else {
            return player2Selected;
        }
    }

    
}

enum PlayerID {
    player1 = 0,
    player2 = 1
};
