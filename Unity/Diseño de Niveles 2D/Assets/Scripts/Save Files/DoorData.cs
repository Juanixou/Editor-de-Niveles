using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DoorData
{
    public string doorName;
    public int id;
    public int postId;
    public string postScene;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public DoorData(string doorName, Transform transformComponent, int id)
    {
        this.doorName = doorName.Replace("(Clone)", "");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
        this.id = id;
        this.postScene = "";
        this.postId = -1;
    }

    public string DoorName { get; set; }
    public string PostScene { get; set; }
    public int PostId { get; set; }
    public Transform TransformComponent { get; set; }
}
