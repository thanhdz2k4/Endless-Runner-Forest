using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Enemy>() != null)
            Destroy(gameObject);

        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.Instance.PlaySFX(0);
            GameManager.Instance.coins++;
            Destroy(gameObject);
        }
    }
}
