﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoIman : MonoBehaviour
{

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("COLISION");
        if ((isMoving) && (other.gameObject.tag == "iman"))
        {
            Debug.Log("Dentro del collider!");
            if (this.transform.position.x <= other.transform.position.x)
            {
                Debug.Log("Dentro por la izquierda!");
                this.transform.position = new Vector2(other.transform.position.x - this.GetComponent<SpriteRenderer>().size.x, this.transform.position.y);
            }
            else
            {
                Debug.Log("Dentro por la izquierda!");
                this.transform.position = new Vector2(other.transform.position.x + other.gameObject.GetComponent<SpriteRenderer>().size.x, this.transform.position.y);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Transform padreOtro = other.GetComponentInParent<Transform>();
        Transform padreMio = GetComponentInParent<Transform>();
        Debug.Log("COLISION");
        if ((isMoving) && (other.gameObject.tag == "iman"))
        {
            Debug.Log("Dentro del collider!");
            if (this.transform.position.x <= other.transform.position.x)
            {
                Debug.Log("Dentro por la izquierda!");

                padreMio.position = new Vector2(padreOtro.position.x - this.GetComponentInParent<SpriteRenderer>().size.x, padreMio.position.y);
            }
            else
            {
                Debug.Log("Dentro por la izquierda!");
                padreMio.position = new Vector2(padreOtro.position.x + other.GetComponentInParent<SpriteRenderer>().size.x, padreMio.position.y);
            }
        }
    }
}
