using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {
    [SerializeField]
    private PlayerInputs playerInput;

    private Rigidbody2D rb;
    public Animator anim;

    private Vector2 moveInput;

    public float speed;
    public float dashSpeed;
    public float dashTimeCounter, dashTime, dashCoolDown, dashCoolDownCounter;

    private Vector2 currDirection;
    private Vector2 dashDirection;

    private bool dashing = false, canDash = true;

    private float xVal = 0;
    private float yVal = 0;

    private bool facingRight = false;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        if (dashing) {
            Debug.Log("DASHING");
            dashTimeCounter -= Time.deltaTime;
           // rb.velocity = currDirection * dashSpeed;
            if (dashTimeCounter <= 0) {
                dashing = false;
                dashCoolDownCounter = dashCoolDown;
               // rb.velocity = currDirection * speed;
            }
        }
        else if(!canDash) {
            dashCoolDownCounter -= Time.deltaTime;
            if (dashCoolDownCounter <= 0) {
                canDash = true;
                Debug.Log("DASH RESET");
                Debug.Log("canDash: " + canDash);
            }
        }
    }

    private void FixedUpdate() {
        //Debug.Log(dashing);
        if(!dashing)
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
        else {
            //Debug.Log("DASHING");
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            Debug.Log(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        //if (!dashing) {
            anim.SetBool("Idle", false);

            moveInput = ctx.ReadValue<Vector2>() * speed;
            currDirection = rb.velocity;

            xVal = ctx.ReadValue<Vector2>().x;
            yVal = ctx.ReadValue<Vector2>().y;

            anim.SetBool("Walk", true);
            anim.SetTrigger("Clicked");

            anim.SetFloat("X", xVal);
            anim.SetFloat("Y", yVal);

            if (ctx.canceled) {
                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
                anim.SetTrigger("Clicked");
            }
        //}
    }

    public void OnDash(InputAction.CallbackContext ctx) {
        
        if (!dashing && canDash) {
            dashDirection = moveInput;
            dashing = true;
            canDash = false;
            dashTimeCounter = dashTime;
            Debug.Log(dashTimeCounter + " dash time counter");
        }
    }
}
