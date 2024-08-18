using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform[] levelPart;
    [SerializeField] Vector3 nextPartPosition;

    [SerializeField] float distanceToSpawn;
    [SerializeField] float distanceToDelete;
    [SerializeField] Transform player;

    private int levelIndex = 0;
    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }
    private void GeneratePlatform()
    {
        while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {
            Transform part;

            if (levelIndex == 0) 
            {
                part = levelPart[0];
            }
            else if (levelIndex == 1) 
            {
                part = levelPart[1];
            }
            else 
            {
                part = levelPart[Random.Range(2, levelPart.Length)];
            }

            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0f);
            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);
            nextPartPosition = newPart.Find("EndPoint").position;

            levelIndex++; 
        }
    }
    void DeletePlatform()
    {
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);
            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
                Destroy(partToDelete.gameObject);
        }
    }
   
}
