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
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dcha)
        {
            this.transform.position= new Vector2(this.transform.position.x + 0.01f, this.transform.position.y);
            actualDistance+=0.1f;
            if(actualDistance >= maxDistance)
            {
                personaje.transform.Rotate(new Vector2(0, 180));
                dcha = false;
            }
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x - 0.01f, this.transform.position.y);
            actualDistance -= 0.1f ;
            if (actualDistance <= minDistance)
            {
                personaje.transform.Rotate(new Vector2(0, 180));
                dcha = true;
            }
        }
    }
}
