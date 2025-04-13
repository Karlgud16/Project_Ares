using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMove;
    private PlayerAttack playerAttack;
    void Awake()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    public void OnMove(CallbackContext context)
    {
       playerMove.PlayerDirection(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        if (context.performed)
        {
            playerMove.PlayerJump();
        }
    }

    public void OnDash(CallbackContext context)
    {
        if (context.performed)
        {
            playerMove.PlayerDodge();
        }
    }

    public void OnBlock(CallbackContext context)
    {
        if (context.performed)
        {
            playerMove.PlayerBlock();
        }
    }

    public void OnLightAttack(CallbackContext context)
    {
        if (context.performed)
        {
            playerAttack.LightAttack();
        }
    }
    public void OnHeavyAttack(CallbackContext context)
    {
        if (context.performed)
        {
            playerAttack.HeavyAttack();
        }
    }

    public void OnInteract(CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(Interact());
        }
    }

    IEnumerator Interact()
    {
        playerMove.CanInteract = true;
        yield return new WaitForSeconds(0.1f);
        playerMove.CanInteract = false;
    }
}
