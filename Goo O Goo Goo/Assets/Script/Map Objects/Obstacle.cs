using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int HP = 3;
    public int Score = 0;
    Animator animator;

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D eggCollider) {
        if (eggCollider.gameObject.tag.Equals("Egg")) {
            EggControl eggColliding = eggCollider.gameObject.GetComponent<EggControl>();
            string goose = eggCollider.gameObject.gameObject.GetComponent<EggControl>().lastTouchedBy;
            animator.Play("onHit");
            if(goose.Equals("BigGoose")) {
                //send message to UI display
                HP -= 2;
            }else if (goose.Equals("QuickGoose")) {
                HP -= 1;
                //send message to UI display
            } else {
                HP -= 1;
                //send message to UI -- Mage Goose
            }
            //animator.SetFloat("HP", this.HP); no animaton for now
            AudioManager.Instance.Play("breakHouse");
            if (HP <= 0) {
                int scoreToAdd = Score * eggColliding.getMultiplier();
                GameDisplay.Instance.addPoint(scoreToAdd, goose);

                // add bonus award object at the obstacle coordinates
                // random bonus award will be triggered by chance at a predefined probability
                Vector2 position = this.gameObject.transform.position;
                Progression.Instance.SpawnBonusAward(position);

                StartCoroutine(killObstacle());
            }
        }
    }

    public List<GameObject> GetChildren()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in this.gameObject.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    IEnumerator killObstacle() {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
