using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MuerteCaida : MonoBehaviour
{

    GameObject selectionController;
    // Start is called before the first frame update
    void Start()
    {
        selectionController = GameObject.Find("SelectionController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObject.Find("SelectionController") != null)
            {
                //GameObject.Find("SelectionController").GetComponent<SelectionManager>().Editar();
                other.GetComponent<PlayerStats>().EmptyHealth();
            }
            else
            {
                SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
            }
            
        }
        else
        {
            if (GameObject.Find("SelectionController") != null)
            {
                other.gameObject.SetActive(false);
                if (other.gameObject.GetComponent<BasicEnemyMovement>() != null) other.gameObject.GetComponent<BasicEnemyMovement>().enabled = false;
                if (other.gameObject.GetComponent<MyDistanceAttack>() != null) other.gameObject.GetComponent<MyDistanceAttack>().enabled = false;
                if (other.gameObject.GetComponent<MyRayCast>() != null) other.gameObject.GetComponent<MyRayCast>().enabled = false;
                if (other.gameObject.GetComponent<Damage>() != null) other.gameObject.GetComponent<Damage>().enabled = false;
                if (other.gameObject.GetComponent<BoxCollider2D>()) other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                Destroy(other.gameObject);
            }
            
        }
        
    }
}
