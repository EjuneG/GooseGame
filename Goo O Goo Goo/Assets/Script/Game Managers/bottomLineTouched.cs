using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottomLineTouched : MonoBehaviour
{

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision) {
        // adjust egg count
        // name is "Egg" or "Egg(Clone)"
        if (collision.gameObject.tag.Equals("Egg")) {
            GameDisplay.Instance.eggCount -= 1;
        }
        // game over if no egg left
        if (GameDisplay.Instance.eggCount <= 0) {
            AudioManager.Instance.Play("breakFox");
            AudioManager.Instance.StopPlay("bgm");
            AudioManager.Instance.Play("lossBGM");
            GameDisplay.Instance.gameOver = true;
            Debug.Log("Game Over" + Time.time);
        }
    }
}
