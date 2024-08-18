using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;


    private bool isDead;
    [HideInInspector] public bool playerUnlooker;
    [HideInInspector] public bool extraLife;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Move info")]
    [SerializeField] float moveSpeed ;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedMultiplier;//bộ nhân tốc độ
    private float defaultSpeed;
    [Space]
    [SerializeField] float milestoneIncreaser;//bộ tăng cột mốc
    private float defaultMilestoneIncreaser;
    private float speedMilestone; // cột mốc tốc độ

    [Header("Jump info")]
    [SerializeField] float jumpForce ;
    [SerializeField] float doubleJumpForce ;
    private bool canDoubleJump;


    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    private float slideTimerCounter;
    private bool isSliding;
    [SerializeField] private float slideCooldown ;
    private float slideCoolDownCounter;

    [Header("Collision info")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] float ceillingCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    private bool wallDetected;
    private bool isGrounded;
    private bool ceillingDetected;

    [HideInInspector] public bool ledgeDetected;

    [Header("Ledge info")]
    [SerializeField] private Vector2 preClimbOffset; //độ dời cho vị trí trước khi leo
    [SerializeField] private Vector2 postClimbOffset;//độ dời cho vị trí sau khi leo
    private Vector2 climbBegunPosition;//vị trí bắt đầu leo 
    private Vector2 climbOverPosition;//vị trí leo qua 

    private bool canGrabLedge = true; //có thể bám cạnh
    private bool canClimb;//có thể leo

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        
        slideCoolDownCounter = slideCooldown;
        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;

    }

    void Update()
    {
        CheckCollision();
        AnimatorControllers();
        slideTimerCounter -= Time.deltaTime;
        slideCoolDownCounter -= Time.deltaTime;

        extraLife = moveSpeed >= maxSpeed;
        if (Input.GetKeyDown(KeyCode.K) && !isDead)
        {
            Knockback();
        }
        if (Input.GetKeyDown(KeyCode.O) && !isDead)
        {
            StartCoroutine(Die());
        }

        if (isDead)
        {
            return;
        }
        if (isKnocked)
        {
            return;
        }
        if (playerUnlooker)
        {
            SetupMovement();
        }
        if (isGrounded)
        {
            canDoubleJump = true;
            
        }
        SpeedController();
        CheckForLedge();
        CheckForSlideCancel();
        CheckInput();
    }
    public void Damage()
    {
        if (extraLife)
        {
            Knockback();
        }
        else
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        isDead = true;
        rb.velocity = knockbackDir;
        animator.SetBool("isDead",true);
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.RestartLevel();

    }
    #region Knockback
    private IEnumerator Invincibility()
    {
        Color originalColor = spriteRenderer.color;
        Color darkenColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .5f);
        canBeKnocked = false;
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.1f);

        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.15f);

        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.15f);

        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.2f);

        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.2f);

        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.3f);

        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.3f);

        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.4f);

        spriteRenderer.color = originalColor;
        canBeKnocked = true;
    }

    private void Knockback()
    {
        if(!canBeKnocked)
        {
            return;
        }
        StartCoroutine(Invincibility());
        isKnocked = true;
        rb.velocity = knockbackDir;
    }
    private void CancelKnockback() => isKnocked = false;
    #endregion
    #region SpeedControll
    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }
   private void SpeedController()
    {
        if (moveSpeed == maxSpeed)
        {
            return;
        }
        if (transform.position.x > speedMilestone)
        {
            speedMilestone = speedMilestone + milestoneIncreaser;
            moveSpeed *= speedMultiplier;
            milestoneIncreaser = milestoneIncreaser * speedMultiplier;
            if(moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
    #endregion
    #region Ledge Climb Redion
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDectection>().transform.position;
            climbBegunPosition = ledgePosition + preClimbOffset;
            climbOverPosition = ledgePosition + postClimbOffset;
            canClimb = true;
        }
        if (canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }
    private void LedgeClimbOver()
    {
        canClimb = false;
        rb.gravityScale = 5;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }
    private void AllowLedgeGrab() => canGrabLedge = true;
    #endregion
    private void CheckForSlideCancel()
    {
        if (slideTimerCounter < 0 && !ceillingDetected)
        {
            isSliding = false;
        }
    }

    private void SetupMovement()
    {
        if (wallDetected)
        {
            SpeedReset();
            return;
        }
        if (isSliding)
        {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    #region Inputs
    private void SlideButton()
    {
        if (rb.velocity.x != 0 && slideCoolDownCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCoolDownCounter = slideCooldown;
        }
    }

    private void JumpButton()
    {
        if (isSliding)
        {
            return;
        }


        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }
    private void CheckInput()
    {
        /*
        if (Input.GetButton("Fire2"))
        {
            playerUnlooker = true;
        }
        */
        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideButton();
        }
    }
    #endregion
    #region Animations
    private void AnimatorControllers()
    {
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetFloat("yVelocity", rb.velocity.y);

        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSiding", isSliding);
        animator.SetBool("canClimb",canClimb);
        animator.SetBool("isKnocked" , isKnocked);
        if (rb.velocity.y < -30)
        {
            animator.SetBool("canRoll", true);
        }
            
    }
    private void RollAnimationFinished() => animator.SetBool("canRoll",false);
    #endregion
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
    
}
