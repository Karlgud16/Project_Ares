//Handles the logic for the mage projectile

using System.Collections;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    private float _speed;

    private Transform _currentPlayerTransform;

    Animator _animator;

    private bool _hitPlayer;

    private SpriteRenderer _sR;

    private Vector3 _lastPlayerPos;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _sR = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _speed = GameManager.Instance.ProjectileSpeed;
        _currentPlayerTransform = GameManager.Instance.Player.transform;
        _lastPlayerPos = GameManager.Instance.Player.transform.GetChild(3).position;
        _hitPlayer = false;


        if (_currentPlayerTransform.position.x < transform.position.x)
        {
            _sR.flipX = true;
        }
        else
        {
            _sR.flipX = false;
        }
    }


    void Update()
    {
        //If the projectile has not hit the Player
        if (!_hitPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _lastPlayerPos, _speed * Time.deltaTime);
        }

        //If the player has hit the player or the projectile is not moving anymore
        if(Vector3.Distance(transform.position, _lastPlayerPos) < 0.001f && !_hitPlayer)
        {
            StartCoroutine(DestroyProjectile());
        }
    }

    IEnumerator DestroyProjectile()
    {
        _animator.SetTrigger("End");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _hitPlayer = true;
            StartCoroutine(DestroyProjectile());
        }
    }
}
