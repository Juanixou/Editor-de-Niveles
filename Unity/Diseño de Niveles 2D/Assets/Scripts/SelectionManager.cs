﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    private GameObject[] instancias;
    private GameObject player;
    public string transformacion;
    public GameObject play;
    public GameObject pause;
    Transform canvas;

    private Vector3 playerPosition;
    private Vector3 cameraPosition;

    // Use this for initialization
    void Start () {
        transformacion = "Move";
        canvas = GameObject.Find("Canvas").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selection()
    {

        foreach (Transform instancias in canvas)
        {
            if (instancias.gameObject.name.Contains("Clone"))
            {
                instancias.GetComponent<MoveObject>().SelectTransform(EventSystem.current.currentSelectedGameObject.name);
            } 
        }
        transformacion = EventSystem.current.currentSelectedGameObject.name;
    }

    public void Comenzar()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        playerPosition = player.transform.position;
        cameraPosition = Camera.main.transform.position;
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Rigidbody2D>().gravityScale = 3;
            player.GetComponent<MoveObject>().enabled = false;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<MoveObject>().enabled = false;
            if (enemy.GetComponent<BasicEnemyMovement>() != null) enemy.GetComponent<BasicEnemyMovement>().enabled = true;
            if (enemy.GetComponent<MyRayCast>() != null) enemy.GetComponent<MyRayCast>().enabled = true;
            if (enemy.GetComponent<MyDistanceAttack>() != null) enemy.GetComponent<MyDistanceAttack>().enabled = true;
        }
        Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().enabled = true;
        play.SetActive(false);
        pause.SetActive(true);
    }

    public void Editar()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerPosition;
        Camera.main.transform.position = cameraPosition;
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.GetComponent<MoveObject>().enabled = true;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<MoveObject>().enabled = true;
            if (enemy.GetComponent<BasicEnemyMovement>() != null) enemy.GetComponent<BasicEnemyMovement>().enabled = false;
            if (enemy.GetComponent<MyRayCast>() != null) enemy.GetComponent<MyRayCast>().enabled = false;
            if (enemy.GetComponent<MyDistanceAttack>() != null) enemy.GetComponent<MyDistanceAttack>().enabled = false;
        }
        Camera.main.GetComponent<UnityStandardAssets._2D.Camera2DFollow>().enabled = false;
        play.SetActive(true);
        pause.SetActive(false);
    }

}
