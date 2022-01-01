using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Reached a checkpoint. Player is at position: " + collision.gameObject.transform.position);

            GameManager.S.lastCheckPointPosition = collision.gameObject.transform.position;
        }
    }
}

