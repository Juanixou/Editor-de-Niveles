using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using SFB;

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
    public GameObject doorNameGO;
    private DoorData[] puertas;
    private Text nextDoor;


    // Start is called before the first frame update
    void Start()
    {
        controlador = GameObject.Find("DataController");
        listaOpciones = new List<string>();
        puertas = new DoorData[10];
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

    public void ActivarEstadosCarga(string doorName, string nextDoorName)
    {
        for(int i = 1; i <= 3; i++)
        {
            ventana.transform.Find(i.ToString()).gameObject.SetActive(false);
        }


        count++;
        if (count <= 3)
        {
            GameObject escena = ventana.transform.Find(count.ToString()).gameObject;
            escena.SetActive(true);
            if(doorName!="")
            escena.transform.Find("Nombre_" + count.ToString()).GetComponent<InputField>().text = doorName;
            escena.transform.Find("NextDoor_" + count.ToString()).GetComponent<Text>().text = nextDoorName;
        }


    }

    public void AsignarEscena()
    {

        idPuertaActual = Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
        nextDoor = EventSystem.current.currentSelectedGameObject.transform.Find("NextDoor_"+idPuertaActual).GetComponent<Text>();

        nextScene = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];

        if (nextScene != "")
        {
            ventana.SetActive(false);
            listaEscenas.SetActive(true);
            listaOpciones.Add("None");
            if(puertas.Length!=0)
            Array.Clear(puertas, 0, puertas.Length);
            using (StreamReader streamReader = File.OpenText(nextScene + "\\Puertas.txt"))
            {
                string jsonString = streamReader.ReadToEnd();

                puertas = JsonHelper.FromJson<DoorData>(jsonString);
            }
            //Una vez leido el fichero, se instancian todas las puertas en la escena
            for (int i = 0; i < puertas.Length; i++)
            {
                listaOpciones.Add(puertas[i].userDoorName);
            }
            opciones.ClearOptions();
            opciones.AddOptions(listaOpciones);
            listaOpciones.Clear();
        }

    }

    public void DropdownValueChanged(Dropdown drop)
    {
        int id=-1;

            for(int i = 0; i < puertas.Length; i++)
            {
                if (puertas[i].userDoorName == drop.options[drop.value].text)
                {
                    id = puertas[i].id;
                nextDoor.text = puertas[i].userDoorName;
                }   
            }
    }

    public GameObject ShowDoorNameOption()
    {
        doorNameGO.SetActive(true);
        return doorNameGO;
    }

    public void HideNameOption()
    {
        doorNameGO.SetActive(false);
    }

}
