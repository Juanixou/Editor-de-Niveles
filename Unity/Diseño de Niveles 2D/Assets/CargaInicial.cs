using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CargaInicial : MonoBehaviour
{
    GameObject controladorDatos;
    // Start is called before the first frame update
    void Start()
    {
        controladorDatos = GameObject.Find("DataController");
        if (PlayerPrefs.GetString("path") != "" && controladorDatos.GetComponent<SaveGround>() != null)
        {
            controladorDatos.GetComponent<SaveGround>().dataPath = PlayerPrefs.GetString("path");
            controladorDatos.GetComponent<SaveGround>().tempPath = PlayerPrefs.GetString("path");
            PlayerPrefs.SetString("path", "");
            controladorDatos.GetComponent<SaveGround>().LoadData();
            //LoadData();
        }

        if(PlayerPrefs.GetString("playPath") != "" && controladorDatos.GetComponent<LoadLevel>() != null)
        {
            controladorDatos.GetComponent<LoadLevel>().LoadData();
        }
    }
}
