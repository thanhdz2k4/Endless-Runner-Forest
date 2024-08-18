using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer header;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        header.transform.parent = transform.parent;
        header.transform.localScale = new Vector2(spriteRenderer.bounds.size.x, .2f);
        header.transform.position = new Vector2(transform.position.x,spriteRenderer.bounds.max.y - .1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            header.color = GameManager.Instance.platformColor;
        }
       
    }

}
