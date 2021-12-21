using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingPoint : MonoBehaviour
{
    Animator addingPointAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        addingPointAnimator.GetComponent<Animator>();
    }
}
