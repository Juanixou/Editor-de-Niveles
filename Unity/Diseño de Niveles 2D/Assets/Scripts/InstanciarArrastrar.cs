﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstanciarArrastrar : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    GameObject instancia;
    public GameObject ventanaEstados;
    public GameObject doorName;

    private Vector3 offsetSize;

    private Vector3 last_mouse_pos;
    private bool playerCreated;
    private List<GameObject> listaColliders;

    private int limitDoors;

    //Variables Guardado
    GameObject saver;

    // Start is called before the first frame update
    void Start()
    {
        limitDoors = 0;
        ventanaEstados = GameObject.Find("StateController");
        saver = GameObject.Find("DataController");
        listaColliders = new List<GameObject>();
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
            case "Character_1":
                //Si no se ha creado aún, lo instanciamos
                if (!playerCreated)
                {
                    //Instanciamos, hacemos hijo del canvas y actualizamos datos para no poder controlarlo por teclado.
                    instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                    instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
                    instancia.transform.position = this.transform.position;
                    instancia.GetComponent<Rigidbody2D>().gravityScale = 0;
                    instancia.GetComponent<PlayerMovement>().enabled = false;
                    instancia.GetComponent<MoveObject>().enabled = true;
                    saver.GetComponent<SaveGround>().InsertGround(instancia);
                    playerCreated = true;
                }
                break;

            //Para las puertas
            case "Puerta":
                limitDoors++;
                if (limitDoors <= 3)
                {
                    //Activamos la ventana de estados
                    int id = ventanaEstados.GetComponent<StateMachine>().ActivarEstados();
                    //Instanciamos, hacemos hijo del canvas, creamos un id para el futuro y lo metemos en la lista de serialización.
                    instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                    instancia.transform.parent = GameObject.Find("Objects").transform;
                    instancia.transform.position = this.transform.position;
                    instancia.GetComponent<ChangeScene>().id = id;
                    saver.GetComponent<SaveGround>().InsertGround(instancia);
                }
                break;

            //Para el suelo
            case "Suelo_1":
            case "Suelo_2":
            case "Suelo_3":
                //Instanciamos, hacemos hijo del canvas y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.parent = GameObject.Find("Objects").transform;
                instancia.transform.position = this.transform.position;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                AddDescendantsWithTag(instancia.transform, "iman", listaColliders);
                int count = 0;
                foreach (GameObject iman in listaColliders)
                {
                    count++;
                    iman.GetComponentInChildren<ComportamientoIman>().enabled = true;
                    iman.GetComponentInChildren<ComportamientoIman>().isMoving = true;
                }
                Debug.Log(count);
                break;

            case "Espinas":
                //Instanciamos, hacemos hijo del canvas y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
                instancia.transform.position = this.transform.position;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                break;

            case "Enemy 1":
            case "Enemy 2":
            case "Enemy 3":
            case "Enemy 4":
                //Instanciamos, hacemos hijo del canvas y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
                instancia.transform.position = this.transform.position;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                if (instancia.GetComponent<BasicEnemyMovement>() != null) instancia.GetComponent<BasicEnemyMovement>().enabled = false;
                if (instancia.GetComponent<MyRayCast>() != null) instancia.GetComponent<MyRayCast>().enabled = false;
                if (instancia.GetComponent<MyDistanceAttack>() != null) instancia.GetComponent<MyDistanceAttack>().enabled = false;
                //Desactivar Comportamiento
                instancia.GetComponent<BasicEnemyMovement>().enabled = false;
                break;

            default:
                break;

        }
        //Se calcula la posición del ratón para su desplazamiento por la escena.
        screenPoint = Camera.main.WorldToScreenPoint(instancia.gameObject.transform.position);
        offset = instancia.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        offsetSize = instancia.transform.localScale - offsetSize;
        last_mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Camera.main.GetComponent<MoveCamera>().AddInstances(instancia);
    }

    void OnMouseDrag()
    {
        /*Codigo de Traslación*/
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        instancia.transform.position = curPosition;
    }

    void OnMouseUp()
    {
        if (instancia.name.Contains("Suelo"))
        {
            SpriteRenderer obj = instancia.transform.Find("Marco").GetComponent<SpriteRenderer>();
            //GetComponent<MoveObject>().enabled = true;

            if (obj != null)
            {
                obj.enabled = false;
            }
        }else if (instancia.name.Contains("Puerta"))
        {
            ManageDoorName();
        }

        foreach (GameObject iman in listaColliders)
        {
            iman.GetComponentInChildren<ComportamientoIman>().enabled = false;
            iman.GetComponentInChildren<ComportamientoIman>().isMoving = false;
        }
        listaColliders.Clear();
    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
        }
    }

    private void ManageDoorName()
    {
        doorName = GameObject.Find("StateController").GetComponent<StateMachine>().ShowDoorNameOption();

        Transform doorNameText = null;
        Transform doorNameBtn = null;

        for(int i = 0; i < doorName.transform.childCount; ++i)
        {
            switch (doorName.transform.GetChild(i).name)
            {
                case "DoorInputField":
                    doorNameText = doorName.transform.GetChild(i);
                    break;
                case "SaveDoorButton":
                    doorNameBtn = doorName.transform.GetChild(i);
                    break;
            }
        }
        InputField input = doorNameText.GetComponent<InputField>();
        input.onValueChanged.AddListener(delegate { ValueChange(doorNameBtn,input); });
        doorNameBtn.GetComponent<Button>().onClick.AddListener(delegate { SaveDoorName(input.text,doorName); });
        input.Select();
        input.ActivateInputField();
    }

    public void ValueChange(Transform doorBtn, InputField input)
    {
        if(input.text != "")
        {
            doorBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            doorBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void SaveDoorName(string name, GameObject doorMenu)
    {
        saver.GetComponent<SaveGround>().InsertDoor(instancia,name);
        doorMenu.SetActive(false);
    }

}
