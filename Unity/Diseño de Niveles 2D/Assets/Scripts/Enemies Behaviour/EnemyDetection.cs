using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private CircleCollider2D area;
    private bool detection;
    private Vector3 playerPos;
    const string PLAYER = "Player";
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<CircleCollider2D>();
        playerPos = transform.position;
    }

    private void Update()
    {
        if (detection)
        {
            LookForPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == PLAYER)
        {
            playerPos = other.transform.position;
            detection = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.tag == PLAYER)
        {
            playerPos = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == PLAYER)
        {
            Debug.Log("Estoy fuera");
            detection = false;
            playerPos = transform.position;
        }
    }

    private void LookForPlayer()
    {
        
        
    }


}
