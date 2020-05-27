using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    public float maxHealth = 0;
    public float currentHealth;
    public GameObject healthBar;
    private GameObject greenBar;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {


        anim = GetComponentInChildren<Animator>();
        for (int i = 0; i < this.transform.childCount; i++)
        {

            //This condition look for de health bar
            if (this.transform.GetChild(i).name == "HealthBar")
            {
                healthBar = this.transform.GetChild(i).gameObject;
                for (int j = 0; j < healthBar.transform.childCount; j++)
                {
                    if (healthBar.transform.GetChild(j).tag == "Health")
                    {
                        greenBar = healthBar.transform.GetChild(j).gameObject;
                    }
                }
            }


        }
        currentHealth = maxHealth;
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

            //if (this.GetComponent<Rigidbody2D>() != null) this.GetComponent<Rigidbody2D>().isKinematic = true;
            if (this.GetComponent<BasicEnemyMovement>() != null) this.GetComponent<BasicEnemyMovement>().enabled = false;
            if (this.GetComponent<MyDistanceAttack>() != null) this.GetComponent<MyDistanceAttack>().enabled = false;
            if (this.GetComponent<MyRayCast>() != null) this.GetComponent<MyRayCast>().enabled = false;
            if (this.GetComponent<Damage>() != null) this.GetComponent<Damage>().enabled = false;
            if (this.GetComponent<BoxCollider2D>()) this.GetComponent<BoxCollider2D>().enabled = false;
            if (GameObject.Find("SelectionController") != null)
            {
                this.gameObject.SetActive(false);
                currentHealth = maxHealth;
                greenBar.transform.localScale = Vector2.one;
                return;
            }
            else
            {
                StartCoroutine(Muerte());
            }

            /*
            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {

            }
            */
            
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);

    }

    public void Damage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth >= 0)
        {
            greenBar.transform.localScale = new Vector2(currentHealth / maxHealth, 1);
        }

    }

    IEnumerator Muerte()
    {
        anim.SetBool("Muerte", true);
        if (this.transform.GetChild(this.transform.childCount - 1).name.Equals("FeetCollider")) this.transform.GetChild(this.transform.childCount - 1).gameObject.SetActive(false);
        greenBar.transform.parent.transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        Death();
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
