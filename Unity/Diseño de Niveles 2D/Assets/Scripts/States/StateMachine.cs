using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class StateMachine : MonoBehaviour
{

    public GameObject ventana;
    public GameObject listaEscenas;
    public Dropdown opciones;
    public int count;
    public int idPuertaActual;
    public string nextScene;
    public List<string> listaOpciones;
    public GameObject controlador;

    // Start is called before the first frame update
    void Start()
    {
        controlador = GameObject.Find("DataController");
        listaOpciones = new List<string>();
        count = 0;
        opciones.onValueChanged.AddListener(delegate {DropdownValueChanged(opciones);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarMaquina()
    {
        ventana.SetActive(true);
    }

    public int ActivarEstados()
    {
        count++;
        //Limitamos las puertas a 3 por nivel y activamos la ventana de configuración de estados.
        if (count <= 3)
        {
            ventana.transform.Find(count.ToString()).gameObject.SetActive(true);
        }

        return count;
    }

    public void AsignarEscena()
    {
        idPuertaActual= Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log("Puerta Actual: " + idPuertaActual);
        DoorData[] puertas;
        nextScene = EditorUtility.OpenFolderPanel("Select Level", Application.persistentDataPath, "");
        if (nextScene != "")
        {
            ventana.SetActive(false);
            listaEscenas.SetActive(true);
            listaOpciones.Add("None");
            using (StreamReader streamReader = File.OpenText(nextScene + "\\Puertas.txt"))
            {
                string jsonString = streamReader.ReadToEnd();

                puertas = JsonHelper.FromJson<DoorData>(jsonString);
            }
            //Una vez leido el fichero, se instancian todas las puertas en la escena
            for (int i = 0; i < puertas.Length; i++)
            {
                listaOpciones.Add(puertas[i].id.ToString());
            }
            opciones.ClearOptions();
            opciones.AddOptions(listaOpciones);
        }

    }

    public void DropdownValueChanged(Dropdown drop)
    {
        int id;
        if (drop.options[drop.value].text == "None")
        {
            id = -1;
        }
        else
        {
            id = Int32.Parse(drop.options[drop.value].text);
        }

        controlador.GetComponent<SaveGround>().ActualizarDatosPuerta(idPuertaActual, id , nextScene);
    }

}
