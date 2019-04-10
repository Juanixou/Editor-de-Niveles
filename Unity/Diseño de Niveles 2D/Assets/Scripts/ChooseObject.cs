using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseObject : MonoBehaviour, IPointerClickHandler
{
    private InstantiateElements instanciar;
    private GameObject controlador;
    private List<GameObject> listaActivos;

    void Start()
    {
        listaActivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Menu"));
        controlador = GameObject.Find("InstancesController");
       // instanciar = controlador.GetComponent<InstantiateElements>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(this.tag== "Menu")
        {
            foreach(GameObject activo in listaActivos)
            {
                if(activo.name != this.name)
                {
                    foreach (Transform child in activo.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            //instanciar.CreateGameObject(this.name, this.transform);
        }
 
    }

}
