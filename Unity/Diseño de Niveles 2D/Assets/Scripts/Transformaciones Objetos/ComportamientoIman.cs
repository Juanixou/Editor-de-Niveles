using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoIman : MonoBehaviour
{

    public bool isMoving;
    private const float ESCALA = 128.0f;

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
        GameObject padreOtro = other.gameObject.transform.parent.gameObject;

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(padreMio.gameObject.transform.position);
        Vector3 offset = padreMio.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (padreMio.transform.eulerAngles.z != 0 || Input.GetKey(KeyCode.LeftShift)) return;
        if ((isMoving) && (other.gameObject.tag == "iman"))
        {
            padreMio.GetComponent<MoveObject>().enabled = false;
            if (padreMio.transform.position.x <= padreOtro.transform.position.x)
            {
                //IMAN DERECHO EN MOVIMIENTO SE PEGA AL IZQUIERDO FIJO
                if (padreOtro.transform.eulerAngles.z != 0)
                {
                    padreMio.transform.position = new Vector3(other.transform.position.x - (padreMio.GetComponent<BoxCollider2D>().size.x / 2), other.transform.position.y, curPosition.z);
                }
                else
                {
                    padreMio.transform.position = new Vector3((padreOtro.transform.position.x - padreMio.GetComponent<SpriteRenderer>().size.x), padreOtro.transform.position.y, curPosition.z);

                }
                StartCoroutine(Example(padreMio));

            }
            else
            {
                Debug.Log("Iman por la derecha");
                //IMAN IZQUIERDO EN MOVIMIENTO SE PEGA AL DERECHO FIJO
                if (padreOtro.transform.eulerAngles.z != 0)
                {
                    padreMio.transform.position = new Vector3(other.transform.position.x + (padreMio.GetComponent<BoxCollider2D>().size.x / 2), other.transform.position.y, curPosition.z);
                }
                else
                {
                    padreMio.transform.position = new Vector3((padreOtro.transform.position.x + padreMio.GetComponent<SpriteRenderer>().size.x), padreOtro.transform.position.y, curPosition.z);

                }
                SpriteRenderer obj = padreMio.transform.Find("Marco").GetComponent<SpriteRenderer>();
                if (obj != null)
                {
                    obj.enabled = false;
                }
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
