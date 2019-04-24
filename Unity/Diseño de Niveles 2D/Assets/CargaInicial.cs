using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaInicial : MonoBehaviour
{
    GameObject controladorDatos;
    // Start is called before the first frame update
    void Start()
    {
        controladorDatos = GameObject.Find("DataController");
        if (PlayerPrefs.GetString("path") != "")
        {
            controladorDatos.GetComponent<SaveGround>().dataPath = PlayerPrefs.GetString("path");
            PlayerPrefs.SetString("path", "");
            controladorDatos.GetComponent<SaveGround>().LoadData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
