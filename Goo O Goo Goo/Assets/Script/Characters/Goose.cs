using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goose : MonoBehaviour
{
    protected Rigidbody2D gooseRB;
    [SerializeField] protected float speed;
    protected CommandKey keyUsing;
    void Awake()
    {
        gooseRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    protected void FixedUpdate() {
        GooseMovement();
        GooseAbility();
    }

    void GooseMovement() {
        if (keyUsing == CommandKey.left && notOnLeftEdge(this.transform)) {
            this.transform.Translate(Vector2.left * speed * Time.deltaTime, Space.Self);
            flipLeft(this.transform);
        } else if (keyUsing == CommandKey.right && notOnRightEdge(this.transform)) {
            //quickGooseRB.MovePosition(quickGooseRB.position + quickVelocity * Time.fixedDeltaTime);
            this.transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
            flipRight(this.transform);
        }
    }

    //assuming all might have an ability for future flexibility
    protected virtual void GooseAbility() { }


    //i put down constance to save time, better change them later
    protected bool notOnRightEdge(Transform transform) {
        return transform.position.x < 3.6f;
    }

    protected bool notOnLeftEdge(Transform transform) {
        return transform.position.x > -3.6f;
    }

    protected void flipLeft(Transform transform) {
        if (transform.localScale.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }

    protected void flipRight(Transform transform) {
        if (transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }


    public void setKey(CommandKey key) {
        keyUsing = key;
    }


}

/// <summary>
/// set goose to player 1 / player 2's control.
/// </summary>
/// /// <remarks>
/// Player 1 controls with WASD, Player 2 controls with arrows.
/// </remarks>

public enum CommandKey {
    up = 1,
    down = 2,
    left = 3,
    right = 4,
    none = 0
}


