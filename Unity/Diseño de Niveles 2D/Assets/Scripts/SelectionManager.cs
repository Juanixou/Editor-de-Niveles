using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    private GameObject[] instancias;

	// Use this for initialization
	void Start () {
		
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
    }
}
