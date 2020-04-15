using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform player;       //Public variable to store a reference to the player game object

    public float cameraDistance = 30.0f;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    // Use this for initialization
    //void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    //Calculate and store the offset value by getting the distance between the player's position and camera's position.
    //    offset = transform.position - player.transform.position;
    //}

    //// LateUpdate is called after Update each frame
    //void LateUpdate()
    //{
    //    // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
    //    transform.position = player.transform.position + offset;
    //}
}
