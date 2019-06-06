using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name;
    public Vector3 position;
    public float speed;
    public float jumpForce;
    public float jumpPushForce;
    public float jumpWallForce;
    public float health;

    public PlayerData(string name, Transform transf, float speed, float jump, float jumpPush, float jumpWall, float health) 
    {
        this.name = name;
        this.position = transf.position;
        this.speed = speed;
        this.jumpForce = jump;
        this.jumpPushForce = jumpPush;
        this.jumpWallForce = jumpWall;
        this.health = health;
    }

    public string Name { get; set; }
    public Transform TransformComponent { get; set; }

}
