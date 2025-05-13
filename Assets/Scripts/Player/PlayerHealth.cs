//Handles the player taking damage

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    HealthSystem _healthSystem;

    BaseEnemyAttack _baseAttack;

    Animator _animator;

    void Awake()
    {
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_healthSystem.CanTakeDamage && GameManager.Instance.PlayerIsDead == false)
        {
            //If the player is hit by enemy's attack
            if (other.gameObject.tag == "baseAttack")
            {
                _animator.SetTrigger("Hurt");
                switch (other.transform.parent.tag) 
                {
                    case "BaseEnemy":
                        _healthSystem.PlayerCurrentHealth -= GameManager.Instance.BaseEnemyAttack;
                        break;
                    case "Brute":
                        _healthSystem.PlayerCurrentHealth -= GameManager.Instance.BruteBaseAttack;
                        break;
                }
            }
            //If the player is hit by the Mage Projectile
            else if (other.gameObject.tag == "Projectile")
            {
                _animator.SetTrigger("Hurt");
                _healthSystem.PlayerCurrentHealth -= GameManager.Instance.ProjectileAttack;
            }
        }

        if(other.name.Contains("HealthMultiplier") && GameManager.Instance.PlayerIsDead == false)
        {
            GameManager.Instance.HealthPickup += GameManager.Instance.HealthPickupMultiplier;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("HealthPickup") && GameManager.Instance.PlayerIsDead == false)
        {
            if(_healthSystem.PlayerCurrentHealth > 0 && _healthSystem.PlayerCurrentHealth < GameManager.Instance.PlayerHealth)
            {
                _healthSystem.PlayerCurrentHealth += GameManager.Instance.HealthPickup;
                if(_healthSystem.PlayerCurrentHealth > GameManager.Instance.PlayerHealth)
                {
                    _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth;
                }
                Destroy(other.gameObject);
            }
        }
    }
}
