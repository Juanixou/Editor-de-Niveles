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

        if ((isMoving) && (other.gameObject.tag == "iman"))
        {
            padreMio.GetComponent<MoveObject>().enabled = false;
            if (this.transform.position.x <= other.transform.position.x)
            {
                Debug.Log("Iman por la izquierda");
                //IMAN DERECHO EN MOVIMIENTO SE PEGA AL IZQUIERDO FIJO
                float y = 0;
                float x = 0;//padreOtro.transform.position.x - padreMio.GetComponent<SpriteRenderer>().size.x;
                if (padreOtro.transform.rotation.z != 0)
                {
                    Debug.Log("Con rotacion");
                    Debug.Log("PADRE MIO POS: " + padreMio.transform.position);

                    float angGrad = 180 + padreOtro.transform.eulerAngles.z;
                    Quaternion angQua = Quaternion.Euler(0, 0, angGrad);
                    padreMio.transform.position = new Vector2(padreOtro.transform.position.x, padreOtro.transform.position.y);
                    padreMio.transform.position = new Vector2(padreMio.transform.position.x - (Mathf.Abs(Mathf.Cos(angGrad)) * (padreOtro.GetComponent<BoxCollider2D>().size.x)), padreOtro.transform.position.y);
                    padreMio.transform.position = new Vector2(padreMio.transform.position.x - padreOtro.GetComponent<SpriteRenderer>().size.x/2,padreOtro.transform.position.y);
                    padreMio.transform.position = new Vector2(padreMio.transform.position.x,padreOtro.transform.position.y + Mathf.Abs(Mathf.Sin(angGrad))* (padreOtro.GetComponent<BoxCollider2D>().size.x));

                }
                else
                {
                    Debug.Log("Sin rotacion");
                    x = padreOtro.transform.position.x - padreMio.GetComponent<SpriteRenderer>().size.x;
                    y = padreOtro.transform.position.y;
                    padreMio.transform.position = new Vector2(x, y);
                }
                StartCoroutine(Example(padreMio));

            }
            else
            {
                Debug.Log("Iman por la derecha");
                //IMAN IZQUIERDO EN MOVIMIENTO SE PEGA AL DERECHO FIJO

                float x = padreOtro.transform.position.x + padreOtro.GetComponent<SpriteRenderer>().size.x;
                float y;
                if (padreOtro.transform.rotation.z != 0)
                {
                    y = padreMio.transform.position.y;
                }
                else
                {
                    y = padreOtro.transform.position.y;
                }
                Vector2 pos = transform.TransformPoint(other.transform.position);
                padreMio.transform.localPosition = new Vector2(x, padreOtro.transform.position.y);

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
