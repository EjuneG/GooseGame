using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAward : MonoBehaviour
{
    Animator animator;
    bool isTrigger;

    private float bonusX2Time = 10;
    private float animationTime = 0.8f;
    [SerializeField] private float gameboardX;
    [SerializeField] private float gameboardY;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D eggCollider)
    {
        // avoid interaction with hidden bonus award
        //if (eggCollider.gameObject.tag.Equals("Egg") && !gameObject.transform.localScale.Equals(new Vector3(0, 0, 0)))
        if (eggCollider.gameObject.tag.Equals("Egg") && !isTrigger)
        {
            //Debug.Log(eggCollider.gameObject.name + " : " + gameObject.name + " : " + Time.time);

            animator.Play("onHit");
            AudioManager.Instance.Play("bonusCollect");
            isTrigger = true;

            // trigger bonus award by bonus type (saved as tag)
            string bonusType = gameObject.tag;
            switch (bonusType)
            {
                case "EggAward":
                    GenerateEggAward(eggCollider);
                    break;
                case "GoldAward":
                    GenerateGoldAward();
                    break;
                case "X2Award":
                    GenerateX2Award();
                    break;
                default:
                    Debug.Log("Unexpected bonus award triggered.");
                    break;
            }
        }
    }

    // eggaward -> extra egg
    private void GenerateEggAward(Collider2D eggCollider)
    {
        //Debug.Log("generating egg award");

        Vector2 position = eggCollider.transform.position;
        Progression.Instance.SpawnEggAward(position);
        StartCoroutine(killObstacle());
    }

    // goldaward -> clear obstables, gold spread all over screen
    private void GenerateGoldAward()
    {
        // clear all obstables
        Progression.Instance.ClearObstacles();

        // clear other gold to make screen look neat
        Progression.Instance.ClearGold();

        // gold spread all over screen
        Progression.Instance.SpawnGoldAward();  // gold will disappear after its life time in Coin.FixedUpdate()

        StartCoroutine(killObstacle());
    }


    // x2award -> multiplier x2 for 10 seconds
    public void GenerateX2Award()
    {
        StartCoroutine(GenerateX2AwardWaiter());
    }

    /*wait for bonus time
     1. should not destroy bonus object, otherwise the script would not work
     2. multiplier is accumulative.
        it should not be set back to original multiplier because x2 award may be called twice
        (x1 at time 0, x2 at time 3, x4 at time 10, x2 at time 13, x1 at time 20)
        it should be reset to half of the multiplier instead of original
     3. x2 multiplier is recorded separately in GameDisplay and has no effect on the individual egg's multiplier.
        x2 applies for the entire game, not specific eggs.
    */
    IEnumerator GenerateX2AwardWaiter()
    {
        //Debug.Log("generating x2 award");
        int doubledMultiplier = GameDisplay.Instance.x2Multiplier * 2;

        //Debug.Log("Started at timestamp : " + Time.time);
        //Debug.Log("Before multiplier: " + GameDisplay.Instance.x2Multiplier);

        //set multiplier to doubled
        GameDisplay.Instance.setX2Multiplier(doubledMultiplier);
        //Debug.Log("Doubled multiplier: " + GameDisplay.Instance.x2Multiplier);
        //trigger anim event
        GameEvents.current.X2TimeStart();
        yield return new WaitForSeconds(bonusX2Time);
        //Debug.Log("doubled: " + GameDisplay.Instance.x2Multiplier + "reset: " + GameDisplay.Instance.x2Multiplier / 2);
        int resetMultiplier = GameDisplay.Instance.x2Multiplier / 2;
        GameDisplay.Instance.setX2Multiplier(resetMultiplier);
        GameEvents.current.X2TimeEnds();
        //Debug.Log("Finished at timestamp : " + Time.time);
        //Debug.Log("After multiplier: " + GameDisplay.Instance.x2Multiplier);

        Destroy(this.gameObject);
    }

    IEnumerator killObstacle()
    {
        yield return new WaitForSeconds(animationTime);
        //// should not detroy current gameobject yet, script is needed for wait inumerator
        //Destroy(gameObject.GetComponent<Rigidbody>()); // disable rigidbody
        //gameObject.transform.localScale = new Vector3(0, 0, 0); // hide
        Destroy(this.gameObject);
    }
}
