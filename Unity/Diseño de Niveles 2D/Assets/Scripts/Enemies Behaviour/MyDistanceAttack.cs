using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDistanceAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public float floatHeight;     // Desired floating height.
    public float liftForce;       // Force to apply when lifting the rigidbody.
    public float damping;         // Force reduction proportional to speed (reduces bouncing).
    public float yOffset;

    private Animator anim;
    //Rigidbody2D rb2D;
    GameObject player;
    GameObject arrow;
    GameObject animation;

    private bool shoot = true;


    //Ground check variables
    public Transform groundCheck;
    public bool grounded = false;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //rb2D = GetComponent<Rigidbody2D>();
        animation = gameObject.transform.GetChild(0).gameObject;
        anim = GetComponentInChildren<Animator>();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    void FixedUpdate()
    {
        CheckGround();
        float playerDistance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)));
        CheckDir(player.transform.position);

        if (playerDistance<= 6.0 && shoot)
        {
            shoot = false;
            Shoot();
            
        }

    }

    void Shoot()
    {
        anim.SetBool("Shoot", true);
        StartCoroutine(WaitShootAnimation());

    }

    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(1);
        shoot = true;
    }
    IEnumerator WaitShootAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Shoot", false);
        arrow = Instantiate((GameObject)Resources.Load("prefabs/Arrow", typeof(GameObject)));
        arrow.transform.SetParent(GameObject.Find("Objects").transform, false);
        arrow.transform.position = this.transform.position;
        arrow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yOffset, this.transform.position.z);
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
        Vector3 dir = playerPos - arrow.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.GetComponent<ProjectileBehaviour>().enabled = true;
        StartCoroutine(WaitShoot());
    }

    public void CheckDir(Vector3 target)
    {
        if (target.x < this.transform.position.x)
        {
            animation.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            animation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);


        if (!grounded)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            this.GetComponent<Rigidbody2D>().gravityScale = 10;
            //AIBehaviour();
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            
        }
    }

}
