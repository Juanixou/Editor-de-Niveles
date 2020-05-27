using System;
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
    private bool pushed = false;
    public string dataPath = "";

    public GameObject dataController;
    void Start () {
        escena = SceneManager.GetActiveScene();
        try
        {
            tecla = this.gameObject.transform.GetChild(0).gameObject;
            dataController = GameObject.Find("DataController");
        }catch(Exception e)
        {

        }


    }
	
	// Update is called once per frame
	void Update () {
        if (colision)
        {
            if(tecla!=null)tecla.SetActive(true);
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
            if (this.name.Contains("Portal")) EndLevel();
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
            pushed = false;
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

    public void Instrucciones()
    {
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
    }

    public void LoadScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        string newPath = "";
        string curPath = "";
        int nextId = -1;
        Vector3 playerPos = new Vector3(0,0,0);
        DoorData[] puertas;
        //Leemos el JSON de las puertas de ESTE nivel
        using (StreamReader streamReader = File.OpenText(dataPath + "\\Puertas.txt"))
        {
            string jsonString = streamReader.ReadToEnd();

            puertas = JsonHelper.FromJson<DoorData>(jsonString);
        }
        //Recogemos los datos de la puerta actual
        for (int i = 0; i < puertas.Length; i++)
        {
            if (puertas[i].id == id)
            {
                newPath = puertas[i].postScene;
                curPath = puertas[i].curScene;
                nextId = puertas[i].postId;
            }
        }

        //Movemos al player dentro del mismo nivel
        if (curPath.Equals(newPath))
        {
            for(int i = 0; i < puertas.Length; i++)
            {
                if(puertas[i].id == nextId)
                {
                    BlockDoorPush(puertas[i].position);
                    player.transform.position = puertas[i].position;
                    
                }
            }
            return;            
        }

        if (nextId != -1)
        {
            GameObject instancia;
            //Eliminamos todos los objetos creados en la escena para cargar un nuevo nivel
            dataPath = newPath;
            

            foreach (Transform child in GameObject.Find("Objects").transform){

                if (child.name.Contains("(Clone)") && !child.CompareTag("Player"))
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
                            instancia.transform.parent = GameObject.Find("Objects").transform;
                            instancia.transform.localPosition = suelos[i].position;
                            instancia.transform.rotation = Quaternion.Euler(suelos[i].rotation);
                            instancia.GetComponent<SpriteRenderer>().size = suelos[i].spriteSize;
                            instancia.GetComponent<BoxCollider2D>().size = suelos[i].boxColliderSize;
                            instancia.transform.Find("ImanDcho").transform.position = suelos[i].imanDchoPos;
                            instancia.transform.Find("ImanDcho").transform.GetComponent<BoxCollider2D>().size = suelos[i].imanDchoBoxColliderSize;
                            instancia.transform.Find("ImanIzqd").transform.position = suelos[i].imanIzqPos;
                            instancia.transform.Find("ImanIzqd").transform.GetComponent<BoxCollider2D>().size = suelos[i].imanIzqBoxColliderSize;
                            instancia.transform.Find("Sides").GetComponent<BoxCollider2D>().size = suelos[i].sidesSize;
                            if (instancia.transform.Find("Marco").GetComponent<SpriteRenderer>() != null) instancia.transform.Find("Marco").GetComponent<SpriteRenderer>().enabled = false;
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
                            instancia.transform.parent = GameObject.Find("Objects").transform;
                            instancia.transform.localPosition = puertas[i].position;
                            instancia.transform.rotation = Quaternion.Euler(puertas[i].rotation);
                            if (nextId == puertas[i].id)
                            {
                                instancia.GetComponent<ChangeScene>().pushed = true;
                              
                                playerPos = instancia.transform.position;
                            }
                            instancia.GetComponent<SpriteRenderer>().size = puertas[i].spriteSize;
                            instancia.GetComponent<BoxCollider2D>().size = puertas[i].boxColliderSize;

                            instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                            instancia.GetComponent<ChangeScene>().SetDataPath(dataPath);
                        }
                        break;

                case "Enemies.txt":
                    //Generamos un array de suelos que contendrá todos los suelos del nivel
                    EnemiesData[] enemies;
                    //Leemos el JSON
                    using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                    {
                        string jsonString = streamReader.ReadToEnd();

                        enemies = JsonHelper.FromJson<EnemiesData>(jsonString);
                    }
                    //Una vez leido el fichero, se instancian todos los suelos en la escena
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        instancia = Instantiate((GameObject)Resources.Load("prefabs/" + enemies[i].enemyName, typeof(GameObject)));
                        instancia.transform.parent = GameObject.Find("Objects").transform;

                        instancia.GetComponent<MoveObject>().enabled = false;
                        if (instancia.GetComponent<BasicEnemyMovement>() != null) instancia.GetComponent<BasicEnemyMovement>().enabled = true;
                        if (instancia.GetComponent<MyRayCast>() != null) instancia.GetComponent<MyRayCast>().enabled = true;
                        if (instancia.GetComponent<MyDistanceAttack>() != null) instancia.GetComponent<MyDistanceAttack>().enabled = true;
                        instancia.transform.localPosition = enemies[i].position;

                    }
                    break;

                    case "Objects.txt":
                        //Generamos un array de suelos que contendrá todos los suelos del nivel
                        ObjectsData[] objects;
                        //Leemos el JSON
                        using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                        {
                            string jsonString = streamReader.ReadToEnd();

                            objects = JsonHelper.FromJson<ObjectsData>(jsonString);
                        }
                        //Una vez leido el fichero, se instancian todos los suelos en la escena
                        for (int i = 0; i < objects.Length; i++)
                        {
                            instancia = Instantiate((GameObject)Resources.Load("prefabs/" + objects[i].objectName, typeof(GameObject)));
                            instancia.transform.parent = GameObject.Find("Objects").transform;
                            instancia.transform.localPosition = objects[i].position;
                            if (instancia.GetComponent<MoveObject>() != null) instancia.GetComponent<MoveObject>().enabled = false;
                            if (instancia.GetComponent<ChangeScene>() != null) instancia.GetComponent<ChangeScene>().enabled = true;
                            if (instancia.GetComponent<Animator>() != null) instancia.GetComponent<Animator>().enabled = true;
                            if (instancia.GetComponent<Damage>() != null) instancia.GetComponent<Damage>().enabled = true;
                            if (instancia.CompareTag("Background"))
                            {
                                UpdateBackgroundTile(Camera.main.transform.position.x, 0.0f, instancia);
                            }
                            if (!instancia.name.Contains("Backgr"))
                            {
                                instancia.transform.rotation = Quaternion.Euler(objects[i].rotation);
                                instancia.GetComponent<SpriteRenderer>().size = objects[i].spriteSize;
                                instancia.GetComponent<BoxCollider2D>().size = objects[i].boxColliderSize;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            //instancia = Instantiate((GameObject)Resources.Load("prefabs/Character_1", typeof(GameObject)));
            //Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().target = instancia.transform;
            //instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
            player.transform.position = playerPos;
            //instancia.GetComponent<MoveObject>().enabled = false;
        }

        if(dataController.GetComponent<SaveGround>()!=null)dataController.GetComponent<SaveGround>().GuardarRuta(newPath);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void EndLevel()
    {
        if (GameObject.Find("SelectionController") != null)
        {
            GameObject.Find("SelectionController").GetComponent<SelectionManager>().Editar();
        }
        else
        {
            SceneManager.LoadScene("EndLevel", LoadSceneMode.Single);
        }
        
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2);
        pushed = false;
    }

    IEnumerator WaitForEndLevel()
    {
        yield return new WaitForSeconds(0.5f);
        
    }

    void UpdateBackgroundTile(float newPosX, float lastPosX, GameObject bckgrd)
    {

        float xScale = (newPosX - lastPosX);

        if (bckgrd == null) return;
        foreach (SpriteRenderer child in bckgrd.GetComponentsInChildren<SpriteRenderer>())
        {
            child.size = new Vector2(child.size.x + xScale, child.size.y);
        }

    }

    public void SetDataPath(string path) { dataPath = path; }

    public void BlockDoorPush(Vector3 position)
    {
        foreach(GameObject door in GameObject.FindGameObjectsWithTag("Door"))
        {
            if(((int)door.transform.position.x == (int)position.x) && ((int)door.transform.position.y == (int)position.y))
            {
                Debug.Log("Cogida!");
                door.GetComponent<ChangeScene>().pushed = true;
            }
        }
    }

}
