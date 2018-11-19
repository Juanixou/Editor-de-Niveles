using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;


public class Dialogos : MonoBehaviour {

    public GameObject canvas;
    public GameObject mensaje;
    private GameObject player;
    private Platformer2DUserControl movement;
    private float time;
    private Text texto;
    private bool bloqMov;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        time = 0.0f;
        movement = player.GetComponent<Platformer2DUserControl>();
        bloqMov = false;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if((time>= 5.0f)&&(bloqMov==true))
        {
            movement.enabled = true;
            mensaje.SetActive(false);
            bloqMov = false;
        }
	}

    public void GestionarDialogo(string nombre)
    {
        //GameObject mensaje = Instantiate(nube, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //mensaje.transform.parent = canvas.transform;

        movement.enabled = false;
        switch (nombre)
        {
            case "NPC1":
                
                texto = mensaje.GetComponentInChildren<Text>();
                texto.text = "Has interactuado con un NPC!";
                mensaje.SetActive(true);
                time = 0.0f;
                bloqMov = true;
                break;
            default:
                break;
        }
    }

}
