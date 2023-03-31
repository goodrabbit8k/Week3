using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool isAlive;

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float damageKick = 1f;

    Rigidbody2D playerRb;
    Animator playerAnim;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    HealthManager playerHealth;

    Vector2 movementInput;
    float currentGravity;
    int damageCooldown = 1;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<HealthManager>();

        currentGravity = playerRb.gravityScale;

        isAlive = true;
    }

    void Update()
    {
        Run();
        Climb();
        ChangePlayerLookDirection();

        if (playerHealth.currentHealth == 0)
        {
            isAlive = false;
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            if (value.isPressed && isAlive)
            {
                playerRb.velocity = new Vector2(0f, jumpSpeed);
                playerAnim.SetBool("isJumping", true);
            }
        }
    }

    void Run()
    {
        Vector2 playerVel = new Vector2(movementInput.x * runSpeed, playerRb.velocity.y);
        bool playerRun = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (isAlive)
        {
            playerRb.velocity = playerVel;

            playerAnim.SetBool("isRunning", playerRun);
        }
    }

    void Climb()
    {
        Vector2 climbVel = new Vector2(playerRb.velocity.x, movementInput.y * climbSpeed);
        bool playerClimbing = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;

        playerAnim.SetBool("isClimbing", false);

        playerRb.gravityScale = currentGravity;

        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && isAlive)
        {
            playerRb.gravityScale = 0f;
            playerRb.velocity = climbVel;
            playerAnim.SetBool("isClimbing", playerClimbing);
        }
    }

    void ChangePlayerLookDirection()
    {
        bool playerRun = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerRun)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platforms" || collision.gameObject.tag == "Ladder")
        {
            playerAnim.SetBool("isJumping", false);
        }    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerRb.velocity = Vector2.up * damageKick;
            playerHealth.currentHealth -= 1;
            playerAnim.SetBool("isHitting", true);
            StartCoroutine(PlayerDamageCooldown());
        }    
    }

    IEnumerator PlayerDamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        playerAnim.SetBool("isHitting", false);
    }
}
