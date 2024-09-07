using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private float chanceToSpawn;
    //[SerializeField] private GameObject newCharacterPrefab; // New character prefab
    //[SerializeField] private Transform checkpoint; // Checkpoint position

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (Random.Range(0, 100) <= chanceToSpawn)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, respawnPosition.position, Quaternion.identity);
                Destroy(newEnemy, 30);
            }

           
            //Instantiate(newCharacterPrefab, checkpoint.position, Quaternion.identity);
        }
    }
}