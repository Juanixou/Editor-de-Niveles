using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;
using SFB;
using System.Windows.Forms;
using UnityEngine.SceneManagement;

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
    public string dataPath;
    public GameObject stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = "";
        loaded = false;
        listaObjetos = new List<GameObject>();
        listaSuelos = new List<GroundData>();
        listaEnemigos = new List<EnemiesData>();
        listaPlayers = new List<PlayerData>();
        stateMachine = GameObject.Find("StateController");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (CheckExit())
            {
                SceneManager.LoadScene("Inicio", LoadSceneMode.Single);
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
        listaPuertas.Add(new DoorData(puerta.name, puerta.transform, puerta.GetComponent<ChangeScene>().id,name));
    }

    public void SaveData()
    {
        int contadorPuertas = 0;
        //Comprobamos si hay ruta establecida
        ActualizarRuta(null);
        
        //Recorremos la lista de instancias y las almacenamos en su lista correspondiente
        foreach (GameObject item in listaObjetos)
        {
            if (item == null) continue;
            //Si es un suelo...
            switch (item.tag)
            {
                case "Ground":
                    listaSuelos.Add(new GroundData(item.name, item.transform, item.GetInstanceID()));
                    break;
                case "Door":
                    listaPuertas[contadorPuertas].position = item.transform.position;
                    contadorPuertas++;
                    //listaPuertas.Add(new DoorData(item.name, item.transform, item.GetComponent<ChangeScene>().id));
                    break;
                case "Enemy":
                    listaEnemigos.Add(new EnemiesData(item.name, item.transform, item.GetInstanceID()));
                    break;
                case "Player":
                    ManagePlayerInfo(item);
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
        Debug.Log("Players: " + listaPlayers.Count);
        using (streamWriter = File.CreateText(dataPath + "\\Players.txt"))
        {
            string jsonString = JsonHelper.ToJson(listaPlayers.ToArray(), true);
            streamWriter.Write(jsonString);
        }
        listaSuelos.Clear();
        listaPuertas.Clear();
        listaEnemigos.Clear();
        listaPlayers.Clear();
    }

    public void LoadData()
    {
        //Instancia para cargar los objetos
        GameObject instancia;
        //Booleano para mostrar o no el mensaje de advertencia
        bool continuar=false;
        //Se comprueba si el nivel está guardado, y si no, se muestra un mesaje de advertencia.
        //TODO: COMPROBAR CORRECTAMENTE LOS CAMBIOS EN EL NIVEL
        if (listaSuelos.Count != 0)
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
            //Mostramos una ventana para seleccionar la carpeta del nivel correspondiente a cargar
            string path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];
            //string path = path2[0];
            //Si la ruta no está vacía, se cargan los ficheros
            if (path != "")
            {
                //Eliminamos todos los objetos creados en la escena para cargar un nuevo nivel
                dataPath = path;
                foreach (Transform child in GameObject.Find("Objects").transform)
                {
                    if (child.name.Contains("(Clone)"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                //Obtenemos todos los ficheros de almacenamiento del nivel
                DirectoryInfo dir = new DirectoryInfo(dataPath);
                FileInfo[] info = dir.GetFiles("*.*");

                //Limpiamos la lista de gameobjects
                listaObjetos.Clear();
                Camera.main.transform.position = Vector3.zero;
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
                                instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                                //listaPuertas.Add(puertas[i]);
                                listaObjetos.Add(instancia);

                                stateMachine.GetComponent<StateMachine>().ActivarEstadosCarga(puertas[i].userDoorName, puertas[i].postDoorName);
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
                        default:
                            break;
                    }
                }
                
            }

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
                dataPath = StandaloneFileBrowser.OpenFolderPanel("Select Level Name", UnityEngine.Application.persistentDataPath, false)[0];
                //dataPath = Path.Combine(Application.persistentDataPath, "GroundData.txt");
                Directory.CreateDirectory(dataPath);

            }
        }
        else
        {
            dataPath = name;
        }

    }

    public void ActualizarDatosPuerta(int idPuertaActual, int idPuertaNueva, string rutaNivel, string name, string postName)
    {

        foreach(DoorData door in listaPuertas)
        {
            if (door.id == idPuertaActual)
            {
                door.postId=idPuertaNueva;
                door.postScene = rutaNivel;
                door.userDoorName = name;
                if (postName != null)
                {
                    door.postDoorName = postName;
                }
            }
        }
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
        return false;
    }

    public bool CheckExit()
    {

        DialogResult result = MessageBox.Show("Está seguro de que desea salir?", "Cuidado!",
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
        Debug.Log("Guardando player " + name);

        listaPlayers.Add(new PlayerData(name, transf, speed, jump, jumpPush, jumpWall, health));

    }

}

