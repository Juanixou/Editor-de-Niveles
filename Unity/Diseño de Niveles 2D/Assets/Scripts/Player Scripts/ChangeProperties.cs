using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeProperties : MonoBehaviour
{

    public GameObject windowController;

    public Slider speed;
    public Slider jumpForce;
    public Slider jumpPushForce;
    public Slider jumpWallForce;



    // Start is called before the first frame update
    void Start()
    {
        windowController = GameObject.Find("MenusController");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("PRESIONADO");
            GameObject ventana = windowController.GetComponent<ActivarVentanas>().ActivarVentanaPlayer();
            speed = ventana.transform.Find("Speed").GetComponent<Slider>();
            jumpForce = ventana.transform.Find("JumpForce").GetComponent<Slider>();
            jumpPushForce = ventana.transform.Find("JumpPushForce").GetComponent<Slider>();
            jumpWallForce = ventana.transform.Find("JumpWallForce").GetComponent<Slider>();
            ActivarObserver();
        }
    }

    //void OnMouseDown()
    //{

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Debug.Log("PRESIONADOOOO!!!");
    //        GameObject ventana = windowController.GetComponent<ActivarVentanas>().ActivarVentanaPlayer();
    //        speed = ventana.transform.Find("Speed").GetComponent<Slider>();
    //        jumpForce = ventana.transform.Find("jumpForce").GetComponent<Slider>();
    //        jumpPushForce = ventana.transform.Find("jumpPushForce").GetComponent<Slider>();
    //        jumpWallForce = ventana.transform.Find("jumpWallForce").GetComponent<Slider>();
    //        ActivarObserver();
    //    }
        
    //}

    public void ActivarObserver()
    {
        speed.onValueChanged.AddListener(delegate { SpeedChange(); });
        jumpForce.onValueChanged.AddListener(delegate { jumpForceChange(); });
        jumpPushForce.onValueChanged.AddListener(delegate { jumpPushForceChange(); });
        jumpWallForce.onValueChanged.AddListener(delegate { jumpWallForceChange(); });
    }

    // Update is called once per frame
    public void SpeedChange()
    {
        GetComponent<PlayerMovement>().speed = speed.value;
    }

    public void jumpForceChange()
    {
        GetComponent<PlayerMovement>().jumpForce = jumpForce.value;
    }
    public void jumpPushForceChange()
    {
        GetComponent<PlayerMovement>().jumpPushForce = jumpPushForce.value;
    }
    public void jumpWallForceChange()
    {
        GetComponent<PlayerMovement>().jumpWallForce = jumpWallForce.value;
    }
}
