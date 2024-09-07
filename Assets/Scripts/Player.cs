using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
  


    public bool isDead;
    [HideInInspector] public bool playerUnlooker;
    [HideInInspector] public bool extraLife;

    [Header("VFX")]
    [SerializeField] ParticleSystem dustFx;
    [SerializeField] ParticleSystem bloodFx;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Move info")]
    [SerializeField] float speedToSurvice = 18;
    [SerializeField] float moveSpeed ;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedMultiplier;
    private float defaultSpeed;
    [Space]
    [SerializeField] float milestoneIncreaser;
    private float defaultMilestoneIncreaser;
    private float speedMilestone; 

    private bool readyToLand;

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
    [HideInInspector] public float slideCoolDownCounter;

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
    [SerializeField] private Vector2 preClimbOffset; 
    [SerializeField] private Vector2 postClimbOffset;
    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true; 
    private bool canClimb;


    private float obstacleTimer;
    private bool isObstacleDetected;
    private bool obstacleTimerStarted;

    [SerializeField]
    UnityEvent eventStuckAction;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        extraLife = moveSpeed >= speedToSurvice;
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
        CheckForLanding();

        CheckForLedge();
        CheckForSlideCancel();
        CheckInput();

        UpdateObstacleTimer();
    }

    private void StartObstacleTimer()
    {
        if (!obstacleTimerStarted)
        {
            Debug.Log("Obstacle detected, starting timer.");
            obstacleTimer = 3f; 
            isObstacleDetected = true;
            obstacleTimerStarted = true; 
        }
    }

    public void StopObstacleTimer()
    {
        Debug.Log("Obstacle cleared, stopping timer.");
        isObstacleDetected = false;
        obstacleTimerStarted = false; 
    }

    private void UpdateObstacleTimer()
    {
        if (isObstacleDetected)
        {
            obstacleTimer -= Time.deltaTime;
            if (obstacleTimer <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    private void CheckForLanding()
    {
        if (rb.velocity.y < -5 && !isGrounded)
        {
            readyToLand = true;
        }
        if (readyToLand && isGrounded)
        {
            dustFx.Play();
            readyToLand = false;
        }
    }

    public void Damage()
    {
        bloodFx.Play();
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
        AudioManager.Instance.PlaySFX(3);
        isDead = true;
        rb.velocity = knockbackDir;
        animator.SetBool("isDead",true);

        Time.timeScale = .6f;
        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        rb.velocity = new Vector2(0, 0);
        GameManager.Instance.GameEnded();

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
        if (isSliding)
        {
            return;
        }
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
    public void SlideButton()
    {
        if (isDead)
        {
            return;
        }
        if (rb.velocity.x != 0 && slideCoolDownCounter < 0)
        {
            dustFx.Play();
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCoolDownCounter = slideCooldown;
        }
    }

    public void JumpButton()
    {
        if (isSliding || isDead)
        {
            return;
        }
        RollAnimationFinished(); 

        if (isGrounded)
        {
            Jump(jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump(doubleJumpForce);
        }
    }
    private void Jump(float force)
    {
        dustFx.Play();
        AudioManager.Instance.PlaySFX(Random.Range(1, 2));
        rb.velocity = new Vector2 (rb.velocity.x, force);
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

        if(wallDetected)
        {
            eventStuckAction?.Invoke();
            StartObstacleTimer();
        }
        else
        {
            StopObstacleTimer(); 
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            SwitchToNewCharacter(collision.transform);
        }
    }

    private void SwitchToNewCharacter(Transform checkpoint)
    {
        GameObject newCharacter = GameObject.FindWithTag("NewCharacter");
        if (newCharacter != null)
        {
            Player newPlayer = newCharacter.GetComponent<Player>();
            if (newPlayer != null)
            {
                newPlayer.moveSpeed = this.moveSpeed;
                newPlayer.isDead = this.isDead;
                newPlayer.extraLife = this.extraLife;
                this.enabled = false;
                newPlayer.enabled = true;
                Destroy(this.gameObject);
            }
        }
    }
    */

}


