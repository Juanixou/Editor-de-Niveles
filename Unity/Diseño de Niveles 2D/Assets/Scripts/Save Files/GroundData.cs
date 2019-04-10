using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GroundData
{
    public string groundName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public GroundData(string groundName,Transform transformComponent)
    {
        this.groundName = groundName.Replace("(Clone)","");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
    }
    
    public string GroundName { get; set; }
    public Transform TransformComponent { get; set; }
}
