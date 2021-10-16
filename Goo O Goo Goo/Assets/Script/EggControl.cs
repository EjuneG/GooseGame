using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControl : MonoBehaviour
{
    Rigidbody2D rgb;
    public float defaultSpeed = 2f;
    [SerializeField]private float OneLevelSpeedChange = 0.5f;
    Vector2 direction;
    Vector2 normalSpeed;
    [SerializeField]private float currentSpeed;
    private float speedToLaunch;
    public int bounceCount;
    public float quickGooseIncrease = 0.25f;
    public float bigGooseDecrease = -0.25f;
    public float speedBottomThreshold = 3.5f;
    Animator eggAnimator;
    public Animator addAnimator;
    bool touched = false;
    float lastTouchTime = 0f;
    public string lastTouchedBy; //shows which goose last touched it

    public float edgeChange = 0.2f; 
    // Start is called before the first frame update
    void Awake()
    {
        rgb = GetComponent<Rigidbody2D>();
        rgb.AddForce(new Vector2(0, -2));
        eggAnimator = GetComponent<Animator>();
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
        lastTouchTime -= Time.fixedDeltaTime;

        //refresh speed if too low
        if(currentSpeed < speedBottomThreshold) {
            currentSpeed = speedBottomThreshold;
            rgb.velocity = rgb.velocity.normalized * currentSpeed;
        }
        updateBottomSpeed(Progression.Instance.difficultyLevel);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bounceCount += 1;
        //check where the collision comes from, react differently for each goose
        if (collision.gameObject.name.Equals("QuickGoose")) {
            currentSpeed = rgb.velocity.magnitude + 0.25f;
            rgb.velocity = rgb.velocity.normalized * currentSpeed;
            //PlayerControl.Instance.quickGooseAnimator.Play("head");
            lastTouchedBy = "QuickGoose";
            AudioManager.Instance.Play("quickHead");
        }else if (collision.gameObject.name.Equals("BigGoose")){
            currentSpeed = rgb.velocity.magnitude;
            if(currentSpeed > speedBottomThreshold) {
                currentSpeed = rgb.velocity.magnitude - 0.25f;
                rgb.velocity = rgb.velocity.normalized * currentSpeed;
            }
            lastTouchedBy = "BigGoose";
            AudioManager.Instance.Play("bigHead");
            AudioManager.Instance.Play("getWater2");
            StartCoroutine(increasePoint(20));
            addAnimator.Play("appear");
        //    PlayerControl.Instance.bigGooseAnimator.Play("head");
        } else {
            currentSpeed = rgb.velocity.magnitude; //updating currentSpeed
            eggAnimator.Play("pong");
        }
    }


    //egg tilting mechanism when it goes straight
    private void OnCollisionExit2D(Collision2D collision) {
        //if (PlayerControl.Instance.quickHead) {
        //    PlayerControl.Instance.quickHead = false;
        //}
        if(Mathf.Abs(rgb.velocity.normalized.x) > 0.95) {
            float oldX = rgb.velocity.normalized.x;
            float oldY = rgb.velocity.normalized.y;
            float newX;
            float newY;
            if(oldX < 0) {
                newX = oldX + edgeChange;
            } else {
                newX = oldX - edgeChange;
            }

            if(oldY < 0) {
                newY = oldY -edgeChange;
            } else {
                newY = oldY + edgeChange;
            }

            rgb.velocity = new Vector2(newX, newY).normalized * currentSpeed;
        }else if(Mathf.Abs(rgb.velocity.normalized.y) > 0.95) {
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

    public void eggStop() {
        //PlayerControl.Instance.mageGooseAnimator.Play("hold");
        speedToLaunch = rgb.velocity.magnitude; //save speed
        rgb.velocity = Vector2.zero;
    }

    public void eggLaunch() {
        AudioManager.Instance.Play("mageShoot");
        bounceCount = 0;
        Vector2 desiredDirection = new Vector2(0, 1);
        //PlayerControl.Instance.mageGooseAnimator.Play("shoot");
        rgb.velocity = desiredDirection.normalized * speedToLaunch; //shoot out
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

    public void updateBottomSpeed(int difficulty) {
        switch (difficulty) {
            case 1:
                Debug.Log("updated to new speed");
                speedBottomThreshold = 3.5f * 1.25f;
                break;
            case 2:
                speedBottomThreshold = 3.5f * 1.5f;
                break;
            case 3:
                speedBottomThreshold = 3.5f * 2.0f;
                break;
            case 4:
                speedBottomThreshold = 3.5f * 2.5f;
                break;
            default:
                break;
        }
    }

    IEnumerator stopShoot() {
        yield return new WaitForSeconds(1f);

        //PlayerControl.Instance.mageShoot = false;
    }
    IEnumerator increasePoint(int point) {
        int pointToAdd = point;
        while (pointToAdd > 0) {
            GameDisplay.Instance.points += 1;
            pointToAdd--;
            if (pointToAdd == point / 2 && point > 5) {
                AudioManager.Instance.Play("bigWin");
            }
            yield return new WaitForSeconds(0.03f);
            if (pointToAdd < 0) {
                StopAllCoroutines();
            }
        }
    }
}
