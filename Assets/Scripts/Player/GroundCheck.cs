//Script that checks if the player is on the ground or not
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [ReadOnly] public bool grounded;
    [SerializeField] private float groundDistance;
    private Animator animator;

    void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);

        //Draw debug ray for editor
        if (grounded)
        {
            Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.red);
        }
    }
}
