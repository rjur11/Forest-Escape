using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private GameObject player;
    private float xVelocity = 0.0f;
    private float yVelocity = 0.15f;
   
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            if (gameObjects.Length > 0)
            {
                player = gameObjects[0];
            } else
            {
                return;
            }
        }
        Vector3 playerposition = player.transform.position;
        Vector3 cameraposition = transform.position;

        // cameraposition.x = playerposition.x;
        // cameraposition.y = playerposition.y;

        cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x, ref xVelocity, 0.5f);
        cameraposition.y = Mathf.SmoothDamp(cameraposition.y, playerposition.y, ref yVelocity, 0.5f);
        transform.position = cameraposition;
    }
}
