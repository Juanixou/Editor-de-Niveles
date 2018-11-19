using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseObject : MonoBehaviour, IPointerClickHandler
{
    private InstantiateElements instanciar;
    private GameObject controlador;

    void Start()
    {
        controlador = GameObject.Find("GameController");
        instanciar = controlador.GetComponent<InstantiateElements>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.tag== "Menu")
        {
            
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            instanciar.CreateGameObject(this.name);
        }
 
    }

}
