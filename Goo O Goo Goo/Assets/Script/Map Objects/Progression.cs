using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    public GameObject coin;
    public GameObject[] obstacles;

    [SerializeField]private float gameboardX;
    [SerializeField]private float gameboardY;

    int dropTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 5.0f, 5.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void SpawnCol() {
        float x = Random.Range(-gameboardX, gameboardX);
        float y = 0;

        while (y <= gameboardY) {
            Instantiate(coin, new Vector3(x, y), Quaternion.identity);
            y += 0.5f;
        }
    }
    void Spawn() {
        if (dropTime % 4 != 0) {
            Instantiate(coin, new Vector3(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY)), Quaternion.identity);
        } else {
            SpawnCol();
        }
        dropTime += 1;

        if(dropTime % 12 == 0) {
            Instantiate(obstacles[2], new Vector3(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY)), Quaternion.identity);
        }else if(dropTime % 6 == 0) {
            Instantiate(obstacles[1], new Vector3(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY)), Quaternion.identity);
        }else if(dropTime % 2 == 0) {
            Instantiate(obstacles[0], new Vector3(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY)), Quaternion.identity);
        }
    }
}
