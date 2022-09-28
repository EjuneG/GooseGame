using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator explosionAnim;
    // Start is called before the first frame update
    void Start()
    {
        explosionAnim = GetComponent<Animator>();
    }

    public void PlayExplosion(Vector3 location, float size) {
        transform.position = location;
        transform.localScale = new Vector3(size, size, size);
        explosionAnim.Play("explode");
    }
}
