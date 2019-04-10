using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public GameObject ventana;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarMaquina()
    {
        ventana.SetActive(true);
    }

    public void ActivarEstados()
    {
        if (count <= 3)
        {
            ventana.transform.Find(count.ToString()).gameObject.SetActive(true);
            count++;
        }
    }

    public void AsignarEscena()
    {

    }

}
