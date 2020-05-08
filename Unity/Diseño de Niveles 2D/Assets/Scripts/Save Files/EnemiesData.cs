using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemiesData
{
    public string enemyName;
    public int id;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public EnemiesData(string enemyName, Transform transformComponent, int id)
    {
        this.enemyName = enemyName.Replace("(Clone)", "");
        this.position = transformComponent.position;
        this.rotation = transformComponent.rotation.eulerAngles;
        this.scale = transformComponent.localScale;
        this.id = id;
    }

    public string EnemyName { get; set; }
    public Transform TransformComponent { get; set; }

}
