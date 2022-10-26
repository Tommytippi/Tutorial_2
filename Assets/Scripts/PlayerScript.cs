using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpforce;
    private int score;
    private int scoreValue = 0;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    private int lives; 
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

      // Sprite & Animation
    Animator anim;
    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scoreText.text = scoreValue.ToString();
        lives = 3;

        musicSource.clip = musicClipTwo;
        musicSource.Play();
        musicSource.loop = true;

        SetScoreText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

                // Animation
        anim = GetComponent<Animator>();

        //Flip
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
           Debug.LogError("Player Sprite is missing a renderer");
        }
    }
void SetScoreText()
{
scoreText.text = "Score: " + scoreValue.ToString();
if (scoreValue == 4)
    {
        transform.position = new Vector2 (80f, 0f);
         SetLivesText();
    }
if (scoreValue >= 8)
{
    winTextObject.SetActive(true);
    musicSource.clip = musicClipTwo;
    musicSource.Stop();
    musicSource.clip = musicClipOne;
    musicSource.Play();
    musicSource.loop = false;
    speed = 0;
}
}

void SetLivesText()
{
    livesText.text = "Lives: " + lives.ToString();
if (lives<= 0)
{
    loseTextObject.SetActive(true);
    Destroy(gameObject);
}

}

  void Update()
    {
        //Flip
          if (Input.GetAxisRaw("Horizontal") > 0)
          {
            _renderer.flipX = false;
          }
          else if (Input.GetAxisRaw("Horizontal") < 0)
          {
            _renderer.flipX = true;
          }   

        //Right
        if (Input.GetKeyDown(KeyCode.D))
        {

          anim.SetInteger("State", 2);

         }
        
        if (Input.GetKeyUp(KeyCode.D))
        {

          anim.SetInteger("State", 0);

         }

        //Left
        if (Input.GetKeyDown(KeyCode.A))
        {

          anim.SetInteger("State", 2);

         }
         
         if (Input.GetKeyUp(KeyCode.A))
        {

          anim.SetInteger("State", 0);

         }

        //Sprint
        if (Input.GetKeyDown(KeyCode.R))
        {

          anim.SetInteger("State", 1);

         }

         if (Input.GetKeyUp(KeyCode.R))
        {
          
          anim.SetInteger("State", 0);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {

          anim.SetInteger("State", 3);

         }

         if (Input.GetKeyUp(KeyCode.W))
        {
          
          anim.SetInteger("State", 0);
        }

       float horizontal = Input.GetAxis("Horizontal");
       float vertical = Input.GetAxis("Vertical");
       Vector2 position = transform.position;
       position.x = position.x + 3.0f * horizontal * Time.deltaTime;
       position.y = position.y + 3.0f * vertical * Time.deltaTime;
       transform.position = position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

       isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            scoreText.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            SetScoreText();


        }
       if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            lives = lives - 1;

            SetLivesText();
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }
}