using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialPanel : MonoBehaviour
{
    public void backToLobby() {
        SceneManager.LoadScene("OpeningScene");
    }
}
