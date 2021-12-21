using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAward : MonoBehaviour
{
    //Animator animator;

    private float bonusGoldTime = 10;
    private float bonusX2Time = 10;
    [SerializeField] private float gameboardX;
    [SerializeField] private float gameboardY;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D eggCollider)
    {
        if (eggCollider.gameObject.name.Equals("Egg"))
        {
            Debug.Log(eggCollider.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            string bonusType = gameObject.name;

            // name is "...Award(Clone)"
            if (bonusType.Contains("EggAward"))
            {
                GenerateEggAward(eggCollider);
            }
            else if (bonusType.Contains("GoldAward"))
            {
                GenerateGoldAward();
            }
            else if (bonusType.Contains("X2Award"))
            {
                GenerateX2Award(eggCollider);
            }

            Destroy(this.gameObject);
        }
    }

    // eggaward -> extra egg
    private void GenerateEggAward(Collider2D eggCollider)
    {
        Debug.Log("generating egg award");

        Vector2 position = eggCollider.transform.position;
        Progression.Instance.SpawnEggAward(position);

    }

    // goldaward -> clear obstables, gold spread all over screen
    private void GenerateGoldAward()
    {
        // clear all obstables
        Progression.Instance.ClearObstacles();

        // gold spread all over screen
        Progression.Instance.SpawnGoldAward();
        // gold will disappear after its life time in Coin.FixedUpdate()
    }


    // x2award -> multiplier x2 for 10 seconds
    private void GenerateX2Award(Collider2D eggCollider)
    {
        StartCoroutine(GenerateX2AwardWaiter(eggCollider));
    }

    // TODO: waitforseconds not working, try add a class similar to coin to count down
    private IEnumerator GenerateX2AwardWaiter(Collider2D eggCollider)
    {
        Debug.Log("generating x2 award");
        int originalMultiplier = eggCollider.gameObject.GetComponent<EggControl>().getMultiplier();
        int doubledMuliplier = originalMultiplier * 2;

        Debug.Log("Started at timestamp : " + Time.time);
        Debug.Log("Before multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());

        //set multiplier to doubled
        eggCollider.gameObject.GetComponent<EggControl>().setMultiplier(doubledMuliplier);
        Debug.Log("Doubled multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());


        //yield return new WaitForSeconds(bonusX2Time);
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Finished at timestamp : " + Time.time);
        eggCollider.gameObject.GetComponent<EggControl>().setMultiplier(originalMultiplier);
        Debug.Log("After multiplier: " + eggCollider.gameObject.GetComponent<EggControl>().getMultiplier());
    }
}
