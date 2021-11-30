using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    public static Progression Instance;
    public GameObject coin;
    public GameObject[] obstacles;
    public GameObject[] bonus;
    public int difficultyLevel;
    private float timePassed = 0;
    [SerializeField]private float gameboardX;
    [SerializeField]private float gameboardY;

    private int dropTime = 1;

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
        float y = 0;

        while (y <= gameboardY) {
            Instantiate(coin, new Vector3(x, y), Quaternion.identity);
            y += 0.5f;
        }
    }
    void Spawn() {
        if(dropTime % 17 == 0) {
            SpawnCol();
        }else if(dropTime % 5 == 0) {
            Instantiate(coin, new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY)), Quaternion.identity);
        }
        switch (difficultyLevel) {
            case 0:
                if(dropTime % 15 == 0) {
                    SpawnObject(obstacles[2]);
                }else if(dropTime % 10 == 0) {
                    SpawnObject(obstacles[1]);
                } else if(dropTime % 5 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            case 1:
                if (dropTime % 12 == 0) {
                    SpawnObject(obstacles[2]);
                } else if (dropTime % 8 == 0) {
                    SpawnObject(obstacles[1]);
                } else if (dropTime % 4 == 0) {
                    SpawnObject(obstacles[0]);
                }
                break;
            case 2:
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
        Bounds colliderBound = objectToSpawn.GetComponent<PolygonCollider2D>().bounds;
        float sqrHalfBoxSize = colliderBound.extents.sqrMagnitude;
        float overlapingCircleRadius = Mathf.Sqrt(sqrHalfBoxSize + sqrHalfBoxSize);
        bool spawnThis = false;
        int tryAttempts = 10;
        //Debug.Log("Spawn Object Called");
        //try to spawn this 10 times if finds conflict
        while (tryAttempts > 0 && !spawnThis) {
            spawnThis = true; //set to true, if hit, then set back to false
            //Debug.Log("While Loop Entered");
            Vector2 positionToSpawn = new Vector2(Random.Range(-gameboardX, gameboardX), Random.Range(0, gameboardY));
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(positionToSpawn, overlapingCircleRadius);
            foreach(Collider2D otherCollider in hitColliders){
                spawnThis = false;
                //Debug.Log("Intersection Stopped");
                break;
            }
            if (spawnThis) {
                Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity);
            }
            tryAttempts--;
        }



    }

    // spawn random bonus award at the position where obstacle is destroyed
    // called from Obstacle class
    public void SpawnBonusAward(Vector2 position)
    {
        int bonusIndex = Random.Range(0, bonus.Length-1);
        //int bonusIndex = 2;
        Debug.Log("Spawn bonus award index: " + bonusIndex);
        Instantiate(bonus[bonusIndex], position, Quaternion.identity);
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
