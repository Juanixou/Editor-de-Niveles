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
    GameObject arrow;

    private bool shoot = true;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {

        float playerDistance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)));

        if(playerDistance<= 6.0 && shoot)
        {
            Debug.Log("Entramos en if");
            shoot = false;
            Shoot();
            
        }

    }

    void Shoot()
    {
        anim.SetBool("Shoot", true);
        arrow = Instantiate((GameObject)Resources.Load("prefabs/Arrow", typeof(GameObject)));
        arrow.transform.SetParent(GameObject.Find("Objects").transform, false);
        arrow.transform.position = this.transform.position;
        Vector3 dir = player.transform.position - arrow.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.GetComponent<ProjectileBehaviour>().enabled = true;
        anim.SetBool("Shoot", false);
        StartCoroutine(WaitShoot());
    }

    IEnumerator WaitShoot()
    {
        Debug.Log("Esperando");
        yield return new WaitForSeconds(2);
        Debug.Log("Esperado suficiente");
        shoot = true;
    }

}
