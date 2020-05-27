using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LoadData()
    {

        string dataPath = PlayerPrefs.GetString("playPath");
        GameObject instancia;

        DirectoryInfo dir = new DirectoryInfo(dataPath);
        FileInfo[] info = dir.GetFiles("*.*");

        Camera.main.transform.position = new Vector3(0.0f, 0.0f, -10);
        //Uno por uno, cargamos e instanciamos los objetos correspondientes
        foreach (FileInfo file in info)
        {
            switch (file.Name)
            {
                //Para los suelos
                case "Grounds.txt":
                    //Generamos un array de suelos que contendrá todos los suelos del nivel
                    GroundData[] suelos;
                    //Leemos el JSON
                    using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
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
                        if (instancia.transform.Find("Marco") != null) instancia.transform.Find("Marco").gameObject.SetActive(false);

                    }
                    break;
                //Para las puertas
                case "Puertas.txt":
                    //Generamos un array de puertas que contendrá todas las puertas del nivel
                    DoorData[] puertas;
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
                        instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                        instancia.GetComponent<ChangeScene>().SetDataPath(dataPath);
                        instancia.GetComponent<SpriteRenderer>().size = puertas[i].spriteSize;
                        instancia.GetComponent<BoxCollider2D>().size = puertas[i].boxColliderSize;
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
                case "Players.txt":
                    //Generamos un array de suelos que contendrá todos los suelos del nivel
                    PlayerData[] players;
                    //Leemos el JSON
                    using (StreamReader streamReader = File.OpenText(dataPath + "\\" + file.Name))
                    {
                        string jsonString = streamReader.ReadToEnd();

                        players = JsonHelper.FromJson<PlayerData>(jsonString);
                    }
                    //Una vez leido el fichero, se instancian todos los suelos en la escena
                    for (int i = 0; i < players.Length; i++)
                    {
                        instancia = Instantiate((GameObject)Resources.Load("prefabs/" + players[i].name, typeof(GameObject)));
                        instancia.transform.parent = GameObject.Find("Objects").transform;

                        //Gestionamos scripts para edicion
                        instancia.GetComponent<PlayerMovement>().enabled = true;
                        instancia.GetComponent<MoveObject>().enabled = false;
                        //Fin Gestion

                        instancia.transform.localPosition = players[i].position;
                        PlayerMovement movement = instancia.GetComponent<PlayerMovement>();
                        movement.speed = players[i].speed;
                        movement.jumpForce = players[i].jumpForce;
                        movement.jumpPushForce = players[i].jumpPushForce;
                        movement.jumpWallForce = players[i].jumpWallForce;
                        instancia.GetComponent<PlayerStats>().maxHealth = players[i].health;
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
                        if(instancia.GetComponent<MoveObject>() != null)instancia.GetComponent<MoveObject>().enabled = false;
                        if (instancia.GetComponent<ChangeScene>() != null) instancia.GetComponent<ChangeScene>().enabled = true;
                        if (instancia.GetComponent<Animator>() != null) instancia.GetComponent<Animator>().enabled = true;
                        if (instancia.GetComponent<Damage>() != null) instancia.GetComponent<Damage>().enabled = true;
                        if (!instancia.name.Contains("Backgr"))
                        {
                            instancia.GetComponent<SpriteRenderer>().size = objects[i].spriteSize;
                            instancia.GetComponent<BoxCollider2D>().size = objects[i].boxColliderSize;
                            instancia.transform.rotation = Quaternion.Euler(objects[i].rotation);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
