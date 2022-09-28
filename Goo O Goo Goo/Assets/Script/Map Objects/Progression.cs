using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    public static Progression Instance;
    public GameObject coin;
    public GameObject egg;
    public GameObject[] obstacles;
    public GameObject[] bonus;
    public int difficultyLevel;
    private float timePassed = 0;
    private int dropTime = 1;
    [SerializeField]private float gameboardX;
    [SerializeField]private float gameboardY;

    private GameObject rock;
    private GameObject house;
    private GameObject truck;
    private GameObject bush;
    // hard-coded constant values
    public const float goldDistance = 0.7f;
    public const float nonObstacleBottomBorderY = 0f;
    public const float obstacleBottomBorderY = 1f;
    public const float bonusProbability = 0.2f;

    void Awake() {
        Instance = this;
        difficultyLevel = 0; //max = 5

        if(obstacles[0] != null) {
            rock = obstacles[0];
            house = obstacles[1];
            truck = obstacles[2];
            bush = obstacles[3];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;
        updateDifficulty(timePassed);
    }

    void SpawnCol() {
        float x = Random.Range(-gameboardX, gameboardX);
        float y = nonObstacleBottomBorderY;

        while (y <= gameboardY) {
            Instantiate(coin, new Vector3(x, y), Quaternion.identity);
            y += goldDistance;
        }
    }
    void Spawn() {
        if(dropTime % 17 == 0) {
            SpawnCol();
        }else if(dropTime % 3 == 0) {
            Instantiate(coin, new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(nonObstacleBottomBorderY, gameboardY)), Quaternion.identity);
        }
        switch (difficultyLevel) {
            case 0:
                if(dropTime % 20 == 0) {
                    SpawnObject(bush);
                }
                if(dropTime % 10 == 0) {
                    SpawnObject(rock);
                }
                break;
            case 1:
                if (dropTime % 20 == 0) {
                    SpawnObject(bush);
                }
                if (dropTime % 14 == 0) {
                    SpawnObject(house);
                } else if (dropTime % 7 == 0) {
                    SpawnObject(rock);
                }
                break;
            case 2:
                if (dropTime % 15 == 0) {
                    SpawnObject(bush);
                }
                if (dropTime % 21 == 0) {
                    SpawnObject(truck);
                } else if (dropTime % 14 == 0) {
                    SpawnObject(house);
                } else if (dropTime % 7 == 0) {
                    SpawnObject(rock);
                }
                break;
            case 3:
                if (dropTime % 15 == 0) {
                    SpawnObject(bush);
                }
                if (dropTime % 14 == 0) {
                    SpawnObject(truck);
                } else if (dropTime % 7 == 0) {
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 5 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            case 4:
                if (dropTime % 15 == 0) {
                    SpawnObject(bush);
                }
                if (dropTime % 14 == 0) {
                    SpawnObject(truck);
                } else if (dropTime % 7 == 0) {
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 3 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            default:
                Debug.Log("Progression Difficulty not working, contact Chris");
                break;
        }
        dropTime++; //every second this increases by 1, and we decide the tempo depending on time.
    }

    //a first attempt to avoid objects stacking, needs improvement
    void SpawnObject(GameObject objectToSpawn) {
        Vector3 rendererSize = objectToSpawn.GetComponent<SpriteRenderer>().bounds.extents;
        int tryAttempts = 10;
        Vector2 positionToSpawn;
        //try to spawn this 10 times if finds conflict
        while (tryAttempts > 0) {
            if (objectToSpawn.Equals(bush))
            {
                positionToSpawn = new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(nonObstacleBottomBorderY, gameboardY));
            }
            else
            {
                positionToSpawn = new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(obstacleBottomBorderY, gameboardY));
            }
            Collider2D hitCollider = Physics2D.OverlapBox(positionToSpawn, rendererSize, 0);
            if (!hitCollider) {
                Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity);
                break;
            } else {
            }
            tryAttempts--;
        }



    }

    // spawn random bonus award at the position where obstacle is destroyed 
    // triggered by chance at a predefined probability
    // called from Obstacle class
    public void SpawnBonusAward(Vector2 position)
    {
        // the probability bonus award is only triggered
        // if bonusRandomChance falls into [0, 0.1) range
        float bonusRandomChance = Random.Range(0f, 1f);
        if (bonusRandomChance < bonusProbability)
        {
            int bonusIndex = Random.Range(0, bonus.Length);
            Instantiate(bonus[bonusIndex], position, Quaternion.identity);
            AudioManager.Instance.Play("bonusSpawn");
        }
    }

    // spawn bonus award, gold spread all over screen
    public void SpawnGoldAward()
    {
        // loop over gameboard x,y, spawn gold over goldDistance
        float x = -gameboardX;
        float y = gameboardY;
        while (x <= gameboardX)
        {
            while (y >= nonObstacleBottomBorderY)
            {
                Instantiate(coin, new Vector2(x, y), Quaternion.identity);
                y -= goldDistance;
            }
            x += goldDistance;
            y = gameboardY;
        }
    }

    // find and clear all obstacles
    public void ClearObstacles()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        for (var i = 0; i < gameObjects.Length; i++)
        {
            // find and destroy all objects with tag "Obstacle"
            if (gameObjects[i].tag.Equals("Obstacle"))
            {
                Destroy(gameObjects[i]);
            }
        }
    }

    // find and clear all Gold
    public void ClearGold()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        for (var i = 0; i < gameObjects.Length; i++)
        {
            // find and destroy all objects with tag "Gold"
            if (gameObjects[i].tag.Equals("Gold"))
            {
                Destroy(gameObjects[i]);
            }
        }
    }

    // spawn extra egg
    public void SpawnEggAward(Vector2 position)
    {
        Instantiate(egg, position, Quaternion.identity);
        GameDisplay.Instance.eggList.Add(this.gameObject);
        GameDisplay.Instance.eggCount += 1;
    }

    private bool CheckBounds2D(Vector2 position, Vector2 boundsSize) {
        Bounds boxBounds = new Bounds(position, boundsSize);

        float sqrHalfBoxSize = boxBounds.extents.sqrMagnitude;
        float overlapingCircleRadius = Mathf.Sqrt(sqrHalfBoxSize + sqrHalfBoxSize);

        /* Hoping I have the previous calculation right, move on to finding the nearby colliders */
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, overlapingCircleRadius);
        foreach (Collider2D otherCollider in hitColliders) {
            //now we ask each of thoose gentle colliders if they sens something is within their bounds
            if (otherCollider.bounds.Intersects(boxBounds))
                return (false);
        }
        return (true);
    }

    private void updateDifficulty(float time) {
        if (time > 227) {
            difficultyLevel = 4;
            if (AudioManager.Instance.isPlaying("bgm77.5%")) {
                AudioManager.Instance.StopPlay("bgm77.5%");
                AudioManager.Instance.Play("bgm65%");
            }
        } else if (time > 140) {
            difficultyLevel = 3;
            if (AudioManager.Instance.isPlaying("bgm87.5%")) {
                AudioManager.Instance.StopPlay("bgm87.5%");
                AudioManager.Instance.Play("bgm77.5%");
            }
        } else if (time > 74) {
            difficultyLevel = 2;
            if (AudioManager.Instance.isPlaying("bgm95%")) {
                AudioManager.Instance.StopPlay("bgm95%");
                AudioManager.Instance.Play("bgm87.5%");
            }
        } else if (time > 38) {
            difficultyLevel = 1;
            if (AudioManager.Instance.isPlaying("bgm")) {
                AudioManager.Instance.StopPlay("bgm");
                AudioManager.Instance.Play("bgm95%");
            }
        }
    }
}

enum DestroyableObjects{
    rock = 0,
    car = 1,
    house = 2
}

enum BonusAwards
{
    EggAward = 0,
    GoldAward = 1,
    X2Award = 2
}