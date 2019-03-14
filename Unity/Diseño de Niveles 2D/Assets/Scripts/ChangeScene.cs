using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private Scene escena;
    public GameObject tecla;
    private bool colision;
    // Use this for initialization
    void Start () {
        escena = SceneManager.GetActiveScene();
        tecla = this.gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (colision)
        {
            tecla.SetActive(true);
        }
        else if(tecla!=null)
        {
            tecla.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.tag == "Player"))
        {
            colision = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                CambiarEscena();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "Player"))
        {
            colision = false;
        }
    }

    public void CambiarEscena()
    {
        tecla.SetActive(false);
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
                SceneManager.LoadScene("Damage Trial", LoadSceneMode.Single);
                break;
            case "Damage Trial":
                SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    public void SeleccionarInicio()
    {
        SceneManager.LoadScene("Inicio", LoadSceneMode.Single);
    }

    public void Mecanicas()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    public void Editor()
    {
        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }
}
