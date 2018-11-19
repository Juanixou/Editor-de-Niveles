using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour {

    private Dialogos dialog;
    private GameObject gestorTexto;

	// Use this for initialization
	void Start () {
        gestorTexto = GameObject.Find("ControladorTexto");
        dialog = gestorTexto.GetComponent<Dialogos>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "Player") && (Input.GetKeyDown(KeyCode.E)))
        {
            Debug.Log("Y me llamo " + this.name);
            dialog.GestionarDialogo(this.name);
        }
            
    }
}
