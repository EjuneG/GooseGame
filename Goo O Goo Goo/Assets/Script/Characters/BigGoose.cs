using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGoose : Goose
{

    //animator
    bool head;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Egg") {
            base.gooseAnim.Play("head");
        }
    }

    override protected void GooseAnimation() {
        base.GooseAnimation();
        gooseAnim.SetBool("head", head);
    }
}
