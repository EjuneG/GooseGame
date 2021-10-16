using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// managers the character selection menu and assigns each player the goose they choose
/// </summary>
public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance;
    public Goose player1Goose;
    public Goose player2Goose;

    void Awake() {
        Instance = this;
    }
    public Goose[] getGooseBelonging() {
        Goose[] playerGooses = new Goose[2];
        //brute for testing
        playerGooses[0] = player1Goose;
        playerGooses[1] = player2Goose;
        return playerGooses;
    }
}
