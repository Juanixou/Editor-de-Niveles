using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;
using SFB;
using System.Windows.Forms;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class SaveGround : MonoBehaviour
{
    public bool loaded;
    public List<GameObject> listaObjetos;
    [SerializeField]
    public List<GroundData> listaSuelos;
    public List<DoorData> listaPuertas;
    public List<EnemiesData> listaEnemigos;
    public List<PlayerData> listaPlayers;
    public List<ObjectsData> listaObjects;
    public string dataPath;
    public GameObject stateMachine;
    public string tempPath;
    private bool saved = true;
    public GameObject doorName;

    public bool canEnterText = false;

    InputField input;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = "";
        loaded = false;
        listaObjetos = new List<GameObject>();
        listaSuelos = new List<GroundData>();
        listaEnemigos = new List<EnemiesData>();
        listaPlayers = new List<PlayerData>();
        listaObjects = new List<ObjectsData>();
        stateMachine = GameObject.Find("StateController");
        InsertGround(GameObject.FindGameObjectWithTag("Background"));
        ManageDoorName();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !canEnterText)
        {
            if (CheckExit())
            {
                SceneManager.LoadScene("Inicio", LoadSceneMode.Single);
            }
        }

        if(canEnterText)
        {
            if (Input.GetKey(KeyCode.Return) && input.text != "")
            {
                SaveDoorName();
            }

            if (Input.GetMouseButtonDown(0))
            {
                input.Select();
            }

        }
    }

    public void InsertGround(GameObject suelo)
    {
        //Añadimos a la lista los elementos instanciados
        listaObjetos.Add(suelo);
    }

    public void InsertDoor(GameObject puerta, string name)
    {
        listaPuertas.Add(new DoorData(puerta.name, puerta.transform, puerta.GetComponent<ChangeScene>().id,name, dataPath));
    }

    public void FillObjectsList()
    {
        listaObjetos.Clear();
        GameObject objects = GameObject.Find("Objects");
        for (int i = 0; i< objects.transform.childCount; i++)
        {
            listaObjetos.Add(objects.transform.GetChild(i).gameObject);
        }

    }

    public void SaveData()
    {
        int contadorPuertas = 0;
        //Comprobamos si hay ruta establecida
        ActualizarRuta(null);
        FillObjectsList();
        //Recorremos la lista de instancias y las almacenamos en su lista correspondiente
        foreach (GameObject item in listaObjetos)
        {
            if (item == null) continue;

            switch (item.tag)
            {
                case "Ground":
                    GroundData ground = new GroundData(item.name, item.transform, item.GetInstanceID());
                    ground.SetSizeData(item.GetComponent<SpriteRenderer>().size, item.GetComponent<BoxCollider2D>().size, item.transform.Find("Sides").GetComponent<BoxCollider2D>().size);
                    ground.SetImanDchoData(item.transform.Find("ImanDcho").transform.position, item.transform.Find("ImanDcho").GetComponent<BoxCollider2D>().size);
                    ground.SetImanIzqData(item.transform.Find("ImanIzqd").transform.position, item.transform.Find("ImanIzqd").GetComponent<BoxCollider2D>().size);
                    listaSuelos.Add(ground);
                    break;
                case "Door":
                    listaPuertas[contadorPuertas].position = item.transform.position;
                    listaPuertas[contadorPuertas].curScene = dataPath;
                    listaPuertas[contadorPuertas].SetScaleSettings(item.GetComponent<SpriteRenderer>().size, item.GetComponent<BoxCollider2D>().size);
                    item.GetComponent<ChangeScene>().SetDataPath(dataPath);

                    contadorPuertas++;
                    //listaPuertas.Add(new DoorData(item.name, item.transform, item.GetComponent<ChangeScene>().id));
                    break;
                case "Enemy":
                    listaEnemigos.Add(new EnemiesData(item.name, item.transform, item.GetInstanceID()));
                    break;
                case "Player":
                    ManagePlayerInfo(item);
                    break;
                case "Objects":
                case "Background":
                    ObjectsData data = new ObjectsData(item.name, item.transform, item.GetInstanceID());
                    if (!item.name.Contains("Backgr"))
                    {
                        data.SetScaleSettings(item.GetComponent<SpriteRenderer>().size, item.GetComponent<BoxCollider2D>().size);
                    }
                    listaObjects.Add(data);
                    break;
            }
            
        }
        //Creamos el StreamWriter para escribir en ficheros y limpiamos la lista
        StreamWriter streamWriter;
        //listaObjetos.Clear();
        //Guardamos datos de los suelos
        using (streamWriter = File.CreateText(dataPath+"\\Grounds.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaSuelos.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        //Guardamos datos de las puertas
        using (streamWriter = File.CreateText(dataPath+ "\\Puertas.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaPuertas.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        //Guardamos datos de los Enemigos
        using (streamWriter = File.CreateText(dataPath + "\\Enemies.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaEnemigos.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        //Guardamos datos de los Players
        using (streamWriter = File.CreateText(dataPath + "\\Players.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaPlayers.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        //Guardamos datos de los Objects
        using (streamWriter = File.CreateText(dataPath + "\\Objects.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaObjects.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        listaSuelos.Clear();
        listaEnemigos.Clear();
        listaPlayers.Clear();
        listaObjects.Clear();
        saved = true;
    }

    public void LoadData()
    {
        //Instancia para cargar los objetos
        GameObject instancia;
        //Booleano para mostrar o no el mensaje de advertencia
        bool continuar=false;
        //Se comprueba si el nivel está guardado, y si no, se muestra un mesaje de advertencia.
        if (IsSaveNecessary())
        {
            continuar = CheckSave();
        }
        else
        {
            continuar = true;
        }
        //Si el usuario puede/quiere cargar partida, comienza la fiesta
        if (continuar)
        {
            string path = "";
            if (tempPath == "")
            {
                //Mostramos una ventana para seleccionar la carpeta del nivel correspondiente a cargar
                path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];
            }
            else
            {
                path = tempPath;
            }

            //Si la ruta no está vacía, se cargan los ficheros
            if (path != "")
            {
                //Obtenemos todos los ficheros de almacenamiento del nivel
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] info = dir.GetFiles("*.*");
                //Eliminamos todos los objetos creados en la escena para cargar un nuevo nivel
                dataPath = path;

                if (info.Length == 0) return;
                foreach (Transform child in GameObject.Find("Objects").transform)
                {
                    if (child.name.Contains("(Clone)"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                //Limpiamos la lista de gameobjects
                listaObjetos.Clear();
                Camera.main.transform.position = new Vector3(0.0f, 0.0f, -10);

                //Uno por uno, cargamos e instanciamos los objetos correspondientes
                foreach(FileInfo file in info)
                {
                    switch (file.Name)
                    {
                        //Para los suelos
                        case "Grounds.txt":
                            listaSuelos.Clear();
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
                                instancia.transform.position = suelos[i].position;
                                instancia.transform.rotation = Quaternion.Euler(suelos[i].rotation);
                                instancia.GetComponent<SpriteRenderer>().size = suelos[i].spriteSize;
                                instancia.GetComponent<BoxCollider2D>().size = suelos[i].boxColliderSize;
                                instancia.transform.Find("ImanDcho").position = suelos[i].imanDchoPos;
                                instancia.transform.Find("ImanDcho").transform.GetComponent<BoxCollider2D>().size = suelos[i].imanDchoBoxColliderSize;
                                instancia.transform.Find("ImanIzqd").transform.position = suelos[i].imanIzqPos;
                                instancia.transform.Find("ImanIzqd").transform.GetComponent<BoxCollider2D>().size = suelos[i].imanIzqBoxColliderSize;
                                instancia.transform.Find("Sides").GetComponent<BoxCollider2D>().size = suelos[i].sidesSize;
                                if (instancia.transform.Find("Marco") != null) instancia.transform.Find("Marco").gameObject.SetActive(false);
                                //listaSuelos.Add(suelos[i]);
                                listaObjetos.Add(instancia);
                            }
                            break;
                        //Para las puertas
                        case "Puertas.txt":
                            listaPuertas.Clear();
                            //Generamos un array de puertas que contendrá todas las puertas del nivel
                            DoorData[] puertas;
                            //Leemos el JSON
                            using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                            {
                                string jsonString = streamReader.ReadToEnd();

                                puertas = JsonHelper.FromJson<DoorData>(jsonString);
                            }
                            stateMachine.GetComponent<StateMachine>().count = 0;
                            //Una vez leido el fichero, se instancian todas las puertas en la escena
                            for (int i = 0; i < puertas.Length; i++)
                            {
                                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + puertas[i].doorName, typeof(GameObject)));
                                instancia.transform.parent = GameObject.Find("Objects").transform;
                                instancia.transform.position = puertas[i].position;
                                instancia.transform.rotation = Quaternion.Euler(puertas[i].rotation);
                                instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                                instancia.GetComponent<ChangeScene>().SetDataPath(path);
                                instancia.GetComponent<SpriteRenderer>().size = puertas[i].spriteSize;
                                instancia.GetComponent<BoxCollider2D>().size = puertas[i].boxColliderSize;
                                listaPuertas.Add(puertas[i]);
                                listaObjetos.Add(instancia);
                            }
                            break;
                        case "Enemies.txt":
                            listaEnemigos.Clear();
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

                                instancia.GetComponent<MoveObject>().enabled = true;
                                if (instancia.GetComponent<BasicEnemyMovement>() != null) instancia.GetComponent<BasicEnemyMovement>().enabled = false;
                                if (instancia.GetComponent<MyRayCast>() != null) instancia.GetComponent<MyRayCast>().enabled = false;
                                if (instancia.GetComponent<MyDistanceAttack>() != null) instancia.GetComponent<MyDistanceAttack>().enabled = false;

                                instancia.transform.position = enemies[i].position;
                                //listaSuelos.Add(suelos[i]);
                                listaObjetos.Add(instancia);
                            }
                            break;
                        case "Players.txt":
                            listaPlayers.Clear();
                            //Generamos un array de suelos que contendrá todos los suelos del nivel
                            PlayerData[] players;
                            //Leemos el JSON
                            using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                            {
                                string jsonString = streamReader.ReadToEnd();

                                players = JsonHelper.FromJson<PlayerData>(jsonString);
                            }
                            //Una vez leido el fichero, se instancian todos los suelos en la escena
                            for(int i = 0; i< players.Length; i++)
                            {
                                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + players[i].name, typeof(GameObject)));
                                instancia.transform.parent = GameObject.Find("Objects").transform;

                                //Gestionamos scripts para edicion
                                instancia.GetComponent<PlayerMovement>().enabled = false;
                                instancia.GetComponent<Rigidbody2D>().gravityScale = 0;
                                instancia.GetComponent<MoveObject>().enabled = true;
                                instancia.GetComponent<PlayerStats>().enabled = false;
                                //Fin Gestion

                                instancia.transform.position = players[i].position;
                                PlayerMovement movement = instancia.GetComponent<PlayerMovement>();
                                movement.speed = players[i].speed;
                                movement.jumpForce = players[i].jumpForce;
                                movement.jumpPushForce = players[i].jumpPushForce;
                                movement.jumpWallForce = players[i].jumpWallForce;
                                instancia.GetComponent<PlayerStats>().maxHealth = players[i].health;
                                listaObjetos.Add(instancia);
                            }
                            break;
                        case "Objects.txt":
                            listaObjects.Clear();
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
                                instancia.transform.position = objects[i].position;
                                listaObjetos.Add(instancia);
                                if (instancia.GetComponent<ChangeScene>() != null) instancia.GetComponent<ChangeScene>().enabled = false;
                                if (instancia.GetComponent<Animator>() != null) instancia.GetComponent<Animator>().enabled = false;
                                if (!instancia.name.Contains("Backgr"))
                                {
                                    instancia.transform.rotation = Quaternion.Euler(objects[i].rotation);
                                    instancia.GetComponent<SpriteRenderer>().size = objects[i].spriteSize;
                                    instancia.GetComponent<BoxCollider2D>().size = objects[i].boxColliderSize;
                                }
                                //if (instancia.GetComponent<Damage>() != null) instancia.GetComponent<Damage>().enabled = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                
            }
            tempPath = "";
            saved = true;
        }

    }

    public void ActualizarRuta(string name)
    {
        //Si la ruta está vacía porque es un nuevo nivel, se pregunta por el nombre del nivel
        if (name == null)
        {
            if (dataPath == "")
            {
                //Se decide el nombre y se crea el directorio
                dataPath = StandaloneFileBrowser.OpenFolderPanel("Seleccione Carpeta de Guardado", UnityEngine.Application.persistentDataPath, false)[0];
                //dataPath = Path.Combine(Application.persistentDataPath, "GroundData.txt");
                Directory.CreateDirectory(dataPath);

            }
        }
        else
        {
            dataPath = name;
        }

    }

    public void GuardarRuta(string name)
    {
        tempPath = name;
    }

    public void ActualizarDatosPuerta(int idPuertaActual, int idPuertaNueva, string rutaNivel, string postName)
    {

        foreach(DoorData door in listaPuertas)
        {
            if (door.id == idPuertaActual)
            {
                door.postId=idPuertaNueva;
                door.postScene = rutaNivel;
                //door.userDoorName = name;
                if (postName != null)
                {
                    door.postDoorName = postName;
                }
            }
        }
        saved = false;
    }

    public bool CheckSave()
    {

        DialogResult result = MessageBox.Show("Si continua, se borrarán los datos. Desea continuar?", "Cuidado!",
                             MessageBoxButtons.OKCancel,
                             MessageBoxIcon.Exclamation);
        if (result == DialogResult.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool CheckExit()
    {

        DialogResult result = MessageBox.Show("Está seguro de que desea salir? Se perderán los cambios no guardados", "Cuidado!",
                             MessageBoxButtons.OKCancel,
                             MessageBoxIcon.Exclamation);
        if (result == DialogResult.OK)
        {
            return true;
        }
        return false;
    }

    void ManagePlayerInfo(GameObject item)
    {
        string name = item.name;
        Transform transf = item.transform;
        PlayerMovement playerMov = item.GetComponent<PlayerMovement>();
        float speed = playerMov.speed;
        float jump = playerMov.jumpForce;
        float jumpPush = playerMov.jumpPushForce;
        float jumpWall = playerMov.jumpWallForce;
        float health = item.GetComponent<PlayerStats>().maxHealth;

        listaPlayers.Add(new PlayerData(name, transf, speed, jump, jumpPush, jumpWall, health));

    }

    public void SaveNecessary()
    {
        saved = false;
    }

    public bool IsSaveNecessary() { return !saved; }

    private void ManageDoorName()
    {
        doorName = GameObject.Find("StateController").GetComponent<StateMachine>().GetDoorNameOption();

        Transform doorNameText = null;
        Transform doorNameBtn = null;

        for (int i = 0; i < doorName.transform.childCount; ++i)
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
        input = doorNameText.GetComponent<InputField>();
        input.onValueChanged.AddListener(delegate { ValueChange(doorNameBtn, input); });
        doorNameBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { SaveDoorName(); });
    }

    public void ValueChange(Transform doorBtn, InputField input)
    {
        if (input.text != "")
        {
            doorBtn.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            doorBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void SaveDoorName()
    {

        InsertDoor(listaObjetos[listaObjetos.Count -1], input.text);
        input.text = "";
        GameObject.Find("StateController").GetComponent<StateMachine>().HideNameOption();
        canEnterText = false;
    }

    public void RemoveDoorFromList(int id)
    {
        foreach(DoorData puerta in listaPuertas)
        {
            if (puerta.id == id) listaPuertas.Remove(puerta);
        }
    }

    public List<String> GetCurrentDoors()
    {
        List<String> nombres = new List<String>();
        foreach(DoorData puerta in listaPuertas)
        {
            nombres.Add(puerta.userDoorName);
        }
        return nombres;
    }

    public List<DoorData> GetCurrentDoorsData()
    {
        return listaPuertas;
    }

}

