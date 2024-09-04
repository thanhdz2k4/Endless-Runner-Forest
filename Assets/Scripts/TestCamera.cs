using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [Header("Move info")]
    [SerializeField] float speedToSurvice = 18;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedMultiplier;
    private float defaultSpeed;
    private float speedMilestone;
    [SerializeField] float milestoneIncreaser;
    private float defaultMilestoneIncreaser;

    [Header("Player Reference")]
    [SerializeField] Player player; 

    [Header("Camera Follow Settings")]
    [SerializeField] float followSpeed = 2f; 
    [SerializeField] float stopFollowDelay = .1f; 

    private bool isFollowingPlayer = true;
    private float stopFollowTimer;

    void Start()
    {
        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            SpeedController();
        }

        if (player.isDead)
        {
            stopFollowTimer += Time.deltaTime;
            if (stopFollowTimer >= stopFollowDelay)
            {
                isFollowingPlayer = false;
            }
        }
        else
        {
            stopFollowTimer = 0;
            isFollowingPlayer = true;
        }

       
        if (player.rb.velocity.x == 0)
        {
            isFollowingPlayer = false;
        }
        else
        {
            isFollowingPlayer = true;
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
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
            if (moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
}
