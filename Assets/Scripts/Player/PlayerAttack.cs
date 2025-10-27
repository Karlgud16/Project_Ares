//Script that handels the Player Attack (Light, Heavy)

using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [ReadOnly] public bool IsAttacking;

    private Animator _animator;
    private SpriteRenderer _sR;

    [SerializeField][ReadOnly] public bool CanLightAttack;
    [SerializeField][ReadOnly] public bool CanHeavyAttack;

    private bool _lightToggle;
    private bool _heavyToggle;

    private GameObject _lightAttackCollider, _heavyAttackCollider;

    private GroundCheck _groundCheck;

    private PlayerMovement _playerMove;

    private PlayerManager _playerManager;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _groundCheck = gameObject.transform.GetChild(0).GetComponent<GroundCheck>();
        _sR = GetComponent<SpriteRenderer>();
        _lightAttackCollider = GameObject.FindGameObjectWithTag("lightAttack");
        _playerMove = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

        CanLightAttack = true;
        CanHeavyAttack = true;
        _lightToggle = true;
        _heavyToggle = true;
    }

    public void LightAttack()
    {
        if(_groundCheck.grounded && CanLightAttack && _lightToggle && _playerManager.PlayerIsDead == false)
        {
            StartCoroutine("PlayerLightAttack");
        }
    }

    public void HeavyAttack()
    {
        if (_groundCheck.grounded && CanHeavyAttack && _heavyToggle && _playerManager.PlayerIsDead == false)
        {
            StartCoroutine("PlayerHeavyAttack");
        }
    }

    IEnumerator PlayerLightAttack()
    {
        IsAttacking = true;
        float elapsedTime = 0f;
        _lightToggle = false;
        CanLightAttack = false;
        _playerMove.StaminaReset = false;
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
        float staminaDrainPerSecond = _playerManager.DefaultPlayer.LightAttackDrain / lightDuration - 0.7f;
        while (elapsedTime < lightDuration - 0.7f)
        {
            elapsedTime += Time.deltaTime;
            _playerMove.Stamina -= staminaDrainPerSecond * Time.deltaTime;

            yield return null;
        }
        StartCoroutine(_playerMove.ResetStamina());
        CanLightAttack = true;
        _lightToggle = true;
        IsAttacking = false;
    }

    IEnumerator PlayerHeavyAttack()
    {
        IsAttacking = true;
        float elapsedTime = 0f;
        _playerMove.StaminaReset = false;
        _heavyToggle = false;
        CanHeavyAttack = false;
        _animator.SetTrigger("Attack3");
        float heavyDuration = _animator.GetCurrentAnimatorClipInfo(0).Length;
        float staminaDrainPerSecond = _playerManager.DefaultPlayer.HeavyAttackDrain / heavyDuration - 0.5f;
        while (elapsedTime < heavyDuration - 0.5f)
        {
            elapsedTime += Time.deltaTime;
            _playerMove.Stamina -= staminaDrainPerSecond * Time.deltaTime;

            yield return null;
        }
        StartCoroutine(_playerMove.ResetStamina());
        CanHeavyAttack = true;
        _heavyToggle = true;
        IsAttacking = false;
    }
}