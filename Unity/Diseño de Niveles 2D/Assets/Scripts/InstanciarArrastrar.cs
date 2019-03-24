using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarArrastrar : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    GameObject instancia;

    private Vector3 offsetSize;

    private Vector3 last_mouse_pos;
    private bool playerCreated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(this.name == "Player")
        {
            if (!playerCreated)
            {
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.parent = GameObject.Find("Canvas").transform;
                instancia.transform.position = this.transform.position;
                instancia.GetComponent<Rigidbody2D>().gravityScale = 0;
                instancia.GetComponent<PlayerMovement>().enabled = false;
                instancia.GetComponent<MoveObject>().enabled = true;
                playerCreated = true;
            }
        }
        else
        {
            instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
            instancia.transform.parent = GameObject.Find("Canvas").transform;
            instancia.transform.position = this.transform.position;
        }

        screenPoint = Camera.main.WorldToScreenPoint(instancia.gameObject.transform.position);

        offset = instancia.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        offsetSize = instancia.transform.localScale - offsetSize;
        last_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    }

    void OnMouseDrag()
    {
                /*Codigo de Traslación*/
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        instancia.transform.position = curPosition;
    }
}
