using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public int moveType;
    public float minDistance = 50.0f;
    public float maxDistance = 500.0f;
    public float actualDistance;
    private bool dcha;

    // Start is called before the first frame update
    void Start()
    {
        moveType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case 0:
                HorizontalMovement();
                break;
            default:
                break;
        }
    }


    public void HorizontalMovement()
    {
        if (dcha)
        {
            this.transform.position = new Vector2(this.transform.position.x + 0.01f, this.transform.position.y);
            actualDistance += 0.1f;
            if (actualDistance >= maxDistance)
            {
                dcha = false;
            }
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x - 0.01f, this.transform.position.y);
            actualDistance -= 0.1f;
            if (actualDistance <= minDistance)
            {
                dcha = true;
            }
        }
    }
}
