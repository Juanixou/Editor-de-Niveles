using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRayCast : MonoBehaviour
{
    public float visionRadius = 2.5f;
    public float attackRadius = 1;
    public float speed = 10f;

    GameObject player;
    GameObject animation;

    Vector3 initialPosition;
    public Vector3 centerDetectionPosition;
    private Vector3 flexibleDetectionPosition;

    public Animator anim;
    Rigidbody2D rb2d;

    private bool detected;

    //Ground check variables
    public Transform groundCheck;
    public bool grounded = false;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    private bool isFirstTime = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        animation = gameObject.transform.GetChild(0).gameObject;
        anim = animation.GetComponentInChildren<Animator>();
        //anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        detected = false;
        centerDetectionPosition.Set(initialPosition.x, initialPosition.y + centerDetectionPosition.y, initialPosition.z);
        flexibleDetectionPosition = centerDetectionPosition;
        isFirstTime = false;
    }

    void Update()
    {
        
        CheckGround();
        if (grounded)
        {
            AIBehaviour();
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(flexibleDetectionPosition, visionRadius);
        Gizmos.DrawWireSphere(flexibleDetectionPosition, attackRadius);
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);


        if (!grounded)
        {
            this.GetComponent<Rigidbody2D>().gravityScale=10;
            //AIBehaviour();
        }
        else
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (initialPosition.y != transform.position.y)
            {
                float yCenter = centerDetectionPosition.y - initialPosition.y;
                initialPosition = transform.position;
                centerDetectionPosition.Set(initialPosition.x, initialPosition.y + yCenter, initialPosition.z);
            }
        }
    }

    private void AIBehaviour()
    {
        flexibleDetectionPosition.Set(transform.position.x, centerDetectionPosition.y, transform.position.z);
        Vector3 target = centerDetectionPosition;
        RaycastHit2D hit = Physics2D.Raycast(
            flexibleDetectionPosition,
            player.transform.position - flexibleDetectionPosition,
            visionRadius,
            LayerMask.GetMask("Player"));
        
        if (hit.collider != null)
        {
            
            if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("MeleeWeapon")) && grounded && player.transform.position.y <= centerDetectionPosition.y)
            {
                target = new Vector3(player.transform.position.x, centerDetectionPosition.y, player.transform.position.z);

            }
        }

        float distance = Vector3.Distance(target, flexibleDetectionPosition);
        Vector3 dir = (target - flexibleDetectionPosition).normalized;

        if (target != centerDetectionPosition && distance < attackRadius)
        {
            //TODO: Activamos las animaciones de ataque
            anim.SetBool("Walking", false);
            anim.SetBool("Atacar", true);
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        else 
        {
            detected = true;
            anim.SetBool("Atacar", false);
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            //Nos movemos hacia el player
            //rb2d.MovePosition(new Vector2(transform.position.x + dir.x, transform.position.y + dir.y) * speed * Time.deltaTime);
            Vector2 positionMoved = new Vector2(transform.position.x + dir.x * speed * Time.deltaTime, transform.position.y);
            rb2d.MovePosition(positionMoved);
            anim.speed = 1;


            //TODO: Activamos animación de movimiento
            anim.SetBool("Walking", true);
        }

        //Evitamos bugs forzando la posicion inicial
        if (target == centerDetectionPosition && distance < 0.02f)
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

        Debug.DrawLine(flexibleDetectionPosition, target, Color.green);
    }

    public void SetInitialPosition(Vector3 newPos)
    {
        if (!isFirstTime)
        {
            float yCenter = centerDetectionPosition.y - initialPosition.y;
            initialPosition = newPos;
            centerDetectionPosition.Set(initialPosition.x, initialPosition.y + yCenter, initialPosition.z);
        }

        
    }

}
