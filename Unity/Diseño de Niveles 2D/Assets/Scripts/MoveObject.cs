using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {


    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 offsetSize;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        offsetSize =   transform.localScale - offsetSize;
    }

    void OnMouseDrag()
    {
        /*Codigo de Traslación
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        */

        /*Código de Rotación
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));


        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);
        */
        /*
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        float startX = curPosition.x;
        float startY = curPosition.y;
        float startSize = transform.localScale.z;
        Vector3 size = transform.localScale;
        size.x =  (Input.mousePosition.x - startX) * 0.02f;
        size.y =  (Input.mousePosition.y - startY) * 0.02f;

        transform.localScale = size;
        */

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curSize = Camera.main.ScreenToWorldPoint(curScreenPoint) + offsetSize;
        transform.localScale = curSize;
    }
}
