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

    protected virtual void OnCollisionEnter2D(Collision2D eggCollider)
    {
        if (eggCollider.gameObject.tag.Equals("Egg"))
        {
            EggControl eggColliding = eggCollider.gameObject.GetComponent<EggControl>();
            string goose = eggCollider.gameObject.gameObject.GetComponent<EggControl>().lastTouchedBy;

            if (goose.Equals("BigGoose"))
            {
                //if last touched by big goose, obstacle is destroyed at once
                HP = 0;
                bigGooseDestroy(eggColliding, goose);
            }
            else if (goose.Equals("QuickGoose") || goose.Equals("MageGoose") )
            {
                HP -= 1;
                destroyOrBounce(eggColliding, goose);
                //send message to UI display
            }
            
        }
    }

    // check obstacle's HP -> destroy or bounce
    private void destroyOrBounce(EggControl eggColliding, string goose)
    {
        // destroy and pass through obstacle
        if (HP <= 0)
        {
            int scoreToAdd = Score * eggColliding.getMultiplier();
            GameDisplay.Instance.addPoint(scoreToAdd, goose);
            //TODO: update this animation
            animator.Play("onHit");
            AudioManager.Instance.Play("bigDestroy");

            // add bonus award object at the obstacle coordinates
            // random bonus award will be triggered by chance at a predefined probability
            Vector2 position = this.gameObject.transform.position;
            Progression.Instance.SpawnBonusAward(position);

            //StartCoroutine(killObstacle());
            Destroy(this.gameObject);
        }
        // hit and bounce
        else
        {
            animator.Play("onHit");
            AudioManager.Instance.Play("breakHouse");
        }
    }

    // check obstacle's HP -> destroy or bounce
    private void bigGooseDestroy(EggControl eggColliding, string goose)
    {
        // destroy and pass through obstacle
        int scoreToAdd = Score * eggColliding.getMultiplier();
        GameDisplay.Instance.addPoint(scoreToAdd, goose);
        
        animator.Play("onHit"); //TODO: update this animation
        AudioManager.Instance.Play("bigDestroy");

        // when destroyed, egg will continue follow the same direction and "pass" through the obstacle
        eggColliding.setVelocity(eggColliding.lastVelocity);
        //Debug.Log(eggColliding.lastVelocity + "bounce"+ eggColliding.GetComponent<Rigidbody2D>().velocity);

        // add bonus award object at the obstacle coordinates
        // random bonus award will be triggered by chance at a predefined probability
        Vector2 position = this.gameObject.transform.position;
        Progression.Instance.SpawnBonusAward(position);

        //StartCoroutine(killObstacle());
        Destroy(this.gameObject);
    }

    //used in BonusAward
    public List<GameObject> GetChildren()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in this.gameObject.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    //IEnumerator killObstacle() {
    //    yield return new WaitForSeconds(0.1f);
    //    Destroy(this.gameObject);
    //}
}
