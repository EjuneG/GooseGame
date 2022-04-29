using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControl : MonoBehaviour
{
    Rigidbody2D rgb;
    Collider2D collider;
    public const float defaultSpeed = 3.5f;
    [SerializeField]private float OneLevelSpeedChange = 0.1f;
    Vector2 direction;
    Vector2 normalSpeed;
    [SerializeField]private float currentSpeed;
    public float CurrentSpeed {
        get { return currentSpeed;}
        set { currentSpeed = value;}
    }

    private bool beingHeld;
    public bool BeingHeld {
        get { return beingHeld; }
        set { beingHeld = value; }
    }

    private float speedToLaunch;

    public int bounceCount;
    public float quickGooseIncrease = 0.25f;
    public float bigGooseDecrease = -0.25f;
    public float speedBottomThreshold = 3.5f;
    Animator eggAnimator;
    public Animator addAnimator;
    private float lastTouchTime = 0f;
    public float LastTouchedTime {
        get { return lastTouchTime; }
        set { lastTouchTime = value; }
    }

    private int pointStreakAdder = 0;
    public int PointStreakAdder {
        get { return pointStreakAdder; }
        set { pointStreakAdder = value; }
    }


    public string lastTouchedBy; //shows which goose last touched it
    public Vector2 lastVelocity;
    private int pointMultiplier;
    public float edgeChangeVariance = 0.4f;

    // Start is called before the first frame update
    void Awake()
    {
        rgb = GetComponent<Rigidbody2D>();
        rgb.AddForce(new Vector2(0, -2));
        eggAnimator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    private void Start() {
        GameDisplay.Instance.eggList.Add(this.gameObject);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Egg Speed:" + rgb.velocity.magnitude);
        normalSpeed = rgb.velocity.normalized;
        //Debug.Log("normalX: " + normalSpeed.x);
        //Debug.Log("normalY: " + normalSpeed.y);
        lastTouchTime += Time.fixedDeltaTime;


        updateSpeed();
        updatePointMultiplier();
        //Debug.Log("Point Multi: " + pointMultiplier);
        updateSpeedVFX();

        // update rotation based on current velocity, egg points towards its current direction
        updateRotation();
        Debug.Log("CurrentSpeed:" + currentSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bounceCount += 1;
        if (!collision.gameObject.tag.Equals("Obstacle")) {
            lastVelocity = rgb.velocity; // save velocity before collision
        }

        //check where the collision comes from, react differently for each goose
        if (collision.gameObject.name.Equals("QuickGoose") && lastTouchTime > 0.1f) {
            currentSpeed = rgb.velocity.magnitude + 0.1f;
            rgb.velocity = rgb.velocity.normalized * currentSpeed;
            //PlayerControl.Instance.quickGooseAnimator.Play("head");
            lastTouchedBy = "QuickGoose";
            AudioManager.Instance.Play("quickHead");
        } else if (collision.gameObject.name.Equals("BigGoose") && lastTouchTime > 0.1f) {
            currentSpeed = rgb.velocity.magnitude;
            if (currentSpeed > speedBottomThreshold) {
                currentSpeed = rgb.velocity.magnitude - 0.1f;
                rgb.velocity = rgb.velocity.normalized * currentSpeed;
            }
            lastTouchedBy = "BigGoose";
            AudioManager.Instance.Play("getWater2");
            // unused code, big goose no longer gets point increase
            //3/21 trying to decrease to 10, b/c other gooses gain points pretty easily now
            AudioManager.Instance.Play("bigHead");
            //AudioManager.Instance.Play("bigWin");
            GameDisplay.Instance.addPoint(10, "BigGoose");
            //    addAnimator.Play("appear");
            //    PlayerControl.Instance.bigGooseAnimator.Play("head");
        } else {
            currentSpeed = rgb.velocity.magnitude; //updating currentSpeed
        }

        if (collision.gameObject.name == "BigGoose" || collision.gameObject.name == "QuickGoose" || collision.gameObject.name == "MageGoose") {
            //touched by goose --> check if going down
            if (rgb.velocity.normalized.y < 0) {
                //if going down, we want it to go up ^ ^
                float oldX = rgb.velocity.normalized.x;
                float oldY = rgb.velocity.normalized.y;
                float newY;
                newY = Mathf.Abs(oldY);

                rgb.velocity = new Vector2(oldX, newY).normalized * currentSpeed;
            }
        }
    }


    // egg tilting mechanism when it goes straight
    // tilt to a random angle upon collision
    private void OnCollisionExit2D(Collision2D collision) {

        if (collision.gameObject.name == "BigGoose" || collision.gameObject.name == "QuickGoose" || collision.gameObject.name == "MageGoose") {
            resetStreak();
        } else {
            float edgeChange = Random.Range(-edgeChangeVariance, edgeChangeVariance);
            if (Mathf.Abs(rgb.velocity.normalized.x) > 0.95) {
                float oldX = rgb.velocity.normalized.x;
                float oldY = rgb.velocity.normalized.y;
                float newX;
                float newY;
                if (oldX < 0) {
                    newX = oldX + edgeChange;
                } else {
                    newX = oldX - edgeChange;
                }

                if (oldY < 0) {
                    newY = oldY - edgeChange;
                } else {
                    newY = oldY + edgeChange;
                }

                rgb.velocity = new Vector2(newX, newY).normalized * currentSpeed;
            } else if (Mathf.Abs(rgb.velocity.normalized.y) > 0.95) {
                float oldX = rgb.velocity.normalized.x;
                float oldY = rgb.velocity.normalized.y;
                float newX;
                float newY;
                if (oldX > 0) {
                    newX = oldX + edgeChange;
                } else {
                    newX = oldX - edgeChange;
                }

                if (oldY > 0) {
                    newY = oldY - edgeChange;
                } else {
                    newY = oldY + edgeChange;
                }
                rgb.velocity = new Vector2(newX, newY).normalized * currentSpeed;
            }
        }
        

        if(collision.gameObject.name == "BigGoose" || collision.gameObject.name == "QuickGoose" ||collision.gameObject.name == "MageGoose") {
            lastTouchTime = 0;
        }
        //Debug.Log("last"+lastVelocity + "new" + rgb.velocity);
    }

    public void eggStop() {
        //PlayerControl.Instance.mageGooseAnimator.Play("hold");
        speedToLaunch = rgb.velocity.magnitude; //save speed
        rgb.velocity = Vector2.zero;
    }

    public void eggLaunch() {
        AudioManager.Instance.Play("mageShoot");
        bounceCount = 0;
        Vector2 desiredDirection = new Vector2(0, 1);
        //// mage shoot at random angle between -45 to 45 degree
        //Vector2 desiredDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1);
        rgb.velocity = desiredDirection.normalized * speedToLaunch; //shoot out
        //lastVelocity = rgb.velocity;
        stopShoot();
    }
    void initializeEgg(float speed, float launchSpeed, Vector2 direction) {
        currentSpeed = speed;
        speedToLaunch = launchSpeed;
        this.direction = direction;
    }
    /// <summary>
    /// speed up this egg 1 level (0.25)
    /// </summary>
    public void speedUp() {
        currentSpeed += OneLevelSpeedChange;
    }

    public void speedDown() {
        if (currentSpeed >= speedBottomThreshold) {
            currentSpeed -= OneLevelSpeedChange;
        }
    }

    private void resetStreak() {
        pointStreakAdder = 0;
    }

    private void updatePointMultiplier() {
        if(currentSpeed < 4.49f) {
            pointMultiplier = 1;
        }else if(currentSpeed < 6.49f) {
            pointMultiplier = 2;
        }else{
            pointMultiplier = 4;
        }
    }

    private void updateSpeed() {
        rgb.velocity = rgb.velocity.normalized * currentSpeed;
        updateBottomSpeed(Progression.Instance.difficultyLevel);
        if (currentSpeed < speedBottomThreshold) {
            currentSpeed = speedBottomThreshold;
        }
    }
    private void updateSpeedVFX() {
        GameObject speedVFX = transform.GetChild(0).gameObject;
        if (pointMultiplier >= 2) {
            if (!speedVFX.activeSelf) {
                speedVFX.SetActive(true);
            }
        } else {
            if (speedVFX.activeSelf) {
                speedVFX.SetActive(false);
            }
        }
    }



    private void updateBottomSpeed(int difficulty) {
        switch (difficulty) {
            case 1:
                speedBottomThreshold = 3.9f;
                break;
            case 2:
                speedBottomThreshold = 4.5f;
                break;
            case 3:
                speedBottomThreshold = 5.3f;
                break;
            case 4:
                speedBottomThreshold = 6.5f;
                break;
            default:
                break;
        }
    }

    // update rotation based on current velocity, egg points towards its current direction
    private void updateRotation() {
        // get the angle from current velocity
        // adjust angle: rotate by 90 degrees so that 0 degree points upwards
        if (!beingHeld) {
            float angle = Mathf.Atan2(rgb.velocity.y, rgb.velocity.x) * Mathf.Rad2Deg - 90;
            //Debug.Log("angle" + angle);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void setMultiplier(int inputPointMultiplier)
    {
        pointMultiplier = inputPointMultiplier;
    }

    public int getMultiplier() {
        return pointMultiplier;
    }

    public void setVelocity(Vector2 velocity)
    {
        rgb.velocity = velocity;
    }


    IEnumerator stopShoot() {
        yield return new WaitForSeconds(1f);

        //PlayerControl.Instance.mageShoot = false;
    }

    // unused code, big goose no longer gets point increase
    //IEnumerator increasePoint(int point) {
    //    int pointToAdd = point;
    //    while (pointToAdd > 0) {
    //        GameDisplay.Instance.points += 1;
    //        pointToAdd--;
    //        if (pointToAdd == point / 2 && point > 5) {
    //            AudioManager.Instance.Play("bigWin");
    //        }
    //        yield return new WaitForSeconds(0.03f);
    //        if (pointToAdd < 0) {
    //            StopAllCoroutines();
    //        }
    //    }
    //}


}
