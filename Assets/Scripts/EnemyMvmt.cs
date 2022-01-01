using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMvmt : MonoBehaviour
{

    public float speed;
    private bool faceLeft = true;

    private Animator animator;

    private CharacterController2D controller;
    private bool dying;
 
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        dying = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        float horizontalMove = speed * Time.fixedDeltaTime;

        if (faceLeft)
        {
            horizontalMove *= -1.0f;
        }

        controller.Move(horizontalMove, false, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turnaround")
        {
            Debug.Log("Hit the collider");
            faceLeft = !faceLeft;
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
                    Die();
                    break;
                }
                else if (!dying)
                {
                    GameManager.S.KillPlayer();
                    break;
                }
            }
        }
    }

    public void Die()
    {
        if (dying)
        {
            return;
        }
        // switch to dying animation
        animator.SetTrigger("Dying");
        dying = true;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameManager.S.currPlayer.GetComponent<Collider2D>());
        AudioManager.S.PlayEnemyDeath();

        // destroy the object 
        Destroy(this.gameObject, 0.5f);
    }
}
