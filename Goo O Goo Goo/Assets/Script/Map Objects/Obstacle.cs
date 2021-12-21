using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int HP = 3;
    public int Score = 0;
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D eggCollider) {
        if (eggCollider.gameObject.name.Equals("Egg")) {
            EggControl eggColliding = eggCollider.gameObject.GetComponent<EggControl>();
            string goose = eggCollider.gameObject.gameObject.GetComponent<EggControl>().lastTouchedBy;
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
            animator.SetFloat("HP", this.HP);
            AudioManager.Instance.Play("breakHouse");
            if (HP <= 0) {
                int scoreToAdd = Score * eggColliding.getMultiplier();
                GameDisplay.Instance.addPoint(scoreToAdd, goose);

                // add bonus award object in obstacle coordinates
                Vector2 position = this.gameObject.transform.position;
                Debug.Log("Destroyed obstacle position: " + position);
                Progression.Instance.SpawnBonusAward(position);


                Destroy(this.gameObject);
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
}
