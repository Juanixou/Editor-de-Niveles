using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoIman : MonoBehaviour
{

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject padreMio = this.transform.parent.gameObject;
        GameObject padreOtro = other.transform.parent.gameObject;
        //Transform padreOtro = other.GetComponentInParent<Transform>();
        //Transform padreMio = GetComponentInParent<Transform>();
        if ((isMoving) && (other.gameObject.tag == "iman"))
        {
            padreMio.GetComponent<MoveObject>().enabled = false;
            if (this.transform.position.x <= other.transform.position.x)
            {

                padreMio.transform.position = new Vector2(padreOtro.transform.position.x - padreMio.GetComponent<SpriteRenderer>().size.x, padreOtro.transform.position.y);

                StartCoroutine(Example(padreMio));

            }
            else
            {
                padreMio.transform.position = new Vector2(padreOtro.transform.position.x + padreOtro.GetComponent<SpriteRenderer>().size.x, padreOtro.transform.position.y);
                StartCoroutine(Example(padreMio));
            }
        }
    }

    IEnumerator Example(GameObject padreMio)
    {
        yield return new WaitForSeconds(2);
        padreMio.GetComponent<MoveObject>().enabled = true;
    }
}
