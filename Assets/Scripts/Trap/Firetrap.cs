
using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool triggered;// when the trap gets triggered
    private bool active; // When the trap is active and can hurt the player

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFireTrap());
            }
            if(active)
            {
                collision.GetComponent<Player>().Damage();
            }
        }
    }
    private IEnumerator ActivateFireTrap()
    {
        
        triggered = true;
        
        yield return new WaitForSeconds(activationDelay);
        active = true;

        animator.SetBool("activated", true); 
        yield return new WaitForSeconds(activeTime);
      
        active = false;
        triggered = false;
        animator.SetBool("activated", false);
    }
}
