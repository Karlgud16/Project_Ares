using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHealth : MonoBehaviour
{
    PlayerAttack playerDam;

    [SerializeField] float enemyHealth;

    float currentHealth;

    void Start()
    {
        playerDam = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        currentHealth = enemyHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "lightAttack")
        {
            currentHealth -= playerDam.LightAttack;
        }
    }
}
