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

    //animator
    bool hold;
    bool shoot;

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
                base.gooseAnim.Play("shoot");
                StartCoroutine(animationMove(false));
                holdingEgg = false;
                EggBeingHeld.BeingHeld = false;
            }
        }
    }

    protected override void GooseAbility() {
        if (keyUsing == CommandKey.up && holdingEgg) {
            EggBeingHeld.eggLaunch();
            base.gooseAnim.Play("shoot");
            StartCoroutine(animationMove(false));
            holdingEgg = false;
            EggBeingHeld.BeingHeld = false;
        }
    }

    override protected void GooseAnimation() {
        base.GooseAnimation();
        gooseAnim.SetBool("hold", hold);
        gooseAnim.SetBool("shoot", shoot);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!holdingEgg && collision.gameObject.tag == "Egg") {
            EggBeingHeld = collision.gameObject.GetComponent<EggControl>();
            holdingEgg = true;
            EggBeingHeld.BeingHeld = true;
            EggBeingHeld.transform.localRotation = Quaternion.Euler(0, 0, 0);
            AudioManager.Instance.Play("mageGet");
            base.gooseAnim.Play("hold");
            StartCoroutine(animationMove(true));
            timeHeld = holdingTime;
            EggBeingHeld.eggStop();
            EggBeingHeld.lastTouchedBy = "MageGoose";
            collision.GetComponent<EggControl>().PointStreakAdder = 0; //reset streak
        }
    }

    IEnumerator animationMove(bool isUp) {
        float distanceToMove = 0.3f;
        float moveByOne = 0.015f;
        while (distanceToMove >= 0) {
            if (isUp) {
                this.transform.Translate(Vector2.up * moveByOne, Space.Self);
            } else {
                this.transform.Translate(Vector2.down * moveByOne, Space.Self);
            }
            distanceToMove -= moveByOne;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
