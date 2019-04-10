using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    private GameObject[] instancias;
    private GameObject player;
    public string transformacion;
    public GameObject play;
    public GameObject pause;

    // Use this for initialization
    void Start () {
        transformacion = "Move";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selection()
    {
        instancias = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject instancias in instancias){
            instancias.GetComponent<MoveObject>().SelectTransform(EventSystem.current.currentSelectedGameObject.name);
            
        }
        transformacion = EventSystem.current.currentSelectedGameObject.name;
    }

    public void Comenzar()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Rigidbody2D>().gravityScale = 3;
            player.GetComponent<MoveObject>().enabled = false;
        }
        play.SetActive(false);
        pause.SetActive(true);
    }

    public void Editar()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.GetComponent<MoveObject>().enabled = true;
        }
        play.SetActive(true);
        pause.SetActive(false);
    }

}
