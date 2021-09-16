using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControl : MonoBehaviour
{
    Rigidbody2D rgb;
    public float initialSpeed = 2f;
    public Transform magePosition;
    [SerializeField]private float currentSpeed;
    private float speedToLaunch;
    private Vector2 direction;
    private Vector2 normalSpeed;
    public int bounceCount;
    public float quickGooseIncrease = 0.25f;
    public float bigGooseDecrease = -0.25f;
    public float speedBottomThreshold = 3.5f;
    Animator eggAnimator;
    public Animator addAnimator;
    bool touched = false;
    float lastTouchTime = 0f;

    public float edgeChange = 0.2f; 
    // Start is called before the first frame update
    void Awake()
    {
        rgb = GetComponent<Rigidbody2D>();
        rgb.AddForce(new Vector2(0, -2));
        eggAnimator = GetComponent<Animator>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bounceCount += 1;
        //check where the collision comes from, react differently for each goose
        if (collision.gameObject.name.Equals("QuickGoose")) {
            currentSpeed = rgb.velocity.magnitude + 0.25f;
            rgb.velocity = rgb.velocity.normalized * currentSpeed;
            PlayerControl.Instance.quickGooseAnimator.Play("head");
            AudioManager.Instance.Play("quickHead");
        }else if (collision.gameObject.name.Equals("BigGoose")){
            currentSpeed = rgb.velocity.magnitude;
            if(currentSpeed > speedBottomThreshold) {
                currentSpeed = rgb.velocity.magnitude - 0.25f;
                rgb.velocity = rgb.velocity.normalized * currentSpeed;
            }
            AudioManager.Instance.Play("bigHead");
            AudioManager.Instance.Play("getWater2");
            StartCoroutine(increasePoint(20));
            addAnimator.Play("appear");
            PlayerControl.Instance.bigGooseAnimator.Play("head");
        } else {
            currentSpeed = rgb.velocity.magnitude; //updating currentSpeed
            eggAnimator.Play("pong");
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (PlayerControl.Instance.quickHead) {
            PlayerControl.Instance.quickHead = false;
        }
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
        PlayerControl.Instance.mageGooseAnimator.Play("hold");
        speedToLaunch = rgb.velocity.magnitude; //save speed
        //stop the egg for now and put it in the desired spot
        transform.position = magePosition.transform.position;
        rgb.velocity = Vector2.zero;

        
    }

    public void eggLaunch() {
        AudioManager.Instance.Play("mageShoot");
        bounceCount = 0;
        Vector2 desiredDirection = new Vector2(0, 1);
        PlayerControl.Instance.mageGooseAnimator.Play("shoot");
        rgb.velocity = desiredDirection.normalized * speedToLaunch; //shoot out
        stopShoot();
    }
    void initializeEgg(float speed, float launchSpeed, Vector2 direction) {
        currentSpeed = speed;
        speedToLaunch = launchSpeed;
        this.direction = direction;
    }

    IEnumerator stopShoot() {
        yield return new WaitForSeconds(1f);

        PlayerControl.Instance.mageShoot = false;
    }

    IEnumerator increasePoint(int point) {
        int pointToAdd = point;
        while (pointToAdd > 0) {
            GameDisplay.Instance.points += 1;
            pointToAdd--;
            if (pointToAdd == pointToAdd/2) {
                AudioManager.Instance.Play("bigWin");
            }
            yield return new WaitForSeconds(0.03f);
            if (pointToAdd < 0) {
                StopAllCoroutines();
            }
        }
    }
}
