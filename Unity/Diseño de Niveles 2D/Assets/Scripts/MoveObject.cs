using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObject : MonoBehaviour {


    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 offsetSize;

    private string selection;
    private Vector3 last_mouse_pos;
    public bool rotar;
    public bool escalar;
    private bool isMoving;
    private List<GameObject> listaColliders;


    // Use this for initialization
    void Start () {
        selection = GameObject.Find("SelectionController").GetComponent<SelectionManager>().transformacion;
        isMoving = false;
        listaColliders = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            //AddDescendantsWithTag(child, tag, list);
        }
    }


    void OnMouseDown()
    {
        if (!enabled)
        {
            return;
        }
        else
        {
            AddDescendantsWithTag(this.transform, "iman", listaColliders);
        }
        if (this.tag == "Player")
        {
            this.GetComponent<SpriteOutline>().enabled = true;
        }
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        offsetSize =   transform.localScale - offsetSize;
        last_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        isMoving = true;
    }

    void OnMouseDrag()
    {
        if (!enabled)
        {
            return;
        }
        else
        {
            foreach(GameObject iman in listaColliders)
            {
                iman.GetComponentInChildren<ComportamientoIman>().enabled = false;
                StartCoroutine(WaitToActive(iman));
            }

        }
        switch (selection)
        {
            case "Move":
                /*Codigo de Traslación*/
                /*
                for(int i = 0; i < listaColliders.Length; i++)
                {
                    listaColliders[i].GetComponent<ComportamientoIman>().isMoving = true;
                }
                */

                foreach (GameObject iman in listaColliders)
                {
                    iman.GetComponentInChildren<ComportamientoIman>().isMoving = true;
                }
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                transform.position = curPosition;
                break;
            case "Rotate":
                if (rotar)
                {
                    isMoving = false;
                    /*Código de Rotación*/
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));

                    transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);
                }
                break;
            case "Scale":
                if (escalar)
                {
                    isMoving = false;
                    /*Código de Escalado*/
                    Vector3 curScreenPoint2 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                    //Se calcula la diferencia entre la posicion anterior y nueva del ratón y se aplica un pequeño offset para poder ajustar bien el tamaño.
                    Vector3 curSize = (curScreenPoint2 - last_mouse_pos) * 0.01f + new Vector3(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y, 0);
                    GetComponent<SpriteRenderer>().size = curSize;
                    last_mouse_pos = curScreenPoint2;
                }
                break;
            default:
                break;
        }
    }

    public void OnMouseUp()
    {
        foreach (GameObject iman in listaColliders)
        {
            iman.GetComponentInChildren<ComportamientoIman>().isMoving = false;
        }
        if (this.tag == "Player")
        {
            this.GetComponent<SpriteOutline>().enabled = false;
        }
    }

    public void SelectTransform(string tipo)
    {
        selection = tipo;
    }

    IEnumerator WaitToActive(GameObject iman)
    {
        yield return new WaitForSeconds(2);
        iman.GetComponentInChildren<ComportamientoIman>().enabled = true;
    }

}
