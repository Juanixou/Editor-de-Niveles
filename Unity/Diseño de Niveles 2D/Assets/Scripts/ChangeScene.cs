using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private Scene escena;
    public GameObject tecla;
    public GameObject player;
    public int id;
    private bool colision;
    private bool pushed=false;

    public GameObject dataController;
    void Start () {
        escena = SceneManager.GetActiveScene();
        if(this.gameObject.transform.GetChild(0).gameObject!=null)
        tecla = this.gameObject.transform.GetChild(0).gameObject;
        dataController = GameObject.Find("DataController");
	}
	
	// Update is called once per frame
	void Update () {
        if (colision)
        {
            tecla.SetActive(true);
        }
        else if(tecla!=null)
        {
            tecla.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.tag == "Player"))
        {
            colision = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Player"))
        {
            if (Input.GetKey(KeyCode.E)&&!pushed)
            {
                pushed = true;
                CambiarEscena();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "Player"))
        {
            colision = false;
        }
    }

    public void CambiarEscena()
    {
        tecla.SetActive(false);
        switch (escena.name)
        {
            case "City":
                SceneManager.LoadScene("Jump Trial", LoadSceneMode.Single);
                break;
            case "Jump Trial":
                SceneManager.LoadScene("Wall Jump Trial", LoadSceneMode.Single);
                break;
            case "Wall Jump Trial":
                SceneManager.LoadScene("Push Objects", LoadSceneMode.Single);
                break;
            case "Push Objects":
                SceneManager.LoadScene("Damage Trial", LoadSceneMode.Single);
                break;
            case "Damage Trial":
                SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
                break;
            default:
                LoadScene();
                break;
        }
    }

    public void SeleccionarInicio()
    {
        SceneManager.LoadScene("Inicio", LoadSceneMode.Single);
    }

    public void Mecanicas()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    public void Editor()
    {
        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }

    public void SeleccionEditor()
    {
        SceneManager.LoadScene("Crear-Cargar", LoadSceneMode.Single);
    }

    public void LoadScene()
    {
        Debug.Log("Cargando Escena");
        string newPath = "";
        int nextId = -1;
        Vector3 playerPos = new Vector3(0,0,0);
        DoorData[] puertas;
        string dataPath = dataController.GetComponent<SaveGround>().dataPath;
        //Leemos el JSON
        using (StreamReader streamReader = File.OpenText(dataPath + "\\Puertas.txt"))
        {
            string jsonString = streamReader.ReadToEnd();

            puertas = JsonHelper.FromJson<DoorData>(jsonString);
        }
        //Una vez leido el fichero, se instancian todas las puertas en la escena
        for (int i = 0; i < puertas.Length; i++)
        {
            if (puertas[i].id == id)
            {
                newPath = puertas[i].postScene;
                nextId = puertas[i].postId;
            }
        }
        if (nextId != -1)
        {
            GameObject instancia;
            //Eliminamos todos los objetos creados en la escena para cargar un nuevo nivel
            dataPath = newPath;
            

            foreach (Transform child in GameObject.Find("Canvas").transform)
                {
                    if (child.name.Contains("(Clone)"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                //Obtenemos todos los ficheros de almacenamiento del nivel
                DirectoryInfo dir = new DirectoryInfo(dataPath);
                FileInfo[] info = dir.GetFiles("*.*");

                //Uno por uno, cargamos e instanciamos los objetos correspondientes
                foreach(FileInfo file in info)
                {
                    switch (file.Name)
                    {
                        //Para los suelos
                        case "Grounds.txt":
                            //Generamos un array de suelos que contendrá todos los suelos del nivel
                            GroundData[] suelos;
                            //Leemos el JSON
                            using (StreamReader streamReader = File.OpenText(dataPath + "\\"+file.Name))
                            {
                                string jsonString = streamReader.ReadToEnd();

                                suelos = JsonHelper.FromJson<GroundData>(jsonString);
                            }
                            //Una vez leido el fichero, se instancian todos los suelos en la escena
                            for (int i = 0; i < suelos.Length; i++)
                            {
                                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + suelos[i].groundName, typeof(GameObject)));
                                instancia.transform.parent = GameObject.Find("Canvas").transform;
                                instancia.transform.position = suelos[i].position;
                            }
                            break;
                        //Para las puertas
                        case "Puertas.txt":
                            //Generamos un array de puertas que contendrá todas las puertas del nivel
                            //Leemos el JSON
                            using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                            {
                                string jsonString = streamReader.ReadToEnd();

                                puertas = JsonHelper.FromJson<DoorData>(jsonString);
                            }
                            //Una vez leido el fichero, se instancian todas las puertas en la escena
                            for (int i = 0; i < puertas.Length; i++)
                            {
                                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + puertas[i].doorName, typeof(GameObject)));
                                instancia.transform.parent = GameObject.Find("Canvas").transform;
                                playerPos = instancia.transform.position = puertas[i].position;
                                instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                            }
                            break;


                        default:
                            break;
                    }
                }
            instancia = Instantiate((GameObject)Resources.Load("prefabs/Character_1", typeof(GameObject)));
            instancia.transform.SetParent(GameObject.Find("Canvas").transform, false);
            instancia.transform.position = playerPos;
            instancia.GetComponent<MoveObject>().enabled = false;
            
        }

        dataController.GetComponent<SaveGround>().ActualizarRuta(newPath);
        Debug.Log("Fin");
    }
}
