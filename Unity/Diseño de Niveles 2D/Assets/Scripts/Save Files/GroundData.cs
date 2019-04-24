using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GroundData
{
    public string groundName;
    public int id;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public GroundData(string groundName,Transform transformComponent, int id)
    {
        this.groundName = groundName.Replace("(Clone)","");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
        this.id = id;
    }
    
    public string GroundName { get; set; }
    public Transform TransformComponent { get; set; }
}
