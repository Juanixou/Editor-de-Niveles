using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {

    public float speed = 2.5f;
    bool grounded = false;
    bool walled = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float jumpForce;
    public float shortJumpForce = 300f;
    public float longJumpForce = 700f;
    public float wallForce = 200f;
    private bool doubleJump = false;
    private float moveHorizontal = 0.0f;
    private float counter = 0.0f;

    private bool caminando;
    private Rigidbody2D rb2d;

    private Animator anim;

    // Use this for initialization
    void Start () {
        caminando = false;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        Walk(moveHorizontal);

        if (Input.GetKey(KeyCode.Space))
        {
            
            counter += Time.deltaTime;
            Debug.Log("Counter: " + counter);
            if (counter <= 50.0)
            {

                jumpForce = shortJumpForce;
            }
            else
            {

                jumpForce = longJumpForce;
            }
        }

        GroundJump();

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("Correr", false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            
            counter = 0.0f;

        }

    }

    private void FixedUpdate()
    {

    }

    //Comprueba la dirección del personaje, así como si ha presionado la tecla para correr. Establece su velocidad y animacion.
    public void Walk(float velocity)
    {
        if (velocity > 0.0f)
        {
            caminando = true;
            anim.SetBool("Caminar", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (velocity == 0)
        {
            anim.SetBool("Caminar", false);
            caminando = false;
        }
        else
        {
            caminando = true;
            anim.SetBool("Caminar", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(velocity*speed, rb2d.velocity.y);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        if ((caminando) && (!Input.GetKey(KeyCode.LeftShift)))
        {
            rb2d.velocity = movement;
        }else if((caminando) && (Input.GetKey(KeyCode.LeftShift)))
        {
            Running(movement);
        }
        
    }

    public void Running(Vector2 move)
    {
        anim.SetBool("Correr", true);
        rb2d.velocity = new Vector2(move.x * 2.0f  ,rb2d.velocity.y);
    }

    public void GroundJump()
    {
        counter++;
        if(counter <= 30)
        {

        }
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        anim.SetBool("Ground", grounded);

        if (grounded)
        {
            doubleJump = false;
        }
        anim.SetFloat("vSpeed", rb2d.velocity.y);

        //Parte utilizada para el salto
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Ground", false);
            rb2d.AddForce(new Vector2(0, jumpForce));
            if (!doubleJump && !grounded)
            {
                doubleJump = true;
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "Pared")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (moveHorizontal >= 0)
                {
                    rb2d.AddForce(new Vector2(-900, jumpForce));
                }
                else
                {
                    rb2d.AddForce(new Vector2(900, jumpForce));
                }
            }
        }
    }


}
