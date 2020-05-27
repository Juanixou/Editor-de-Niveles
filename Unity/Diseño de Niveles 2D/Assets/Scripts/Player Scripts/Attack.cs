using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private Animator anim;
    public Damage weapon;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K)&& GetComponent<PlayerMovement>().grounded)
        {
            weapon.isAttacking = true;
            anim.SetBool("Atacar", true);
        }
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1H"))
        {
            anim.SetBool("Atacar", false);
        }

    }

}
