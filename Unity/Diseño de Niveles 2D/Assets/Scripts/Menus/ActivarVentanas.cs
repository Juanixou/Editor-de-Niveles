using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarVentanas : MonoBehaviour
{

    public GameObject PlayerProperties;

    public GameObject ActivarVentanaPlayer()
    {
        PlayerProperties.SetActive(true);
        return PlayerProperties;
    }
    
}
