using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public float damage=20;
    public bool isAttacking;
    private bool damaged;


    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        damaged = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {

        switch (other.tag)
        {

            case "Player":
                if (damaged)
                {
                    other.GetComponent<PlayerStats>().Damage(damage);
                    damaged = false;
                    StartCoroutine(WaitDamage());
                }

                break;

            case "Enemy":
                Debug.Log("Enemigo Dañado!");
                if (isAttacking && this.tag != "EnemyMeleeWeapon")
                {
                    other.GetComponent<EnemiesHealth>().Damage(damage);
                    isAttacking = false;
                    StartCoroutine(WaitDamage());
                }

                break;
            default:
                break;
        }
    }

    IEnumerator WaitDamage()
    {
        yield return new WaitForSeconds(2);
        damaged = true;
    }

}
