using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageGoose : Goose
{
    EggControl EggBeingHeld;
    bool holdingEgg;
    [SerializeField]private Transform holdingPosition;
    [SerializeField]private float holdingTime = 2f; //how long can hold the egg
    private float timeHeld = 0; //time MageGoose has held onto the egg

    override protected void Awake() {
        base.Awake();
        holdingEgg = false;
    }

    override protected void FixedUpdate() {
        base.FixedUpdate();
        if (holdingEgg) {
            Transform EggTransform = EggBeingHeld.gameObject.transform;
            EggTransform.position = holdingPosition.position;
            timeHeld -= Time.fixedDeltaTime;
            if (timeHeld <= 0) {
                EggBeingHeld.eggLaunch();
                holdingEgg = false;
            }
        }
    }

    protected override void GooseAbility() {
        if (keyUsing == CommandKey.up && holdingEgg) {
            EggBeingHeld.eggLaunch();
            holdingEgg = false;
        }
    }

    override protected void GooseAnimation() {
        base.GooseAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!holdingEgg && collision.gameObject.tag == "Egg") {
            EggBeingHeld = collision.gameObject.GetComponent<EggControl>();
            holdingEgg = true;
            AudioManager.Instance.Play("mageGet");
            timeHeld = holdingTime;
            EggBeingHeld.eggStop();
            EggBeingHeld.lastTouchedBy = "MageGoose";
        }
    }
}
