using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pausePop : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject statViewUI;
    public GameObject menuViewUI;
    public GameObject Shade;
    public GameObject quitButton;
    public GameObject retryButton;
    public GameObject backButton;
    // Start is called before the first frame update
    // Update is called once per frame

    private void Start() {
        Button quitBt = quitButton.GetComponent<Button>();
        Button retryBt = retryButton.GetComponent<Button>();
        Button backBt = backButton.GetComponent<Button>();

        quitBt.onClick.AddListener(SoundOnClick);
        retryBt.onClick.AddListener(SoundOnClick);
        backBt.onClick.AddListener(SoundOnClick);


    }
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
        AudioManager.Instance.StopPlay("bgm");
        AudioManager.Instance.Play("bgm");
        SceneManager.LoadScene("GameScene");
    }

    void SoundOnClick() {
        AudioManager.Instance.Play("menuClick");
    }
}
