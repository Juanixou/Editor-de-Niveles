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
        if (SelectLevel.path != null)
        {
            controladorDatos.GetComponent<SaveGround>().dataPath = SelectLevel.path;
            controladorDatos.GetComponent<SaveGround>().LoadData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
