using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[Serializable]
public class SaveGround : MonoBehaviour
{

    public List<GameObject> listaSuelos;
    [SerializeField]
    public List<GroundData> listaDatos;
    string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        listaSuelos = new List<GameObject>();
        listaDatos = new List<GroundData>();
        dataPath = Path.Combine(Application.persistentDataPath, "GroundData.txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertGround(GameObject suelo)
    {
        listaSuelos.Add(suelo);
    }

    //public void SaveData()
    //{  
    //    string jsonString = "{\"Grounds\":[";
    //    StreamWriter streamWriter;
    //    using (streamWriter = File.CreateText(dataPath))
    //    {
    //        for(int i = 0; i<listaSuelos.Count;i++)
    //        {
    //            /*
    //            GroundData dato = new GroundData(ground.name, ground.transform);
    //            listaDatos.Add(dato);
    //            */
    //            jsonString += JsonUtility.ToJson(new GroundData(listaSuelos[i].name, listaSuelos[i].transform));
    //            if(i!=listaSuelos.Count - 1)
    //            {
    //                jsonString += ",";
    //                streamWriter.Write(jsonString);
    //            }
    //            else
    //            {
    //                streamWriter.Write(jsonString);
    //                streamWriter.Write("]}");
    //            }
    //            jsonString = "";
    //        }
    //    }
    //}

    public void SaveData()
    {
        foreach (GameObject ground in listaSuelos)
        {
            listaDatos.Add(new GroundData(ground.name, ground.transform));
        }
        StreamWriter streamWriter;
        using (streamWriter = File.CreateText(dataPath))
        {
            string jsonString = JsonHelper.ToJson(listaDatos.ToArray(), true);
            streamWriter.Write(jsonString);
        }
    }

    public void LoadData()
    {
        GroundData[] suelos;
        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            Debug.Log(jsonString);
            suelos = JsonHelper.FromJson<GroundData>(jsonString);
        }
        GameObject instancia;
        for (int i = 0; i < suelos.Length; i++)
        {
            instancia = Instantiate((GameObject)Resources.Load("prefabs/" + suelos[i].groundName, typeof(GameObject)));
            instancia.transform.parent = GameObject.Find("Canvas").transform;
            instancia.transform.position = suelos[i].position;
        }
    }

}
