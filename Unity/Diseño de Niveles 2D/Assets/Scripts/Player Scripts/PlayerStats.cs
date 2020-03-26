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

        //TO-DO: Coger el Health Bar del hijo, no uno genérico
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
            Debug.Log(currentHealth);
        }

    }

    public void RotateHealthBar(Quaternion rotationQuantity)
    {
        healthBar.transform.rotation = rotationQuantity;
    }

}
