using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{

    public float minDistance = 50.0f;
    public float maxDistance = 500.0f;
    private float actualDistance;
    private bool dcha;
    private GameObject personaje;
    public GameObject healthBar;
    public float speed;

    private Animator anim;

    //Ground check variables
    public Transform groundCheck;
    public bool grounded = false;
    //Defect value for Enemy 1
    public float groundRadius = 0.074f;
    public LayerMask whatIsGround;


    // Start is called before the first frame update
    void Start()
    {
        dcha = true;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Walking", true);
        actualDistance = minDistance;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "Animation")
            {

                personaje = this.transform.GetChild(i).gameObject;
            }else if (this.transform.GetChild(i).name == "Feet")
            {
                groundCheck = this.transform.GetChild(i).transform;
            }
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        CheckGround();

    }

    public void EnemyMovement()
    {
        string name = this.name.Replace("(Clone)", "");
        switch (name)
        {
            case "Enemy 1":
                if (dcha)
                {
                    this.transform.position = new Vector3(this.transform.position.x + speed, this.transform.position.y,this.transform.position.z);
                    actualDistance += 0.1f;
                    if (maxDistance != 0 && actualDistance >= maxDistance)
                    {
                        personaje.transform.Rotate(new Vector2(0, 180));
                        dcha = false;
                    }
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x - speed, this.transform.position.y, this.transform.position.z);
                    actualDistance -= 0.1f;
                    if (minDistance != 0 && actualDistance <= minDistance)
                    {
                        personaje.transform.Rotate(new Vector2(0, 180));
                        dcha = true;
                    }
                }
                break;
            default:
                break;
        }

        
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);


        if (!grounded)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            this.GetComponent<Rigidbody2D>().gravityScale = 10;
            dcha = !dcha;
            personaje.transform.Rotate(new Vector2(0, 180));
            StartCoroutine(WaitForChecking());
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    IEnumerator WaitForChecking()
    {
        yield return new WaitForSeconds(2);

    }

}
