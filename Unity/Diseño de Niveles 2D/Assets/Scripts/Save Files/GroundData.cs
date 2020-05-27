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
    public Vector2 spriteSize;
    public Vector2 boxColliderSize;
    public Vector2 sidesSize;
    public Vector3 imanDchoPos;
    public Vector3 imanIzqPos;
    public Vector2 imanDchoBoxColliderSize;
    public Vector2 imanIzqBoxColliderSize;

    public GroundData(string groundName,Transform transformComponent, int id)
    {
        this.groundName = groundName.Replace("(Clone)","");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
        this.id = id;
    }
    
    public void SetSizeData(Vector2 spriteSize, Vector2 boxColliderSize, Vector2 sidesSize)
    {
        this.spriteSize = spriteSize;
        this.boxColliderSize = boxColliderSize;
        this.sidesSize = sidesSize;
    }

    public void SetImanDchoData(Vector3 imanDchoPos, Vector2 imanDchoBoxColliderSize)
    {
        this.imanDchoPos = imanDchoPos;
        this.imanDchoBoxColliderSize = imanDchoBoxColliderSize;
    }

    public void SetImanIzqData(Vector3 imanIzqPos, Vector2 imanIzqBoxColliderSize)
    {
        this.imanIzqPos = imanIzqPos;
        this.imanIzqBoxColliderSize = imanIzqBoxColliderSize;
    }


    public string GroundName { get; set; }
    public Transform TransformComponent { get; set; }
}
