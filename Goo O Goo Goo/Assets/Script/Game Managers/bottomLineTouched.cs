using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottomLineTouched : MonoBehaviour
{

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name.Equals("Egg")) {
            AudioManager.Instance.Play("breakFox");
            AudioManager.Instance.StopPlay("bgm");
            AudioManager.Instance.Play("lossBGM");
            GameDisplay.Instance.gameOver = true;
            Debug.Log("Game Over" + Time.time);
        }
    }
}
