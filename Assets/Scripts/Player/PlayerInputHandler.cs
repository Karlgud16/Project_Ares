//Handles what input the player is doing depending on the action from the Input System

using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfig _playerConfig;
    private PlayerControls _controls;

    private PlayerMovement _playerMove;
    private PlayerAttack _playerAttack;

    void Start()
    {
        _playerMove = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();

        _controls = new PlayerControls();
    }

    public void InitializePlayer(PlayerConfig pi)
    {
        _playerConfig = pi;
        _playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == _controls.PlayerMovement.Movement.name)
        {
            OnMove(obj);
        }
        else if(obj.action.name == _controls.PlayerMovement.Jump.name)
        {
            OnJump(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.Dash.name)
        {
            OnDash(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.Block.name)
        {
            OnBlock(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.LightAttack.name)
        {
            OnLightAttack(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.HeavyAttack.name)
        {
            OnHeavyAttack(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.Interact.name)
        {
            OnInteract(obj);
        }
        else if (obj.action.name == _controls.PlayerMovement.Pause.name)
        {
            OnPause(obj);
        }
    }

    public void OnMove(CallbackContext context)
    {
        _playerMove.PlayerDirection(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        if (context.performed)
        {
            _playerMove.PlayerJump();
        }
    }

    public void OnDash(CallbackContext context)
    {
        if (context.performed)
        {
            _playerMove.PlayerDodge();
        }
    }

    public void OnBlock(CallbackContext context)
    {
        if (context.performed)
        {
            _playerMove.PlayerBlock();
        }
    }

    public void OnLightAttack(CallbackContext context)
    {
        if (context.performed)
        {
            _playerAttack.LightAttack();
        }
    }
    public void OnHeavyAttack(CallbackContext context)
    {
        if (context.performed)
        {
            _playerAttack.HeavyAttack();
        }
    }

    public void OnInteract(CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(Interact());
        }
    }

    public void OnPause(CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.IsPaused)
            {
                GameManager.Instance.MenuManager.Resume();
            }
            else if (GameManager.Instance.CanPause)
            {
                GameManager.Instance.PauseState = true;
            }
        }
    }

    IEnumerator Interact()
    {
        _playerMove.CanInteract = true;
        yield return new WaitForSeconds(0.1f);
        _playerMove.CanInteract = false;
    }
}
