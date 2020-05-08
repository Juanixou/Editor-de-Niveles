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
                    Debug.Log("Pa la dcha");
                    this.transform.position = new Vector2(this.transform.position.x + 0.01f, this.transform.position.y);
                    actualDistance += 0.1f;
                    if (maxDistance != 0 && actualDistance >= maxDistance)
                    {
                        personaje.transform.Rotate(new Vector2(0, 180));
                        dcha = false;
                    }
                }
                else
                {
                    Debug.Log("Pa la izq");
                    this.transform.position = new Vector2(this.transform.position.x - 0.01f, this.transform.position.y);
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
        Debug.Log("Checkeandooo");
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);


        if (!grounded)
        {
            Debug.Log("Cambio de lado");
            dcha = !dcha;
            personaje.transform.Rotate(new Vector2(0, 180));
            StartCoroutine(WaitForChecking());
        }
    }

    IEnumerator WaitForChecking()
    {
        yield return new WaitForSeconds(2);

    }

}
