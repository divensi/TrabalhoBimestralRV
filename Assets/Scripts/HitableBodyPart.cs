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
    
    
    private void Awake()
    {
        charaterDamage = GetComponentInParent<CharaterDamage>();
        animator = GetComponentInParent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        charaterDamage.OnBeingHit(bodyIdentification);
        animator.SetBool("Die", true);
    }

    private void Die()
    {
        Destroy(gameObject,2.0f);
    }
}


