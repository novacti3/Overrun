using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private float rollSpeed;

    [SerializeField]
    private bool rolling;

    private bool waitToStopRoll = false;

    private Vector2 direction;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        if(Input.GetAxisRaw("Horizontal") != 0) {
            direction = new Vector2(Mathf.Floor(Mathf.Clamp(rb.velocity.x, -1, 1)), 0);
        }

        if(!rolling && IsGrounded())
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        if(!IsGrounded()) {
            if(rb.velocity.x > -8 && Input.GetAxisRaw("Horizontal") < 0){
                rb.AddForce(new Vector2(-speed, 0));
            } else if(rb.velocity.x < 8 && Input.GetAxisRaw("Horizontal") > 0) {
                rb.AddForce(new Vector2( speed, 0));
            }
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && !rolling)
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        if(!IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            WallJump();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !rolling) {
            StartCoroutine("Roll");
        }

        if(waitToStopRoll && IsGrounded()) {
            rolling = false;
            waitToStopRoll = false;
        }
 
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        return hit;
    }

    bool IsWalled(Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, wallLayer);
        return hit;
    }

    void WallJump() {
        if(IsWalled(Vector2.right)) {
            rb.AddForce(Vector2.right * -1 * jumpForce + new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rolling = true;
            waitToStopRoll = true;
        }
        if(IsWalled(Vector2.right * -1)) {
            rb.AddForce(Vector2.right * jumpForce + new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rolling = true;
            waitToStopRoll = true;
        }
    }

    IEnumerator Roll() {
        rolling = true;
        for(float time = 0f; time < 1f; time += Time.deltaTime * 3) {
               rb.AddForce(direction * rollSpeed * (1-time), ForceMode2D.Impulse);     
            
            yield return null;
        }
        if(IsGrounded()) {
            rolling = false;
        } else {
            waitToStopRoll = true;
        }
    }
}