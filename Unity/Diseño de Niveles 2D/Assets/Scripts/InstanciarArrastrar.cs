using System.Collections;
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

    //Variables Guardado
    GameObject saver;

    // Start is called before the first frame update
    void Start()
    {
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
                if (GameObject.FindGameObjectWithTag("Player")== null)
                {
                    //Instanciamos, hacemos hijo del canvas y actualizamos datos para no poder controlarlo por teclado.
                    instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                    instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
                    instancia.transform.position = this.transform.position;
                    instancia.GetComponent<Rigidbody2D>().gravityScale = 0;
                    instancia.GetComponent<PlayerMovement>().enabled = false;
                    instancia.GetComponent<MoveObject>().enabled = true;
                    saver.GetComponent<SaveGround>().InsertGround(instancia);

                }
                else
                {
                    return;
                }
                break;

            //Para las puertas
            case "Puerta":
                //Activamos la ventana de estados
                
                //Instanciamos, hacemos hijo del canvas, creamos un id para el futuro y lo metemos en la lista de serialización.
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                int id = instancia.GetInstanceID();
                instancia.transform.parent = GameObject.Find("Objects").transform;
                instancia.transform.position = this.transform.position;
                instancia.GetComponent<ChangeScene>().id = id;
                saver.GetComponent<SaveGround>().InsertGround(instancia);

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
                instancia.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                break;
            case "Tree 1":
            case "Tree 2":
            case "Tree 3":
            case "Tree 4":
            case "Tree 5":
            case "Tree 6":
            case "Portal":
            case "Espinas":
                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
                instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
                instancia.transform.position = this.transform.position;
                saver.GetComponent<SaveGround>().InsertGround(instancia);
                if(instancia.GetComponent<ChangeScene>() != null) instancia.GetComponent<ChangeScene>().enabled = false;
                //if(instancia.GetComponent<Damage>() != null) instancia.GetComponent<Damage>().enabled = false;
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
        } else if (instancia.name.Contains("Puerta"))
        {
            GameObject.Find("StateController").GetComponent<StateMachine>().ShowDoorNameOption();
        } else if (instancia.name.Contains("Tree"))
        {
            instancia.transform.position = new Vector3(instancia.transform.position.x, instancia.transform.position.y, instancia.transform.position.z+1);
        }

        foreach (GameObject iman in listaColliders)
        {
            iman.GetComponentInChildren<ComportamientoIman>().enabled = false;
            iman.GetComponentInChildren<ComportamientoIman>().isMoving = false;
        }
        listaColliders.Clear();
        saver.GetComponent<SaveGround>().SaveNecessary();
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


}
