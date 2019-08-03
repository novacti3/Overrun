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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), rb.velocity.y);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundLayer);
        bool grounded = hit.transform != null && hit.transform.gameObject.layer == groundLayer ? true : false;
        return grounded;
    }
}