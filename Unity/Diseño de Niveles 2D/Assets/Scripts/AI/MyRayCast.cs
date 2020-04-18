using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRayCast : MonoBehaviour
{
    public float visionRadius = 2.5f;
    public float attackRadius = 1;
    public float speed = 1.5f;

    GameObject player;
    GameObject animation;

    Vector3 initialPosition;

    public Animator anim;
    Rigidbody2D rb2d;

    private bool detected;

    //Ground check variables
    public Transform groundCheck;
    public bool grounded = false;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        initialPosition = transform.position;
        animation = gameObject.transform.GetChild(0).gameObject;
        anim = animation.GetComponentInChildren<Animator>();
        //anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        detected = false;
    }

    void Update()
    {

        CheckGround();
        AIBehaviour();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);


        if (grounded)
        {
            this.GetComponent<Rigidbody2D>().gravityScale=10;
            //AIBehaviour();
        }
    }

    private void AIBehaviour()
    {
        Vector3 target = initialPosition;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            LayerMask.GetMask("Player"));
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;

            }
        }

        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        if (target != initialPosition && distance < attackRadius)
        {
            //TODO: Activamos las animaciones de ataque
            anim.SetBool("Walking", false);
            anim.SetBool("Atacar", true);
        }
        else
        {
            detected = true;
            anim.SetBool("Atacar", false);
            //Nos movemos hacia el player
            //rb2d.MovePosition(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y) * speed * Time.deltaTime);
            rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);
            anim.speed = 1;


            //TODO: Activamos animación de movimiento
            anim.SetBool("Walking", true);
        }

        //Evitamos bugs forzando la posicion inicial
        if (target == initialPosition && distance < 0.02f)
        {
            transform.position = initialPosition;
            //TODO: Pasamo a idle
            anim.SetBool("Walking", false);
            anim.SetBool("Atacar", false);
            detected = false;
        }

        if (target.x < this.transform.position.x && detected)
        {
            animation.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            animation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Debug.DrawLine(transform.position, target, Color.green);
    }

}
