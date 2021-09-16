using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl Instance;
    [SerializeField] GameObject quickGoose;
    [SerializeField] GameObject mageGoose;
    [SerializeField] GameObject bigGoose;
    public float leftBorder;
    public float rightBorder;
    public float quickVelocity = 2.0f;
    public float bigVelocity = 0.5f;
    public float mageVelocity = 1.0f;
    Rigidbody2D quickGooseRB;
    Rigidbody2D mageGooseRB;
    Rigidbody2D bigGooseRB;
    [SerializeField]Transform mageEggPosition;
    public Animator quickGooseAnimator;
    public Animator mageGooseAnimator;
    public Animator bigGooseAnimator;
    public Animator foxAnimator;
    public Animator houseAnimator;
    public Animator rockAnimator;
    private bool controlOn = false;

    private Vector3 mousePosition;

    //animation
    public bool quickWalk;
    public bool quickHead;
    public bool mageWalk;
    public bool mageHold;
    public bool mageShoot;
    public bool bigWalk;
    public bool bigHead;


    void Awake()
    {
        Instance = this;
        quickGooseRB = quickGoose.GetComponent<Rigidbody2D>();
        mageGooseRB = mageGoose.GetComponent<Rigidbody2D>();
        bigGooseRB = bigGoose.GetComponent<Rigidbody2D>();
        quickGooseAnimator = quickGoose.GetComponent<Animator>();
        mageGooseAnimator = mageGoose.GetComponent<Animator>();
        bigGooseAnimator = bigGoose.GetComponent<Animator>();
        controlOn = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputControl();
        animationControl();
    }

    void inputControl() {
        quickGooseControl();
        bigGooseControl();
        mageGooseControl();
    }

    void animationControl() {
        quickGooseAnimator.SetBool("walk", quickWalk);
        quickGooseAnimator.SetBool("head", quickHead);
        mageGooseAnimator.SetBool("walk", mageWalk);
        mageGooseAnimator.SetBool("hold", mageHold);
        mageGooseAnimator.SetBool("shoot", mageShoot);
        bigGooseAnimator.SetBool("walk", bigWalk);
        bigGooseAnimator.SetBool("head", bigHead);
    }
    void quickGooseControl() {
    // TODO Add border later
        if (Input.GetKey(KeyCode.A) && notOnLeftEdge(quickGoose.transform)) {
            quickGoose.transform.Translate(Vector2.left * quickVelocity * Time.deltaTime, Space.Self);
            flipLeft(quickGoose.transform);
            quickWalk = true;
        } else if (Input.GetKey(KeyCode.D) && notOnRightEdge(quickGoose.transform)) {
            //quickGooseRB.MovePosition(quickGooseRB.position + quickVelocity * Time.fixedDeltaTime);
            quickGoose.transform.Translate(Vector2.right * quickVelocity * Time.deltaTime, Space.Self);
            flipRight(quickGoose.transform);
            quickWalk = true;
        } else {
            quickWalk = false;
        }
    }

    void bigGooseControl() {
        if (Input.GetKey(KeyCode.LeftArrow) && notOnLeftEdge(bigGoose.transform)) {
            bigGoose.transform.Translate(Vector2.left * bigVelocity * Time.deltaTime, Space.Self);
            flipLeft(bigGoose.transform);
            bigWalk = true;
        } else if (Input.GetKey(KeyCode.RightArrow) && notOnRightEdge(bigGoose.transform)) {
            //quickGooseRB.MovePosition(quickGooseRB.position + quickVelocity * Time.fixedDeltaTime);
            bigGoose.transform.Translate(Vector2.right * bigVelocity * Time.deltaTime, Space.Self);
            flipRight(bigGoose.transform);
            bigWalk = true;
        } else {
            bigWalk = false;
        }
    }

    void mageGooseControl() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.x < mageGoose.transform.position.x - 0.5 && notOnLeftEdge(mageGoose.transform)) {
            mageGoose.transform.Translate(Vector2.left * mageVelocity * Time.deltaTime, Space.Self);
            flipLeft(mageGoose.transform);
            mageWalk = true;
        } else if(mousePosition.x > mageGoose.transform.position.x + 0.5 && notOnRightEdge(mageGoose.transform)) {
            mageGoose.transform.Translate(Vector2.right * mageVelocity * Time.deltaTime, Space.Self);
            flipRight(mageGoose.transform);
            mageWalk = true;
        } else {
            mageWalk = false;
        }
        if (GameDisplay.Instance.holdingEgg && Input.GetMouseButton(0)){
            GameDisplay.Instance.theEgg.eggLaunch();
            GameDisplay.Instance.holdingEgg = false;
        }
    }
    bool touchingLeftBorder(Vector3 position) {
        if (position.x <= leftBorder) {
            return false;
        }
        return true;
    }

    bool touchingRightBorder(Vector3 position) {
        if(position.x >= rightBorder) {
            return false;
        }
        return true;
    }


    void flipRight(Transform transform) {
        if(transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }

    void flipLeft(Transform transform) {
        if (transform.localScale.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool notOnRightEdge(Transform transform) {
        return transform.position.x < 3.6f;
    }

    bool notOnLeftEdge(Transform transform) {
        return transform.position.x > -3.6f;
    }

    
}
