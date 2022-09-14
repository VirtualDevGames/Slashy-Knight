using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {
    [SerializeField]
    private PlayerInputs playerInput;

    private Rigidbody2D rb;
    public Animator anim;

    // MOVEMENT //
    public float speed;
    public float dashSpeed, dashDuration, dashDurationCounter, dashCoolDown, dashCoolDownCounter;
    private bool dashing = false, canDash = true;

    private Vector2 moveInput;
    private Vector2 dashDirection;

    // Values for animation
    private float xVal = 0;
    private float yVal = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        #region // Dashing //
        // Check dashing phase (dashing or cooldown)
        // Countdown timer for both
        if (dashing) { // Dashing
            dashDurationCounter -= Time.deltaTime;
            if (dashDurationCounter <= 0) {
                dashing = false;
                dashCoolDownCounter = dashCoolDown;
            }
        } // Cooldown
        else if (!canDash) {
            dashCoolDownCounter -= Time.deltaTime;
            if (dashCoolDownCounter <= 0) {
                canDash = true;
            }
        }
        #endregion
    }

    private void FixedUpdate() {

        #region // Dashing //
        if (!dashing)
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
        else {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
        }
        #endregion
    }

    public void OnMove(InputAction.CallbackContext ctx) {

        // Animation - Disable idle Enable walking
        anim.SetBool("Idle", false);
        anim.SetBool("Walk", true);
        anim.SetTrigger("Clicked");

        // Animation - Set character direction
        xVal = ctx.ReadValue<Vector2>().x;
        yVal = ctx.ReadValue<Vector2>().y;
        anim.SetFloat("X", xVal);
        anim.SetFloat("Y", yVal);

        // Animation - Disable walking Enable idle
        if (ctx.canceled) {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
            anim.SetTrigger("Clicked");
        }

        // Store direction input for movement
        moveInput = ctx.ReadValue<Vector2>() * speed;
    }

    public void OnDash(InputAction.CallbackContext ctx) {

        // Check if player can dash
        // Set booleans and cooldown
        if (canDash) {
            dashDirection = moveInput;
            dashing = true;
            canDash = false;
            dashDurationCounter = dashDuration;
        }
    }
}
