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

    private void Start()
    {
        //origen = new Vector3(0, 0, 0);

    }

    public void CreateGameObject(string type)
    {
        origen = Camera.main.transform.position;
        origen.z = 0;
        switch (type)
        {
            case "Character":
                player = Instantiate(Character);
                player.transform.position = origen;
                break;
            case "Ground":
                GameObject suelo = Instantiate(ground);
                suelo.transform.position = origen;
                break;
        }
    }
}
