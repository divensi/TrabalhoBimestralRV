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
    private GameObject Player; // adicione o player

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateZombieDestination", 0.0f, 1.0f);
    }
    void UpdateZombieDestination()
    {        
        if (Player != null) {
            agente.destination = Player.transform.position; 
        }

        posicaoNova = agente.nextPosition;
    }   
    private void FixedUpdate()
    {
        if (walking) {
            Debug.Log("teste");
        }
        Walking();
        animator.SetBool("Walk", walking);
        animator.SetBool("Attack", !walking);
        
    }
    private void Walking()
    {        
        if ((agente.nextPosition != agente.destination) && (posicaoAnterior != posicaoNova))
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
}
