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

    // hard-coded constant values
    public const float goldDistance = 1f;
    public const float goldBottomBorderY = 0f;

    void Awake() {
        Instance = this;
        difficultyLevel = 0; //max = 5
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
        float y = goldBottomBorderY;

        while (y <= gameboardY) {
            Instantiate(coin, new Vector3(x, y), Quaternion.identity);
            y += goldDistance;
        }
    }
    void Spawn() {
        if(dropTime % 17 == 0) {
            SpawnCol();
        }else if(dropTime % 5 == 0) {
            Instantiate(coin, new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(goldBottomBorderY, gameboardY)), Quaternion.identity);
        }
        switch (difficultyLevel) {
            case 0:
                if(dropTime % 25 == 0) {
                    SpawnObject(obstacles[3]);
                }
                if(dropTime % 15 == 0) {
                    SpawnObject(obstacles[2]);
                }else if(dropTime % 10 == 0) {
                    SpawnObject(obstacles[1]);
                } else if(dropTime % 5 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            case 1:
                if (dropTime % 25 == 0) {
                    SpawnObject(obstacles[3]);
                }
                if (dropTime % 12 == 0) {
                    SpawnObject(obstacles[2]);
                } else if (dropTime % 8 == 0) {
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 4 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            case 2:
                if (dropTime % 15 == 0) {
                    SpawnObject(obstacles[3]);
                }
                if (dropTime % 8 == 0) {
                    SpawnObject(obstacles[2]);
                } else if (dropTime % 6 == 0) {
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 4 == 0) {
                    SpawnObject(obstacles[0]);
                    SpawnObject(obstacles[0]);
                }
                break;
            case 3:
                if (dropTime % 15 == 0) {
                    SpawnObject(obstacles[3]);
                }
                if (dropTime % 8 == 0) {
                    SpawnObject(obstacles[2]);
                } else if (dropTime % 6 == 0) {
                    SpawnObject(obstacles[1]);
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 4 == 0) {
                    SpawnObject(obstacles[0]);
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
        //Debug.Log("Spawn Object Called");
        //try to spawn this 10 times if finds conflict
        while (tryAttempts > 0) {
            //Debug.Log("While Loop Entered");
            Vector2 positionToSpawn = new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(goldBottomBorderY, gameboardY));
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
    // called from Obstacle class
    public void SpawnBonusAward(Vector2 position)
    {
        int bonusIndex = 1;
        //int bonusIndex = Random.Range(0, bonus.Length-1);
        Debug.Log("Spawn bonus award index: " + bonusIndex);
        Instantiate(bonus[bonusIndex], position, Quaternion.identity);
    }

    // spawn bonus award, gold spread all over screen
    public void SpawnGoldAward()
    {
        Debug.Log("progression: spawning gold");
        // loop over gameboard x,y, spawn gold over goldDistance
        float x = -gameboardX;
        float y = gameboardY;
        while (x <= gameboardX)
        {
            while (y >= goldBottomBorderY)
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
            //if (gameObjects[i].name.Contains("Rock") || gameObjects[i].name.Contains("Fox") || gameObjects[i].name.Contains("Truck"))
            {
                //Debug.Log(gameObjects[i] + "  : " + i + " (OBSTACLE)");
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
        if (time > 480) {
            difficultyLevel = 4;
        } else if (time > 240) {
            difficultyLevel = 3;
        } else if (time > 120) {
            difficultyLevel = 2;
        } else if (time > 60) {
            difficultyLevel = 1;
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