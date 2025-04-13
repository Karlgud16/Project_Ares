//Script that handels the Player Movement (W,A,S,D, Jump)

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _playerInput, _moveVector;

    [ReadOnly][SerializeField] private Vector3 _dodgeDirection;

    private Rigidbody _rb;

    private GroundCheck _groundCheck;

    private SpriteRenderer _sR;

    private Animator _animator;

    private bool _facingRight = true; 

    [ReadOnly] public bool CanMove;
    [ReadOnly] public bool CanBlock;
    [ReadOnly] public bool DodgeToggle; //Toggle On/Off cooldown
    [ReadOnly] public bool CanInteract;
    [ReadOnly] public bool CanDodge;
    [ReadOnly] public bool CanJump;
    [ReadOnly] public bool DodgeCooldownStart; //Starts the cooldown inbetween dodges
    [ReadOnly][SerializeField] private float _playerSpeed; //Current player speed

    [ReadOnly][SerializeField] public float DodgeCooldownTimer; //For Accurate Dodge Cooldown Animation
    private Slider _dodgeCooldownSlider;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _sR = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _groundCheck = gameObject.transform.GetChild(0).GetComponent<GroundCheck>();
        _dodgeCooldownSlider = GameObject.FindGameObjectWithTag("Player1HUD").transform.GetChild(0).GetComponent<Slider>();
    }

    void Start()
    {
        CanMove = true;
        CanBlock = true;
        CanDodge = true;
        DodgeToggle = true;
        CanJump = true;
        DodgeCooldownStart = false;
        _dodgeDirection = Vector3.right;
        _dodgeCooldownSlider.value = 1f;
        DodgeCooldownTimer = GameManager.Instance.PlayerDodgeCooldown;
    }

    void Update()
    {
        _playerSpeed = _rb.velocity.magnitude;

        if (CanMove)
        {
            PlayerMove();
            PlayerFlip();
        }
        PlayerAnimation();
        DodgeCooldown();
    }

    public void PlayerDirection(Vector2 dir)
    {
        //Get Input from Player
        _playerInput.x = dir.x;
        _playerInput.z = dir.y;
    }

    void PlayerMove()
    {
        //Apply the playerInput to the Rigidbody
        _moveVector = transform.TransformDirection(_playerInput) * GameManager.Instance.PlayerMoveSpeed;
        _rb.velocity = new Vector3(_moveVector.x, _rb.velocity.y, _moveVector.z);

        //If the player if going Diagonally,
        //set the moveVector to the original speed and not double it
        if (_rb.velocity.magnitude > 1)
        {
            _moveVector.Normalize();
        }

        // Store the movement input direction if the player is moving
        if (_playerInput.x != 0 || _playerInput.z != 0)
        {
            _dodgeDirection = new Vector3(_playerInput.x, 0f, _playerInput.z).normalized;
        }
    }

    public void PlayerJump()
    {
        //If the player presses the jump key and the player is grounded,
        //add a force upwards and set animation trigger of Jump
        if (CanJump && _groundCheck.grounded)
        {
            _rb.AddForce(Vector3.up * GameManager.Instance.PlayerJump, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
        }
    }

    public void PlayerDodge()
    {
        if(CanDodge && DodgeToggle)
        {
            StartCoroutine(DodgeRoll());
        }
    }

    IEnumerator DodgeRoll()
    {
        _animator.SetTrigger("Roll");
        DodgeCooldownStart = true;
        CanDodge = false;
        CanMove = false;
        float startTime = Time.time;
        float endTime = startTime + GameManager.Instance.PlayerDodgeDuration;
        while (Time.time < endTime)
        {
            transform.Translate(_dodgeDirection * GameManager.Instance.PlayerDodgeSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        CanMove = true;
    }

    void DodgeCooldown()
    {
        _dodgeCooldownSlider.value = DodgeCooldownTimer;

        if (DodgeCooldownStart)
        {
            DodgeCooldownTimer -= Time.deltaTime;

            if (_dodgeCooldownSlider.value <= 0f)
            {
                CanDodge = true;
                DodgeCooldownTimer = GameManager.Instance.PlayerDodgeCooldown;
                DodgeCooldownStart = false;
            }
        }
    }

    public void PlayerBlock()
    {
        if (_groundCheck.grounded && CanBlock && _animator.GetBool("IdleBlock") == true)
        {
            _animator.SetTrigger("Block");
        }
    }

    void PlayerAnimation()
    {
        //Set AnimState 1 if the player is moving, else set to 0
        if(_moveVector.x != 0 || _moveVector.z != 0)
        {
            _animator.SetFloat("AnimState", 1);
            _animator.SetBool("IdleBlock", false);
            _animator.SetFloat("walkSpeed", _playerSpeed / GameManager.Instance.PlayerMoveSpeed); //Dividing the current player speed by the target player speed to slow down animation
        }
        else
        {
            _animator.SetFloat("AnimState", 0);
            _animator.SetBool("IdleBlock", true);
            _animator.SetFloat("walkSpeed", 0);
        }

        //Set grounded to true if the player is on the ground
        if (_groundCheck.grounded)
        {
            _animator.SetBool("Grounded", true);
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }

        //Set AirSpeedY to the players Y velocity to check if the player is fallling
        _animator.SetFloat("AirSpeedY", _rb.velocity.y);
    }

    void PlayerFlip()
    {
        //If the player is pressing the opposite key and the sprite is in the wrong direction,
        //Flip the sprite and set facingRight to the opposite of what it is
        if (_playerInput.x < 0 && _facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            _facingRight = !_facingRight;
        }
        else if (_playerInput.x > 0 && !_facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            _facingRight = !_facingRight;
        }
    }
}
