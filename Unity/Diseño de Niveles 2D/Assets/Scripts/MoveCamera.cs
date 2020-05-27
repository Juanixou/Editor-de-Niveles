using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public float speed;
    private GameObject instancias;
    private GameObject transformaciones;
    private float oldPosX;
    // Use this for initialization
    void Start()
    {
        instancias = GameObject.Find("Instancias");
        transformaciones = GameObject.Find("Transformaciones");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            oldPosX = transform.position.x;
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            UpdateBackgroundTile(transform.position.x, oldPosX);
        }
        if (Input.GetKey(KeyCode.LeftArrow)&&transform.position.x>=0)
        {
            oldPosX = transform.position.x;
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            UpdateBackgroundTile(transform.position.x, oldPosX);
        }

    }



    void UpdateBackgroundTile(float newPosX, float lastPosX)
    {

        float xScale = (newPosX - lastPosX);
        GameObject bckgrd = GameObject.FindGameObjectWithTag("Background");
        if (bckgrd == null) return;
        foreach (SpriteRenderer child in bckgrd.GetComponentsInChildren<SpriteRenderer>())
        {
            child.size = new Vector2(child.size.x + xScale, child.size.y);
        }

    }

}
