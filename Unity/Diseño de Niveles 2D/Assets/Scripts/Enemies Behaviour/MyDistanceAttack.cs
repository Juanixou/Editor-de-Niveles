using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDistanceAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public float floatHeight;     // Desired floating height.
    public float liftForce;       // Force to apply when lifting the rigidbody.
    public float damping;         // Force reduction proportional to speed (reduces bouncing).

    private Animator anim;
    //Rigidbody2D rb2D;
    GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position);

        float playerDistance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)));
        Debug.Log("Code Distance = " + playerDistance);

        if(playerDistance<= 6.0)
        {
            Shoot();
            anim.SetBool("Shoot", true);
        }

    }

    void Shoot()
    {
        GameObject instancia = Instantiate((GameObject)Resources.Load("prefabs/Espinas", typeof(GameObject)));
        instancia.transform.SetParent(GameObject.Find("Canvas").transform, false);
        instancia.transform.LookAt(player.transform);
        instancia.transform.Translate(player.transform.position);
    }

}
