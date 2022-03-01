using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Obstacle
{
    [SerializeField]private float speedReduction = 1f;
    private Animator bushAnim;
    protected override void Awake() {
        bushAnim = GetComponent<Animator>();
    }

    protected override void OnCollisionEnter2D(Collision2D eggCollider) {
        //we don't want collision so this is just to make sure it is not there anymore
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //When egg enters, we want to play a sound and gradually slow down the egg a bit if it will not reduce the egg's speed below minimum

        //play the sound first 
        AudioManager.Instance.Play("bush");
        //slow down the egg
        if(collision.gameObject.tag == "Egg") {
            EggControl egg = collision.gameObject.GetComponent<EggControl>();
            float initialSpeed = egg.CurrentSpeed;
            if(egg.CurrentSpeed - 0.5 >= egg.speedBottomThreshold) {
                StartCoroutine(slowDownEgg(egg));
            }
            bushAnim.Play("shake");
        }
            //play disappear anim, do nothing
            
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Egg") {
            Destroy(this.gameObject);
        }
    }

        IEnumerator slowDownEgg(EggControl egg) {
        float speedToReduce = speedReduction;
        float initialSpeed = egg.CurrentSpeed;
        while (speedToReduce > 0){
            Debug.Log("Doing work");
            egg.CurrentSpeed -= 0.2f;
            speedToReduce -= 0.2f;
            yield return new WaitForSeconds(0.05f);
            if (speedToReduce < 0) {
                float afterSpeed = egg.CurrentSpeed;
                Debug.Log("BeforeSpeed:" + initialSpeed + " AfterSpeed:" + afterSpeed);
            }
        }
    }
}
