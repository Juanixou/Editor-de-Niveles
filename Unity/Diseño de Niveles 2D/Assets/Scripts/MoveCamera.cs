using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    private float speed;
    private GameObject instancias;
    private GameObject transformaciones;
    private List<GameObject> elements;
    // Use this for initialization
    void Start()
    {
        speed = 2.0f;
        instancias = GameObject.Find("Instancias");
        transformaciones = GameObject.Find("Transformaciones");
        elements = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)&&transform.position.x<=50)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            MoveInstances("dch");
            //instancias.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            //transformaciones.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow)&&transform.position.x>=0)
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            MoveInstances("izq");
            //instancias.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            //transformaciones.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        /************** CONTROL VERTICAL **************
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        */
    }

    public void AddInstances(GameObject elem)
    {
        elements.Add(elem);
    }

    public void MoveInstances(string mov)
    {
        if(mov == "dch")
        {
            foreach(GameObject elem in elements)
            {
                elem.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            foreach (GameObject elem in elements)
            {
                elem.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
    }

}
