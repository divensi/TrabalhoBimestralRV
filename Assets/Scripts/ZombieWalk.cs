using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieWalk : MonoBehaviour
{

    private Transform destino;
    private NavMeshAgent agente;
    private bool walking = false;
    private bool notwalk = true;
    Animator animator;
    private Vector3 posicaoAnterior;
    private Vector3 posicaoNova;
    private GameObject Player; 
    private bool morreu;
    private AudioSource audioSource;
    private float zombieDistance = 25.0;

    void Start()
    {
        morreu = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateZombieDestination", 0.0f, 0.3f);
        
    }
    void UpdateZombieDestination()
    {   
       if(Vector3.Distance (Player.transform.position,  transform.position  ) > zombieDistance){
            agente.isStopped= true;
            animator.SetBool("Idle", true);
            return;
        }
        if(agente.isStopped == true){
            agente.isStopped= false;

            }
        if (morreu){
            return;
        }
        if (Player != null) {
            agente.destination = Player.transform.position; 
        }

        posicaoNova = agente.nextPosition;
        Walking();
        animator.SetBool("Walk", walking);
        animator.SetBool("Attack", !walking);
        
    }   
    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die")){
            morreu = true;
            audioSource.Stop();
            agente.isStopped = true;
            Die();
        }
        
        //UpdateZombieDestination();
        
    }
    private void Walking()
    {   
        if ((agente.nextPosition != Player.transform.position) && (posicaoAnterior != posicaoNova))
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
        posicaoAnterior = posicaoNova;
        posicaoNova = agente.nextPosition;
    }
    private void Die()
    {
        Destroy(gameObject,5.0f);
    }

}
