using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{

    public float minDistance = 50.0f;
    public float maxDistance = 100.0f;
    private float actualDistance;
    private bool dcha;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        dcha = true;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Walking", true);
        actualDistance = minDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(dcha)
        {
            this.transform.Translate(new Vector2(1.0f, this.transform.position.y));
            actualDistance++;
            if(actualDistance >= maxDistance)
            {
                dcha = false;
            }
        }
        else
        {
            this.transform.Translate(new Vector2(-1.0f, this.transform.position.y));
            actualDistance--;
            if (actualDistance <= minDistance)
            {
                dcha = true;
            }
        }
    }
}
