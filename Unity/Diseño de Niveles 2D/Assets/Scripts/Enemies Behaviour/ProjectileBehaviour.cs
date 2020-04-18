using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float arrowSpeed = 10.0f;
    private Vector2 target;
    private Vector2 position;
    private bool move;
    private float distanceControl;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        distanceControl = 0;
        move = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target = playerTransform.position;
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = arrowSpeed * Time.deltaTime;

        // move sprite towards the target location
        if (move)
        {
            position = transform.position = Vector2.MoveTowards(transform.position, target, step);
        }

        if(position == target)
        {
            distanceControl++;
            target *= 2;
            if(distanceControl >= 4)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy")) return;
        move = false;
        Debug.Log("Flecha impacta");
        if (other.tag == "Player")
        {
            Debug.Log("Colision Player");
            Destroy(this.gameObject);
        }else{
            Debug.Log("Colision Otro");
            StartCoroutine(WaitForDisapear());
        }

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy")) return;
        move = false;
        Debug.Log("Flecha impacta");
        if (other.tag == "Player")
        {
            Debug.Log("Colision Player");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Colision Otro");
            StartCoroutine(WaitForDisapear());
        }
    }

    IEnumerator WaitForDisapear()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

}
