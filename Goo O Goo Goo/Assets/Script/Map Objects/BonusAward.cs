using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAward : MonoBehaviour
{
    //Animator animator;

    private float bonusX2Time = 10;
    [SerializeField] private float gameboardX;
    [SerializeField] private float gameboardY;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    //private void Update()
    //{
    //    if (timeRemaining > 0)
    //    {
    //        timeRemaining -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        Debug.Log("Time has run out!");
    //        timeRemaining = 0;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D eggCollider)
    {
        if (eggCollider.gameObject.name.Equals("Egg"))
        {
            Debug.Log(eggCollider.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            string bonusType = gameObject.name;

            // name is "...Award(Clone)"
            if (bonusType.Contains("EggAward"))
            {
                generateEggAward();
            }
            else if (bonusType.Contains("GoldAward"))
            {
                generateGoldAward();
            }
            else if (bonusType.Contains("X2Award"))
            {
                generateX2Award(eggCollider);
            }

            Destroy(this.gameObject);




        }
    }

    // eggaward -> extra egg
    private void generateEggAward()
    {
        Debug.Log("generating egg award");
    }

    // goldaward -> clear obstables, gold spread all over screen
    private void generateGoldAward()
    {
        // clear all obstacles

        // loop over gameboard x,y, spawn gold over ... distance


        Debug.Log("generating gold award");
    }

    // x2award -> multiplier x2 for 10 seconds
    private void generateX2Award(Collider2D eggCollider)
    {
        StartCoroutine(generateX2AwardWaiter(eggCollider));
    }

    IEnumerator generateX2AwardWaiter(Collider2D eggCollider)
    {
        Debug.Log("generating x2 award");
        int originalMultiplier = eggCollider.gameObject.GetComponent<EggControl>().getMultiplier();
        int doubledMuliplier = originalMultiplier * 2;

        Debug.Log("Started at timestamp : " + Time.time);
        Debug.Log("Before multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());

        //set multiplier to doubled
        eggCollider.gameObject.GetComponent<EggControl>().setMultiplier(doubledMuliplier);
        Debug.Log("Doubled multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());

        // wait on doubled point multiplier for 10 seconds, reset to original
        //float counter = 0;
        //float waitTime = 2;
        //while (counter < waitTime)
        //{
        //    //Increment Timer until counter >= waitTime
        //    counter += Time.deltaTime;
        //    Debug.Log("We have waited for: " + counter + " seconds");
        //    yield return null;
        //}
        yield return new WaitForSecondsRealtime(bonusX2Time);
        Debug.Log("Finished at timestamp : " + Time.time);
        eggCollider.gameObject.GetComponent<EggControl>().setMultiplier(originalMultiplier);
        Debug.Log("After multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());
    }
}
