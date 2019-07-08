using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BodyIdentification
{
    Head,
    Chest,
    Hand
}


public class HitableBodyPart : MonoBehaviour {
    [SerializeField]private BodyIdentification bodyIdentification;
    private CharaterDamage charaterDamage;
    Animator animator;
    float life;
    void Start(){
        life = 100f;
    }
    private void Awake()
    {
        charaterDamage = GetComponentInParent<CharaterDamage>();
        animator = GetComponentInParent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    { 

        if(collision != null ){
        Debug.Log("collision "+gameObject.tag);
        if(gameObject.CompareTag("chefao")){
                //collision.gameObject.
         }else{ //normal zombie kill 
            //charaterDamage.OnBeingHit(bodyIdentification);
            animator.SetBool("Walk", false);
            animator.SetBool("Attack",  false);
            animator.SetBool("Die", true);
        } 
        }
    }

    private void Die()
    {
        Destroy(gameObject,2.0f);
    }
}


