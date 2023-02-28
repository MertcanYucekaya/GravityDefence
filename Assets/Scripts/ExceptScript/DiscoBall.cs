using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigi;
    public void discoMethod()
    {
        rigi.isKinematic = false;
        animator.enabled = false;
    }
}
