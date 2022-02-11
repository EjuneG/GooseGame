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
    public string p1GooseName;
    public string p2GooseName;
    [SerializeField] private Goose[] gooses = new Goose[3];

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    public void saveGooseNames(string player1GooseName, string player2GooseName) {
        p1GooseName = player1GooseName;
        p2GooseName = player2GooseName;
    }

    public void setPlayerGooses(Goose p1Goose, Goose p2Goose) {
        player1Goose = p1Goose;
        player2Goose = p2Goose;
    }
}
