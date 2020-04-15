using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private SpriteRenderer outln;
    public Vector2 factor = new Vector2(1.0f,1.0f);
    private Vector3 scaleOrig;


    // Use this for initialization
    void Start () {
        try
        {
            selection = GameObject.Find("SelectionController").GetComponent<SelectionManager>().transformacion;
        }
        catch(NullReferenceException e)
        {

        }
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
            else if (this.tag=="Ground")
            {
                outln = child.gameObject.GetComponent<SpriteRenderer>();
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
            //outln.enabled = true;
            //this.GetComponent<SpriteOutline>().enabled = true;
            Debug.Log("AÑADIR SHADER PARA SELECCION DE PERSONAJE SPRITE OUTLINE");
        }
        
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        offsetSize =   transform.localScale - offsetSize;
        last_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        isMoving = true;
        if (outln != null)
        {
            outln.enabled = true;
        }
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

                    /*Cambiar Imagen y luego ajustar colliders*/
                    Vector3 newCurSize = (curScreenPoint2 - last_mouse_pos);
                    scaleOrig += newCurSize*0.03f;
                    Vector3 imgSize = GetComponent<SpriteRenderer>().size;
                    GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size = imgSize + newCurSize*0.03f;

                    last_mouse_pos = curScreenPoint2;
                }
                break;
            default:
                break;
        }

        if (Input.GetKey(KeyCode.Delete))
        {
            Destroy(this.gameObject);
            return;
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
        else if(selection != "Move" && this.tag == "Ground")
        {
            GameObject imanDcho = transform.Find("ImanDcho").gameObject;
            GameObject imanIzq = transform.Find("ImanIzqd").gameObject;

            imanDcho.transform.position = new Vector3(imanDcho.transform.position.x + scaleOrig.x / 2, imanDcho.transform.position.y,0.0f);
            imanIzq.transform.position = new Vector3(imanIzq.transform.position.x - scaleOrig.x / 2, imanIzq.transform.position.y, 0.0f);
            imanDcho.GetComponent<BoxCollider2D>().size = new Vector2(imanDcho.GetComponent<BoxCollider2D>().size.x, imanDcho.GetComponent<BoxCollider2D>().size.y + scaleOrig.y);
            imanIzq.GetComponent<BoxCollider2D>().size = new Vector2(imanIzq.GetComponent<BoxCollider2D>().size.x, imanIzq.GetComponent<BoxCollider2D>().size.y + scaleOrig.y);
            scaleOrig = Vector3.zero;
        }
        outln.enabled = false;
    }

    public void SelectTransform(string tipo)
    {
        selection = tipo;
    }

    public void SetTransform(Vector2 pos)
    {
        this.transform.localPosition = pos;
    }

    public void TrasladarIman()
    {
        //Mover imanes al centro y después de rotación trasladar a los extremos
    }

    IEnumerator WaitToActive(GameObject iman)
    {
        yield return new WaitForSeconds(2);
        iman.GetComponentInChildren<ComportamientoIman>().enabled = true;
    }

}
