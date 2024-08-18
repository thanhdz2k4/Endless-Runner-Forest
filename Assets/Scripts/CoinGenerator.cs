using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] int amountOfCoins;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int maxCoins;
    [SerializeField] int minCoins;

    [SerializeField] SpriteRenderer coinImage;
    
    void Start()
    {
        coinImage.sprite = null;
        amountOfCoins = Random.Range(minCoins, maxCoins);
        int additionalOffset = amountOfCoins / 2;
        for(int i = 0; i < amountOfCoins; i++)
        {
            Vector3 offset = new Vector2(i-additionalOffset, 0);
            Instantiate(coinPrefab,transform.position+ offset, Quaternion.identity,transform);
        }
        
    }

    
}
