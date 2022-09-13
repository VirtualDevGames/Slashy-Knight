using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager: MonoBehaviour
{
    [SerializeField]
    private PlayerInputs playerInput;

    private Rigidbody2D rb;

    public float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    /*private void OnEnable() {
        playerInput.Enable();
    }
    private void OnDisable() {
        playerInput.Disable();

    }*/

    public void OnMove(InputAction.CallbackContext ctx) {
        rb.velocity = ctx.ReadValue<Vector2>() * speed;
    }

}
