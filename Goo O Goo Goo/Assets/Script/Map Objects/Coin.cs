using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int value = 1;
    float lifeTime = 5f;
    public Animator animator;
    // Start is called before the first frame update

    // Update is called once per frame

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if(lifeTime < 0) {
            animator.Play("disappear");
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Egg")) {
            string touchedGoose = collision.GetComponent<EggControl>().lastTouchedBy; //!!Mage Goose needs set up!!
            EggControl eggColliding = collision.gameObject.GetComponent<EggControl>();
            int eggStreak = collision.GetComponent<EggControl>().PointStreakAdder;
            int pointToAdd = (value + eggStreak) * eggColliding.getMultiplier();
            collision.GetComponent<EggControl>().PointStreakAdder++; //add 1 to streak
            GameDisplay.Instance.addPoint(pointToAdd, touchedGoose);
            AudioManager.Instance.Play("getWater");
            Destroy(this.gameObject);
        }
    }
}
