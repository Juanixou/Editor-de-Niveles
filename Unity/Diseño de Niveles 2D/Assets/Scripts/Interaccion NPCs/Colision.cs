using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour {

    private Dialogos dialog;
    private GameObject gestorTexto;
    public GameObject tecla;
    private bool colision;

	// Use this for initialization
	void Start () {
        tecla = this.gameObject.transform.GetChild(0).gameObject;
        gestorTexto = GameObject.Find("ControladorTexto");
        dialog = gestorTexto.GetComponent<Dialogos>();
	}
	
	// Update is called once per frame
	void Update () {
        if ((colision) && (Input.GetKeyDown(KeyCode.E)))
        {
            tecla.SetActive(false);
            dialog.GestionarDialogo(this.name,tecla); 
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((collision.tag == "Player"))
        {
            tecla.SetActive(true);
            colision = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Player"))
        {
            tecla.SetActive(false);
            colision = false;
        }
    }
}
