using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth=500;
    public float currentHealth;
    private Scene escena;
    public GameObject healthBar;
    private GameObject greenBar;
    private Transform greenHealth;


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < this.transform.childCount; i++)
        {

            //This condition look for de health bar
            if (this.transform.GetChild(i).name == "HealthBar")
            {
                healthBar = this.transform.GetChild(i).gameObject;
                for(int j = 0; j < healthBar.transform.childCount; j++)
                {
                    if(healthBar.transform.GetChild(j).tag == "Health")
                    {
                        greenBar = healthBar.transform.GetChild(j).gameObject;
                    }
                }
            }
            

        }

        currentHealth = maxHealth;
        escena = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            if (GameObject.Find("SelectionController") != null)
            {
                GameObject.Find("SelectionController").GetComponent<SelectionManager>().Editar();
                return;
            }
            Death();
        }
    }

    public void Death()
    {
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }

    public void Damage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth >= 0 && greenBar != null)
        {
            
            greenBar.transform.localScale = new Vector2(currentHealth / maxHealth, greenBar.transform.localScale.y);
        }

    }

    public void RotateHealthBar(Quaternion rotationQuantity)
    {
        healthBar.transform.rotation = rotationQuantity;
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;
        greenBar.transform.localScale = new Vector2(currentHealth / maxHealth, greenBar.transform.localScale.y);
    }

    public void EmptyHealth()
    {
        currentHealth = 0.0f;
    }

}
