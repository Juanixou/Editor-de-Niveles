﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class SaveGround : MonoBehaviour
{
    public bool loaded;
    public List<GameObject> listaObjetos;
    [SerializeField]
    public List<GroundData> listaSuelos;
    public List<DoorData> listaPuertas;
    public string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = "";
        loaded = false;
        listaObjetos = new List<GameObject>();
        listaSuelos = new List<GroundData>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertGround(GameObject suelo)
    {
        //Añadimos a la lista los elementos instanciados
        listaObjetos.Add(suelo);
    }

    public void InsertDoor(GameObject puerta)
    {
        listaPuertas.Add(new DoorData(puerta.name, puerta.transform, puerta.GetComponent<ChangeScene>().id));
    }

    public void SaveData()
    {
        int contadorPuertas = 0;
        //Comprobamos si hay ruta establecida
        ActualizarRuta();
        
        //Recorremos la lista de instancias y las almacenamos en su lista correspondiente
        foreach (GameObject item in listaObjetos)
        {
            //Si es un suelo...
            if (item.tag == "Ground")
            {
                listaSuelos.Add(new GroundData(item.name, item.transform, item.GetInstanceID()));
            }
            else //Si es una puerta...
            {
                listaPuertas[contadorPuertas].position = item.transform.position;
                contadorPuertas++;
                //listaPuertas.Add(new DoorData(item.name, item.transform, item.GetComponent<ChangeScene>().id));
            }
            
        }
        //Creamos el StreamWriter para escribir en ficheros y limpiamos la lista
        StreamWriter streamWriter;
        listaObjetos.Clear();
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

    }

    public void LoadData()
    {
        //Instancia para cargar los objetos
        GameObject instancia;
        //Booleano para mostrar o no el mensaje de advertencia
        bool continuar=false;
        //Se comprueba si el nivel está guardado, y si no, se muestra un mesaje de advertencia.
        if (listaObjetos.Count != 0)
        {
            continuar = EditorUtility.DisplayDialog("Cuidado!", "Si continua, se borrarán los datos. Desea continuar?", "Si", "No");
        }
        else
        {
            continuar = true;
        }
        //Si el usuario puede/quiere cargar partida, comienza la fiesta
        if (continuar)
        {
            //Mostramos una ventana para seleccionar la carpeta del nivel correspondiente a cargar
            string path = EditorUtility.OpenFolderPanel("Select Level", Application.persistentDataPath, "");
            //Si la ruta no está vacía, se cargan los ficheros
            if (path != "")
            {
                //Eliminamos todos los objetos creados en la escena para cargar un nuevo nivel
                dataPath = path;
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
                                instancia.transform.parent = GameObject.Find("Canvas").transform;
                                instancia.transform.position = suelos[i].position;
                                listaSuelos.Add(suelos[i]);
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
                            //Una vez leido el fichero, se instancian todas las puertas en la escena
                            for (int i = 0; i < puertas.Length; i++)
                            {
                                instancia = Instantiate((GameObject)Resources.Load("prefabs/" + puertas[i].doorName, typeof(GameObject)));
                                instancia.transform.parent = GameObject.Find("Canvas").transform;
                                instancia.transform.position = puertas[i].position;
                                instancia.GetComponent<ChangeScene>().id = puertas[i].id;
                                listaPuertas.Add(puertas[i]);
                            }
                            break;


                        default:
                            break;
                    }
                }
                
            }

        }

    }

    public void ActualizarRuta()
    {
        //Si la ruta está vacía porque es un nuevo nivel, se pregunta por el nombre del nivel
        if (dataPath == "")
        {
            //Se decide el nombre y se crea el directorio
            dataPath = EditorUtility.SaveFilePanel("Select Level Name", Application.persistentDataPath, "", "");
            //dataPath = Path.Combine(Application.persistentDataPath, "GroundData.txt");
            Directory.CreateDirectory(dataPath);

        }
    }

    public void ActualizarDatosPuerta(int idPuertaActual, int idPuertaNueva, string rutaNivel)
    {

        foreach(DoorData door in listaPuertas)
        {
            if (door.id == idPuertaActual)
            {
                door.postId=idPuertaNueva;
                door.postScene = rutaNivel;
            }
        }
    }

}
