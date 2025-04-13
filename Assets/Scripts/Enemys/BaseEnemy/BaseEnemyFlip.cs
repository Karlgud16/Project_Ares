using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFlip : MonoBehaviour
{
    [ReadOnly] public bool canFlip;

    private SpriteRenderer sR;

    private GameObject player, attackCol, playerTrigger, spawnProjectile;

    void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (gameObject.tag == "BaseEnemy" || gameObject.tag == "Brute" || gameObject.tag == "Mage")
        {
            attackCol = transform.GetChild(1).gameObject;
            playerTrigger = transform.GetChild(2).gameObject;
        }
        if(gameObject.tag == "Mage")
        {
            spawnProjectile = transform.GetChild(4).gameObject;
        }
    }

    void Start()
    {
        canFlip = true;
    }

    void Update()
    {
        if (canFlip)
        {
            if(gameObject.tag == "BaseEnemy" || gameObject.tag == "Brute" || gameObject.tag == "Mage")
            {
                if (player.transform.position.x < transform.position.x)
                {
                    Vector3 newScale = attackCol.transform.localScale;
                    newScale.x = -1;
                    attackCol.transform.localScale = newScale;
                    Vector3 newScaleTrigger = playerTrigger.transform.localScale;
                    newScaleTrigger.x = -1;
                    playerTrigger.transform.localScale = newScaleTrigger;
                }
                else
                {
                    Vector3 newScaleCol = attackCol.transform.localScale;
                    newScaleCol.x = 1;
                    attackCol.transform.localScale = newScaleCol;
                    Vector3 newScaleTrigger = playerTrigger.transform.localScale;
                    newScaleTrigger.x = 1;
                    playerTrigger.transform.localScale = newScaleTrigger;
                }
            }

            if(gameObject.tag == "Mage")
            {
                if (player.transform.position.x < transform.position.x)
                {
                    Vector3 newPos = spawnProjectile.transform.localPosition;
                    newPos.x = -0.5f;
                    spawnProjectile.transform.localPosition = newPos;
                }
                else
                {
                    Vector3 newScale = spawnProjectile.transform.localPosition;
                    newScale.x = 0.5f;
                    spawnProjectile.transform.localPosition = newScale;
                }
            }

            if (player.transform.position.x < transform.position.x)
            {
                sR.flipX = true;
            }
            else
            {
                sR.flipX = false;
            }
        }
    }
}
