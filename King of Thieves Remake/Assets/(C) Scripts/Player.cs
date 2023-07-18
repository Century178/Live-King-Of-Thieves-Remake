using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region(Variables)
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float slideSpeed = 1.25f;

    private Vector2 spawnPoint;

    private float jumpWindow = 0.25f;
    private float jumpBuffer;

    private float coyoteTime = 0.15f;
    private float airTime;

    private bool isJumping;
    private bool isGrounded;

    private bool isWallSliding;
    private bool isWalled;

    [Header("Ground & Wall Checks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Vector2 wallCheckSize;

    private Rigidbody2D rb;
    #endregion

    #region(Regular)
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        isWalled = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);

        Jump();

        if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
        {
            isJumping = true;

            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isWallSliding) rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }

        WallSlide();
    }
    #endregion

    #region(Collisions)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            transform.position = spawnPoint;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Reverse")) Flip();

        if (collision.gameObject.CompareTag("Goal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion

    #region(Custom)
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBuffer = jumpWindow;
        }
        else
        {
            jumpBuffer -= Time.deltaTime;
        }

        if (isGrounded)
        {
            airTime = coyoteTime;
        }
        else
        {
            if (isJumping) airTime = 0;
            else airTime -= Time.deltaTime;
        }

        if (jumpBuffer > 0 && airTime > 0)
        {
            isJumping = true;
            jumpBuffer = 0;
        }
    }

    private void WallSlide()
    {
        if (isWalled && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        moveSpeed *= -1f;
    }
    #endregion
}
