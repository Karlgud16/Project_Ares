//Script that handels the Player Attack (Light, Heavy)

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sR;

    [SerializeField][ReadOnly] public bool CanLightAttack;
    [SerializeField][ReadOnly] public bool CanHeavyAttack;

    private bool _lightToggle;
    private bool _heavyToggle;

    private GameObject _lightAttackCollider, _heavyAttackCollider;

    private GroundCheck _groundCheck;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _groundCheck = gameObject.transform.GetChild(0).GetComponent<GroundCheck>();
        _sR = GetComponent<SpriteRenderer>();
        _lightAttackCollider = GameObject.FindGameObjectWithTag("lightAttack");
    }

    void Start()
    {
        CanLightAttack = true;
        CanHeavyAttack = true;
        _lightToggle = true;
        _heavyToggle = true;
    }

    public void LightAttack()
    {
        if(_groundCheck.grounded && CanLightAttack && _lightToggle && GameManager.Instance.PlayerIsDead == false)
        {
            StartCoroutine("PlayerLightAttack");
        }
    }

    public void HeavyAttack()
    {
        if (_groundCheck.grounded && CanHeavyAttack && _heavyToggle && GameManager.Instance.PlayerIsDead == false)
        {
            StartCoroutine("PlayerHeavyAttack");
        }
    }

    IEnumerator PlayerLightAttack()
    {
        _lightToggle = false;
        int randomNum = Random.Range(1, 4);
        if(randomNum >= 2)
        {
            _animator.SetTrigger("Attack1");
        }
        else
        {
            _animator.SetTrigger("Attack2");
        }
        float lightDuration = _animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(lightDuration - 0.7f);
        _lightToggle = true;
    }

    IEnumerator PlayerHeavyAttack()
    {
        _heavyToggle = false;
        _animator.SetTrigger("Attack3");
        float heavyDuration = _animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(heavyDuration - 0.5f);
        CanHeavyAttack = true;
        _heavyToggle = true;
    }
}