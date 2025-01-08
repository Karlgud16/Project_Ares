//Script that handels the Player Attack (Light, Heavy)
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;

    bool canLightAttack, canHeavyAttack;

    GroundCheck groundCheck;

    public float LightAttack, HeavyAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        groundCheck = gameObject.transform.GetChild(0).GetComponent<GroundCheck>();
        canLightAttack = true;
        canHeavyAttack = true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && groundCheck.grounded && canLightAttack)
        {
            StartCoroutine("PlayerLightAttack");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && groundCheck.grounded && canHeavyAttack)
        {
            StartCoroutine("PlayerHeavyAttack");
        }
    }

    IEnumerator PlayerLightAttack()
    {
        canLightAttack = false;
        int randomNum = Random.Range(1, 4);
        if(randomNum >= 2)
        {
            animator.SetTrigger("Attack1");
        }
        else
        {
            animator.SetTrigger("Attack2");
        }
        float lightDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(lightDuration - 0.7f);
        canLightAttack = true;
        randomNum = 0;
    }

    IEnumerator PlayerHeavyAttack()
    {
        canHeavyAttack = false;
        animator.SetTrigger("Attack3");
        float heavyDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(heavyDuration - 0.5f);
        canHeavyAttack = true;
    }
}
