using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GackgroundManager : MonoBehaviour
{

    private GameObject instancia;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Destroy(GameObject.FindGameObjectWithTag("Background"));
        instancia = Instantiate((GameObject)Resources.Load("prefabs/" + this.name, typeof(GameObject)));
        instancia.transform.SetParent(GameObject.Find("Objects").transform, false);
        UpdateBackgroundTile(Camera.main.transform.position.x, 0.0f, instancia);
        GameObject.Find("DataController").GetComponent<SaveGround>().InsertGround(instancia);
    }

    void UpdateBackgroundTile(float newPosX, float lastPosX, GameObject bckgrd)
    {

        float xScale = (newPosX - lastPosX);

        if (bckgrd == null) return;
        foreach (SpriteRenderer child in bckgrd.GetComponentsInChildren<SpriteRenderer>())
        {
            child.size = new Vector2(child.size.x + xScale, child.size.y);
        }

    }
}
