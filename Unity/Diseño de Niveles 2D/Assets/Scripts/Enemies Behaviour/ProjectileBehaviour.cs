using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float arrowSpeed = 10.0f;
    private Vector2 target;
    private Vector2 position;
    public float yOffset = 0.5f;
    private bool move;
    private float distanceControl;
    public float damage = 20;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        distanceControl = 0;
        move = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target.Set(playerTransform.position.x, playerTransform.position.y + yOffset);
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
            target = position* 2;
            if(distanceControl >= 4)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy")) return;
        if (other.tag.Equals("MeleeWeapon")) return;
        if (other.CompareTag("Objects")) return;
        if (other.CompareTag("Arrow")) return;
        move = false;
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerStats>().Damage(damage);
                Destroy(this.gameObject);
                break;
            case "Ground":
                StartCoroutine(WaitForDisapear());
                break;
            default:
                StartCoroutine(WaitForDisapear());
                break;
        }

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy")) return;
        if (other.CompareTag("Objects")) return;
        if (other.CompareTag("Arrow")) return;
        move = false;
        switch (other.tag)
        {
            case "Player":
                Destroy(this.gameObject);
                break;
            case "Ground":
                StartCoroutine(WaitForDisapear());
                break;
            default:
                StartCoroutine(WaitForDisapear());
                break;
        }
    }

    IEnumerator WaitForDisapear()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

}
