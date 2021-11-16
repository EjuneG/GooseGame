using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float size = 1f;
    int value = 1;
    float lifeTime = 10f;
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
        if (collision.gameObject.name.Equals("Egg")) {
            string touchedGoose = collision.GetComponent<EggControl>().lastTouchedBy;
            EggControl eggColliding = collision.gameObject.GetComponent<EggControl>();
            GameDisplay.Instance.points += value * eggColliding.getMultiplier();
            switch (touchedGoose) {
                case "MageGoose":
                    //mage goose
                    break;
                case "BigGoose":
                    break;
                case "QuickGoose":
                    break;
                default:
                    Debug.Log("Egg Hit Bonus But No Goose Found");
                    break;
            }
            AudioManager.Instance.Play("getWater");
            Destroy(this.gameObject);
        }
    }
}
