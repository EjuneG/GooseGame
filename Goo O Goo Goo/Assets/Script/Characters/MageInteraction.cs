using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageInteraction : MonoBehaviour
{
    EggControl egg;
    public Transform eggPosition;
    float eggTime = 3f;
    private void FixedUpdate() {
        if (GameDisplay.Instance.holdingEgg) {
            egg.gameObject.transform.position = eggPosition.position;
        }
        if(GameDisplay.Instance.holdingEgg == true) {
            eggTime -= Time.fixedDeltaTime;
            if (eggTime <= 0) {
                GameDisplay.Instance.theEgg.eggLaunch();
                GameDisplay.Instance.holdingEgg = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameDisplay.Instance.theEgg != null) {
            egg = GameDisplay.Instance.theEgg;
            if (collision.gameObject.name.Equals("Egg") &&
                GameDisplay.Instance.theEgg.bounceCount != 0) {
                GameDisplay.Instance.holdingEgg = true;
                AudioManager.Instance.Play("mageGet");
                eggTime = 3f;
                egg.eggStop();
            }
        }
    }
}
