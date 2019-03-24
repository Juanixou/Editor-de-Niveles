using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstantiateElements : MonoBehaviour
{

    public GameObject ground;
    public GameObject Character;
    private GameObject player;
    Vector3 origen;
    private bool playerCreated;

    private void Start()
    {
        //origen = new Vector3(0, 0, 0);
        playerCreated = false;
    }

    public void CreateGameObject(string type, Transform pos)
    {
        origen = Camera.main.transform.position;
        origen.z = 0;
        switch (type)
        {
            case "Character":
                if (!playerCreated)
                {
                    player = Instantiate(Character);
                    player.transform.parent = GameObject.Find("Canvas").transform;
                    player.transform.position = origen;
                    player.GetComponent<Rigidbody2D>().gravityScale = 0;
                    player.GetComponent<PlayerMovement>().enabled = false;
                    player.GetComponent<MoveObject>().enabled = true;
                    playerCreated = true;
                }

                break;
            case "Ground":
                GameObject suelo = Instantiate(ground);
                suelo.transform.parent = GameObject.Find("Canvas").transform;
                suelo.transform.position = pos.position;
                break;
        }
    }
}
