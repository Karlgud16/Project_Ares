using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyMovement : MonoBehaviour
{
    NavMeshAgent enemyNav;
    GameObject player;

    bool isMoving;

    Animator animator;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyNav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        enemyNav.SetDestination(player.transform.position);

        if (transform.hasChanged)
        {
            animator.SetBool("isMoving", true);
            transform.hasChanged = false;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
