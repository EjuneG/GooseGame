using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausePop : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject statViewUI;
    public GameObject menuViewUI;
    public GameObject Shade;
    public GameObject quitButton;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused && !GameDisplay.Instance.gameOver) {
                Resume();
            } else {
                Pause();
            }
        }

        if (GameDisplay.Instance.gameOver) {
            Pause();
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        statViewUI.SetActive(true);
        menuViewUI.SetActive(true);
        Shade.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        if(GameDisplay.Instance.points > highestScore.theScore) {
            highestScore.theScore = GameDisplay.Instance.points;
        }
        pauseMenuUI.SetActive(true);
        statViewUI.SetActive(false);
        menuViewUI.SetActive(false);
        Shade.SetActive(true);
        if (GameDisplay.Instance.gameOver) {
            quitButton.SetActive(false);
        } else {
            quitButton.SetActive(true);
        }
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() {
        AudioManager.Instance.StopPlay("lossBGM");
        AudioManager.Instance.StopPlay("bgm");
        GameDisplay.Instance.gameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("OpeningScene");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Restart() {
        GameDisplay.Instance.gameOver = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioManager.Instance.StopPlay("lossBGM");
        AudioManager.Instance.Play("bgm");
        SceneManager.LoadScene("GameScene");
    }
}
