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
    private Transform greenHealth;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("Health");
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
        Debug.Log(currentHealth);
        Debug.Log(currentHealth/maxHealth);
        healthBar.transform.localScale = new Vector2(currentHealth / maxHealth, 1);
    }
}
