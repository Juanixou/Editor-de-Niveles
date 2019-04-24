﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarArrastrar : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    GameObject instancia;
    public GameObject ventanaEstados;

    private Vector3 offsetSize;

    private Vector3 last_mouse_pos;
    private bool playerCreated;

    //Variables Guardado
    GameObject saver;

    // Start is called before the first frame update
    void Start()
    {
        ventanaEstados = GameObject.Find("StateController");
        saver = GameObject.Find("DataController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        switch (this.name)
        {
            //Para el jugador
            case "Player":
                //Si no se ha creado aún, lo instanciamos
                if (!playerCreated)
                {
                    //Instanciamos, hacemos hijo del canvas y actualizamos datos para no poder controlarlo por teclado.
                    instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                    instancia.transform.parent = GameObject.Find("Canvas").transform;
                    instancia.transform.position = this.transform.position;
                    instancia.GetComponent<Rigidbody2D>().gravityScale = 0;
                    instancia.GetComponent<PlayerMovement>().enabled = false;
                    instancia.GetComponent<MoveObject>().enabled = true;
                    playerCreated = true;
                }
                break;

            //Para las puertas
            case "Puerta":
                //Activamos la ventana de estados
                int id = ventanaEstados.GetComponent<StateMachine>().ActivarEstados();
                //Instanciamos, hacemos hijo del canvas, creamos un id para el futuro y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.parent = GameObject.Find("Canvas").transform;
                instancia.transform.position = this.transform.position;
                instancia.GetComponent<ChangeScene>().id = id;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                saver.GetComponent<SaveGround>().InsertDoor(instancia);
                break;

            //Para el suelo
            case "Suelo_Tile":
                //Instanciamos, hacemos hijo del canvas y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.parent = GameObject.Find("Canvas").transform;
                instancia.transform.position = this.transform.position;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                break;

            default:
                break;

        }
        //Se calcula la posición del ratón para su desplazamiento por la escena.
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
