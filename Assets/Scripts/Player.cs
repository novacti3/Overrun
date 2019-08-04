using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDied;

    [SerializeField]
    private int hp = 3;
    private int maxHP;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float airControlAmount = 5f;

    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    public bool isRolling;
    private bool waitToStopRoll = false;

    [SerializeField]
    private float damageTakenInvincibilityDuration = 1f;
    private bool isInvincible = false;

    private Vector2 direction;

    Animator animator;

    [SerializeField]
    AudioSource jump;

    [SerializeField]
    AudioSource hurt;

    [SerializeField]
    AudioSource roll;
    
    [SerializeField]
    AudioSource death;
    
    bool dead = false;



    void Start()
    {
        animator = GetComponent<Animator>();
        direction = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        maxHP = hp;
    }
    private void FixedUpdate()
    {
        //In air movement to keep jumping velocity and stop insane movement speeds
        if(!IsGrounded()) {
            if(rb.velocity.x > -8 && Input.GetAxisRaw("Horizontal") < 0){
                rb.AddForce(new Vector2(-speed * airControlAmount, 0));
            } else if(rb.velocity.x < 8 && Input.GetAxisRaw("Horizontal") > 0) {
                rb.AddForce(new Vector2( speed * airControlAmount, 0));
            }
        } 

        
        if(isRolling) {
            transform.Rotate(new Vector3(0,0,12.25f));
        } else {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }  
    }
    void Update()
    { 
        if(dead && !death.isPlaying) {
            Destroy(gameObject);
        }
        animator.SetBool("Rolling", isRolling);
        animator.SetBool("Grounded", IsGrounded());
        animator.SetFloat("Y Velocity" , rb.velocity.y);

        //Get direction for roll, this still feels dodgy and needs worked on
        if(Input.GetAxisRaw("Horizontal") != 0) {
            direction = new Vector2(Mathf.Floor(Mathf.Clamp(rb.velocity.x, -1, 1)), 0);
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }

        //Walk
        if(!isRolling && IsGrounded())
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        //Jump
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && !isRolling) {

            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jump.Play();
        }

        //Wall jump
        if(!IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            WallJump();
        }

        //Roll
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isRolling) {
            StartCoroutine("Roll");
        }

        //Wait till player on ground to stop rolling
        if(waitToStopRoll && IsGrounded()) {
            isRolling = false;
            waitToStopRoll = false;
        }
 
    }

    // Whether the player is on the ground or not
    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hit;
           
        
    }

    //Public function to get player direction
    public Vector2 GetDirection() {
        return direction;
    }

    // Whether the player is on the wall or not
    bool IsWalled(Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, wallLayer);
        return hit;
    }

    //Wall jump
    void WallJump() {
        jump.Play();
        //If player is next to right wall, make em roll and apply force to the left
        if(IsWalled(Vector2.right)) {
            rb.AddForce(Vector2.right * -1 * jumpForce + new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isRolling = true;
            waitToStopRoll = true;
        }
        //If player is next to left wall, make em roll and apply force to the right
        if(IsWalled(Vector2.right * -1)) {
            rb.AddForce(Vector2.right * jumpForce + new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isRolling = true;
            waitToStopRoll = true;
        }
    }

    //Roll function
    IEnumerator Roll() {
        
        roll.Play();
        isRolling = true;
        isInvincible = true;
        //Add force for 0.33 seconds
        for(float time = 0f; time < 1f; time += Time.fixedDeltaTime * 3) {
            //Reduces force added at end of roll
            rb.velocity = direction * rollSpeed;
                
             Debug.Log(time);
            yield return null;
        }

        //At end of roll stop roll unless in air
        if(IsGrounded()) {
            isRolling = false;
        } else {
            waitToStopRoll = true;
        }
        isInvincible = false;
    }

    // Makes the player invincible for a period of time
    IEnumerator Invincibility(float duration)
    {
        animator.SetBool("Invincible", true);
        Debug.Log("Started invincibility");
        isInvincible = true;
        gameObject.layer = 16;
        yield return new WaitForSeconds(duration);
        gameObject.layer =11;
        isInvincible = false;
        Debug.Log("Ended invincibility");
        animator.SetBool("Invincible", false);
    }

    public void TakeDamage()
    {
        if(!hurt.isPlaying) 
            hurt.Play();
        
        GameMaster.Instance.PlayerDamage();
        hp--;
        Debug.Log("HP: " + hp);
        if (hp <= 0)
        {
            dead = true;
            death.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            OnPlayerDied?.Invoke();
        }
        // Starts the invincibility 
        StartCoroutine(Invincibility(damageTakenInvincibilityDuration));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        // Damages the player on collision with the enemy
        if (enemy != null && !isInvincible)
        {
            TakeDamage();
        }

        Key key = collision.gameObject.GetComponent<Key>();
        if (key)
        {
            key.Pickup();
        }
    }

}