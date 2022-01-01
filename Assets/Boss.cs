using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour
{
    public Transform player;
    private Collider2D collider;
    private Animator animator;

    public bool isFlipped = false;
    private bool dying;
    private int STARTING_HEALTH = 6;
    private int health;
    

    public Text goblinLives;


    void Start()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        health = STARTING_HEALTH;
    }

    private void Update()
    {
        goblinLives.text = "Goblin Health: " + health;
        goblinLives.enabled = true;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1.0f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180.0f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180.0f, 0f);
            isFlipped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (ContactPoint2D hitPos in collision.contacts)
            {
                Debug.Log("Current normal: " + hitPos.normal);
                if (hitPos.normal.y < 0.0f)
                {
                    StartCoroutine(Hit());
                    break;
                }
                else if (!dying)
                {
                    IgnorePlayerCollisions();
                    GameManager.S.KillPlayer();
                    break;
                }
            }
        }
    }

    public void IgnorePlayerCollisions()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            Collider2D[] player_colliders = GameManager.S.currPlayer.GetComponents<Collider2D>();
            foreach (Collider2D player_collider in player_colliders)
            {
                Physics2D.IgnoreCollision(collider, player_collider);
            }
        }
    }

    public void UnignorePlayerCollisions()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            Collider2D[] player_colliders = GameManager.S.currPlayer.GetComponents<Collider2D>();
            foreach (Collider2D player_collider in player_colliders)
            {
                Physics2D.IgnoreCollision(collider, player_collider, false);
            }
        }
    }

    public IEnumerator Hit()
    {
        animator.SetTrigger("Hit");
        AudioManager.S.playGoblinHit();
        health--;
        GameManager.S.bossSpeed += 0.5f;
        if (health <= 0)
        {
            Die();
            yield break;
        }
        else
        {
            IgnorePlayerCollisions();
            yield return new WaitForSeconds(1.5f);
            UnignorePlayerCollisions();
        }
    }

    public void Die()
    {
        // switch to dying animation
        animator.SetTrigger("Dying");
        dying = true;
        IgnorePlayerCollisions();
        AudioManager.S.PlayEnemyDeath();

        GameManager.S.GameWon();

        // destroy the object
        Destroy(this.gameObject, 0.5f);
    }
}

