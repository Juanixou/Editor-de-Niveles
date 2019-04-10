using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {

    public float speed = 2.5f;
    bool grounded = false;
    bool walled = false;
    public Transform groundCheck;
    public Transform wallCheck;
    float groundRadius = 0.2f;
    float wallTouchRadius = 0.2f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float jumpForce=300f;
    public float jumpPushForce = 50f;
    public float jumpWallForce = 20f;
    private bool doubleJump = false;
    private float moveHorizontal = 0.0f;


    private bool caminando;
    private Rigidbody2D rb2d;

    private Animator anim;

    // Use this for initialization
    void Start () {
        caminando = false;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        Walk(moveHorizontal);

        GroundJump();

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("Correr", false);
        }

        if (Input.GetKeyUp(KeyCode.Space)&&(rb2d.velocity.y>=0))
        {
            //rb2d.AddForce(new Vector2(0, -(jumpForce - rb2d.velocity.y)));
            //En cuanto se levanta la tecla de salto, se atenua la velocidad de subida
            rb2d.velocity = new Vector2(0,0.5f);
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
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        walled = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, whatIsWall);
        anim.SetBool("Ground", grounded);

        if (grounded)
        {
            doubleJump = false;
            walled = false;
        }

        if (walled)
        {
            grounded = false;
            doubleJump = false;
        }
        if (rb2d.velocity.y <= 15.0f || rb2d.velocity.y >= -12.0f)
        {
            anim.SetFloat("vSpeed", rb2d.velocity.y);
            anim.SetFloat("JumpSpeed", rb2d.velocity.y);
        }


        //Parte utilizada para el salto
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Salto Vertical");
            anim.SetBool("Ground", false);
            rb2d.AddForce(new Vector2(0, jumpForce));
            if (!doubleJump && !grounded)
            {
                doubleJump = true;
            }

        }
        if (walled && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Salto Pared");
            WallJump();
        }

    }

    void WallJump()
    {
        rb2d.AddForce(new Vector2(jumpPushForce, jumpWallForce));
    }

}
