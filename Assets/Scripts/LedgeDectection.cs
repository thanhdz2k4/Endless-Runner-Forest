using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDectection : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask whatIsGround;
    Player player;
    private bool canDetected; 
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if (canDetected)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius,whatIsGround);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
