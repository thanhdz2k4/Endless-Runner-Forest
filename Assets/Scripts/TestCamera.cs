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
    // Start is called before the first frame update
    void Start()
    {
        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        SpeedController();
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
