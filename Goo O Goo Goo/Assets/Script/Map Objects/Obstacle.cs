using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int HP = 3;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D eggCollider) {
        if (eggCollider.gameObject.name.Equals("Egg")) {
            HP -= 1;
            animator.SetFloat("HP", this.HP);
            AudioManager.Instance.Play("breakHouse");
            if (HP <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
}
