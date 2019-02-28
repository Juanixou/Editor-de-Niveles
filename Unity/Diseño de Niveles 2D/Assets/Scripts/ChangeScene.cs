using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private Scene escena;
	// Use this for initialization
	void Start () {
        escena = SceneManager.GetActiveScene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {
            switch (escena.name)
            {
                case "City":
                    SceneManager.LoadScene("Jump Trial", LoadSceneMode.Single);
                    break;
                case "Jump Trial":
                    SceneManager.LoadScene("Wall Jump Trial", LoadSceneMode.Single);
                    break;
                case "Wall Jump Trial":
                    SceneManager.LoadScene("Push Objects", LoadSceneMode.Single);
                    break;
                case "Push Objects":
                    SceneManager.LoadScene("City", LoadSceneMode.Single);
                    break;
                default:
                    break;
            }

        }
    }

}
