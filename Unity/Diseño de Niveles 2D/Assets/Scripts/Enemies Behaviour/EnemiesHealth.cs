using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    public float maxHealth = 0;
    public float currentHealth;
    public GameObject healthBar;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetChild(0).tag == "Health")
            {
                healthBar = this.transform.GetChild(i).GetChild(0).gameObject;
                GetComponent<BasicEnemyMovement>().healthBar = this.transform.GetChild(i).gameObject;
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
            this.GetComponent<BasicEnemyMovement>().enabled = false;
            
            StartCoroutine(Muerte());
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
            healthBar.transform.localScale = new Vector2(currentHealth / maxHealth, 1);
        }

    }

    IEnumerator Muerte()
    {
        //anim.SetBool("Muerte", true);
        anim.Play("Die");
        yield return new WaitForSeconds(5);
        Death();
    }
}
