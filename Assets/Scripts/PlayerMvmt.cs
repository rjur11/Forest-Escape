using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmt : MonoBehaviour
{

    public CharacterController2D controller;
    public float speed;

    private float horizontalMove = 0.0f;
    private bool jump = false;
    public bool dead = false;
    private AudioSource audioSource;
    public AudioClip pickupNoise;
    public AudioClip apple;

    private Animator animation;

    private float lastJumpInputTime = 0.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpInputTime = Time.time;
            jump = true;
            animation.SetBool("isOnGround", false);
        }

        float ourSpeed = Input.GetAxis("Horizontal");
        animation.SetFloat("Speed", Mathf.Abs(ourSpeed));
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dead)
        {
            return;
        }
        if (collision.gameObject.tag == "Finish")
        {
            StartCoroutine(GameManager.S.BeatTheLevel());
        }
        else if (collision.gameObject.tag == "Coin")
        {
            GameManager.S.score += 10;
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(pickupNoise);
        }
        else if (collision.gameObject.tag == "DeathZone")
        {
            GameManager.S.KillPlayer();
        }
        else if (collision.gameObject.tag == "Apple")
        {
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(apple);
            StartCoroutine(SpeedUp());
        }
    }

    public void PlayerLanded()
    {
        animation.SetBool("isOnGround", true);
        if (Time.time - lastJumpInputTime < 0.2f)
        {
            jump = true;
            animation.SetBool("isOnGround", false);
        }
    }

    public IEnumerator SpeedUp()
    {
        speed += 10;
        yield return new WaitForSeconds(3.0f);
        speed -= 10;
    }

}
