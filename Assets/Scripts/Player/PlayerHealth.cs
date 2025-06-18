//Handles the player taking damage and picking up health

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    HealthSystem _healthSystem;

    BaseEnemyAttack _baseAttack;

    Animator _animator;

    private ItemManager _itemManager;

    private PlayerManager _playerManager;

    private EnemyManager _enemyManager;

    private MiniBossManager _miniBossManager;

    void Awake()
    {
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();
        _miniBossManager = GameManager.Instance.GetComponent<MiniBossManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_healthSystem.CanTakeDamage && _playerManager.PlayerIsDead == false)
        {
            //If the player is hit by enemy's attack
            if (other.gameObject.tag == "baseAttack")
            {
                _animator.SetTrigger("Hurt");

                switch (other.transform.parent.name)
                {
                    case string a when a.Contains("BaseEnemy"):
                        _healthSystem.PlayerCurrentHealth -= _enemyManager.BaseEnemyAttack;
                        break;
                    case string a when a.Contains("Brute"):
                        _healthSystem.PlayerCurrentHealth -= _enemyManager.BruteBaseAttack;
                        break;
                    case string a when a.Contains("Borrek"):
                        _healthSystem.PlayerCurrentHealth -= _miniBossManager.BorrekAttack;
                        break;
                }
            }

            //If the player is hit by the Mage Projectile
            else if (other.gameObject.tag == "Projectile")
            {
                _animator.SetTrigger("Hurt");
                _healthSystem.PlayerCurrentHealth -= _enemyManager.ProjectileAttack;
            }
        }

        //Adds the health multiplier to the health pickup
        if(other.name.Contains("HealthMultiplier") && _playerManager.PlayerIsDead == false)
        {
            _itemManager.HealthPickup += _itemManager.HealthPickupMultiplier;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Adds health to the player when the player is colliding with health pickup 
        if (other.name.Contains("HealthPickup") && _playerManager.PlayerIsDead == false && _healthSystem.PlayerCurrentHealth > 0 && _healthSystem.PlayerCurrentHealth < _playerManager.PlayerHealth)
        {
            _healthSystem.PlayerCurrentHealth += _itemManager.HealthPickup;
            if(_healthSystem.PlayerCurrentHealth > _playerManager.PlayerHealth)
            {
                _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth;
            }
            Destroy(other.gameObject);
        }
    }
}
