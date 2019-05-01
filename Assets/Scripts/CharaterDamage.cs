using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterDamage : MonoBehaviour {
    //Animator animator;
    public Action<BodyIdentification> BeingHitEvent;
    public void OnBeingHit(BodyIdentification bodyIdentification)
    {
        if(BeingHitEvent != null)
        {
            BeingHitEvent(bodyIdentification);
            //animator.SetBool("Die",true);
        }
    }
}
