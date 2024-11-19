using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;
    public float speed = 250.0f;
    public float jumpHeight = 2.5f;
    public float health = 100.0f;
    private bool isJumping = false;
    private float horizontalInput = 0.0f;
    private bool jumpInput = false;
    private float jumpImpulse;
    public float score = 0.0f;
    public float doubleJumpScore = 100.0f;
    private bool doubleJumpSpent = false;
    private bool doubleJumpEnabled = false;
    public GameObject ScoreCounterObject, PowerUpMessageObject, HealthCounterObject;
    private TextMeshProUGUI scoreCounter, powerUpMessage, HealthCounter;

    [SerializeField] private AudioClip audioJump, audioLand, audioHurt, audioHeal, audioPowerUp;
    public delegate void OnAreaChange(string name);
    public static event OnAreaChange onAreaChange;


    // Start is called before the first frame update
    void Start()
    {
        // Initializing Components
        rb2D = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();

        //Lock the rigidbody2D rotation to avoid bugs when touching the edges of platforms.
        rb2D.freezeRotation = true;

        // Setting the jump impulse according to the kinematic equations. The actual height is a bit lower, depending on the gravity applied to the character.
        jumpImpulse = rb2D.mass * Mathf.Sqrt(2.0f * Mathf.Abs(Physics2D.gravity.y) * rb2D.gravityScale * jumpHeight);

        // Initialize the UI elements with tag.
        //ScoreCounterObject = GameObject.FindWithTag("ScoreCounter");
        scoreCounter = ScoreCounterObject.GetComponent<TextMeshProUGUI>();
        scoreCounter.text = "0 $";
        //PowerUpMessageObject = GameObject.FindWithTag("PowerUpMessage");
        powerUpMessage = PowerUpMessageObject.GetComponent<TextMeshProUGUI>();
        powerUpMessage.text = "";
        HealthCounter = HealthCounterObject.GetComponent<TextMeshProUGUI>();
        HealthCounter.text = "Life: " + health;

    }

    // Update is called once per frame
    void Update()
    {
        // Inputs
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");


        // Behavior if Double Jump is Enabled
        if (doubleJumpEnabled && doubleJumpSpent == false)
        {
            if (jumpInput && isJumping == true)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                Jump();
                doubleJumpSpent = true;
                animator.SetBool("firstJump", false);
                animator.SetBool("secondJump", true);
            }
            
        }

        // Regular Jump Behavior
        if (jumpInput && isJumping == false)
        {
            Jump();
            animator.SetBool("firstJump", true);
        }

        // Testing the maximum reached height for debugging.
        /*if(Mathf.Abs(rb2D.velocity.y) < 0.1f && isJumping == true)
        {
            Debug.Log(rb2D.position.y);
        }*/

    }

    private void FixedUpdate()
    {

        // Horizontal Movement
        rb2D.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb2D.velocity.y);

        //Animation
        if (horizontalInput < 0)
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput == 0)
        {
            animator.SetBool("isWalking", false);
        }



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            
        if (collision.gameObject.tag == "Floor")
        {      
            // Play landing audio if the player comes from a jump.    
            if (isJumping == true)
            {
                audioSource.PlayOneShot(audioLand);
            }
            
            // Recharge Jumping and Double Jump upon touching a floor.
            isJumping = false;
            doubleJumpSpent = false;
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

            // Exit jump animations
            animator.SetBool("firstJump", false);
            animator.SetBool("secondJump", false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 15;
            HealthCounter.text = "Life: " + health;
            audioSource.PlayOneShot(audioHurt);
        }
        



        // Make the transform a child of the platform in case it's a moving platform.
        if (collision.gameObject.layer==LayerMask.NameToLayer("Platform") | collision.gameObject.layer==LayerMask.NameToLayer("InvisiblePlatform"))
        {
            this.transform.SetParent(collision.gameObject.transform, true);
        }

        // Make invisible platforms visible upon collision.
        if (collision.gameObject.layer==LayerMask.NameToLayer("InvisiblePlatform"))
        {
            Material mat = collision.gameObject.GetComponent<SpriteRenderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Keep the jumping reset when in collision with a "Floor" object, to avoid a bug where Jump is disabled when exiting a collision while still colliding with a different object.
        if (collision.gameObject.tag == "Floor")
        {
            isJumping = false;
            doubleJumpSpent = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {          
            // Enable isJumping so it can't jump again after falling from a platform.
            isJumping = true;
        }
        
        // Detach the transform from the platform when leaving it.
        if (collision.gameObject.layer==LayerMask.NameToLayer("Platform") | collision.gameObject.layer==LayerMask.NameToLayer("InvisiblePlatform"))
        {
            this.transform.SetParent(null);
        }

        // Make invisible platforms invisible again upon leaving them.
        if (collision.gameObject.layer==LayerMask.NameToLayer("InvisiblePlatform"))
        {
            Material mat = collision.gameObject.GetComponent<SpriteRenderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        // Collect power ups and increase the score
        if (trigger.gameObject.tag == "PowerUp")
        {
            score += 20.0f;
            scoreCounter.text = score + "$";
            audioSource.PlayOneShot(audioPowerUp);
            Destroy(trigger.gameObject);
            

            // Enable Double Jump upon reaching a high score
            if (score >= doubleJumpScore)
            {
                doubleJumpEnabled = true;
                powerUpMessage.text = "Double Jump Unlocked!";
            }


        }

        if (trigger.gameObject.CompareTag("Healing"))
        {
            health += 15.0f;
            HealthCounter.text = "Life: " + health;
            audioSource.PlayOneShot(audioHeal);
            Destroy(trigger.gameObject);
        }

        if (trigger.gameObject.layer == LayerMask.NameToLayer("AreaLimits"))
        {
            onAreaChange?.Invoke(trigger.gameObject.name);
        }
    }
    
    private void Jump()
    {
        rb2D.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        audioSource.PlayOneShot(audioJump);
        isJumping = true;
    }
}