using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using SFB;
using System.Windows.Forms;

public class StateMachine : MonoBehaviour
{

    public GameObject ventana;
    public GameObject listaEscenas;
    public Dropdown opciones;
    public Dropdown curDoors;
    public int count;
    public int idPuertaActual = -1;
    public string nextScene;
    public List<string> listaOpciones;
    public List<string> listaCurDoors;
    public GameObject controlador;
    public GameObject doorNameGO;
    private DoorData[] puertas;
    private Text nextDoor;

    private int nextDoorId = -1;


    // Start is called before the first frame update
    void Start()
    {
        controlador = GameObject.Find("DataController");
        listaOpciones = new List<string>();
        listaCurDoors = new List<string>();
        puertas = new DoorData[100];
        count = 0;
        opciones.onValueChanged.AddListener(delegate {DropdownValueChanged(opciones);});
        curDoors.onValueChanged.AddListener(delegate { DropdownCurDoorChanged(curDoors);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarMaquina()
    {
        if (controlador.GetComponent<SaveGround>().IsSaveNecessary())
        {
            if (CheckSave())
            {
                controlador.GetComponent<SaveGround>().SaveData();
                listaEscenas.SetActive(true);
                AsignarPuertasActuales();
            }
        }
        else
        {
            listaEscenas.SetActive(true);
            AsignarPuertasActuales();
        }

    }

    public void AsignarEscena()
    {

        nextScene = StandaloneFileBrowser.OpenFolderPanel("Seleccione carpeta del nivel", UnityEngine.Application.persistentDataPath, false)[0];

        if (nextScene != "")
        {
            nextDoorId = -1;
            idPuertaActual = -1;
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

    public void AsignarPuertasActuales()
    {
        listaCurDoors.Add("None");
        foreach (string nombrePuerta in controlador.GetComponent<SaveGround>().GetCurrentDoors())
        {
            listaCurDoors.Add(nombrePuerta);
        }
        curDoors.ClearOptions();
        
        curDoors.AddOptions(listaCurDoors);
        listaCurDoors.Clear();
        AsignarEscena();
    }

    public void DropdownValueChanged(Dropdown drop)
    {
        nextDoorId = -1;
            for(int i = 0; i < puertas.Length; i++)
            {
                if (puertas[i].userDoorName == drop.options[drop.value].text)
                {

                nextDoorId = puertas[i].id;
                }   
            }
    }

    public void DropdownCurDoorChanged(Dropdown drop)
    {

        idPuertaActual = -1;
        foreach(DoorData puerta in controlador.GetComponent<SaveGround>().GetCurrentDoorsData())
        {
            if (puerta.userDoorName.Equals(drop.options[drop.value].text))
            {
                idPuertaActual = puerta.id;
            }
        }

    }

    public void ShowDoorNameOption()
    {
        doorNameGO.SetActive(true);
        controlador.GetComponent<SaveGround>().canEnterText = true;
        InputField input = doorNameGO.transform.GetChild(1).GetComponent<InputField>();
        input.Select();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public GameObject GetDoorNameOption()
    {
        return doorNameGO;
    }

    public void HideNameOption()
    {
        doorNameGO.SetActive(false);
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }


    public void SaveStates()
    {
        if (idPuertaActual != -1 && nextDoorId != -1)
        {
            Debug.Log(nextScene);
            controlador.GetComponent<SaveGround>().ActualizarDatosPuerta(idPuertaActual, nextDoorId, nextScene, opciones.options[opciones.value].text);
            opciones.value = 0;
            curDoors.value = 0;
            listaEscenas.SetActive(false);
        }
        
    }

    public bool CheckSave()
    {

        DialogResult result = MessageBox.Show("Se guardará el nivel. Continuar?", "Cuidado!",
                             MessageBoxButtons.OK,
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

}
