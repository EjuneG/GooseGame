using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startPanel : MonoBehaviour
{
    // Update is called once per frame
    private void Start() {
        if (!AudioManager.Instance.isPlaying("startBGM")) {
            AudioManager.Instance.Play("startBGM");
        }
    }

    public void startGame() {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void startTutorial() {
        SceneManager.LoadScene("TutorialScene");
    }

    public void quitGame() {
        Application.Quit();
    }
}
