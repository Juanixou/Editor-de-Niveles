using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectsData
{

    public string objectName;
    public int id;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Vector2 spriteSize;
    public Vector2 boxColliderSize;

    public ObjectsData(string objectName, Transform transformComponent, int id)
    {
        this.objectName = objectName.Replace("(Clone)", "");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
        this.id = id;
    }

    public void SetScaleSettings(Vector2 spriteSize, Vector2 boxColliderSize)
    {
        this.spriteSize = spriteSize;
        this.boxColliderSize = boxColliderSize;
    }

    public Vector2 GetSpriteSize() { return spriteSize; }
    public Vector2 GetColliderSize() { return boxColliderSize; }

}
