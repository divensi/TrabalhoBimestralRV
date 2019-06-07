using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieWalk2 : MonoBehaviour
{

    private Transform destino;
    private NavMeshAgent agente;
    private bool walking = false;
    private bool notwalk = true;
    Animator animator;
    private Vector3 posicaoAnterior;
    private Vector3 posicaoNova;
    public GameObject Player; // adicione o player
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateZombieDestination", 0.0f, 1.0f);
    }
    void UpdateZombieDestination()
    {        
        //agente.destination = Player.transform.position; //discomente isso
        agente.destination = Camera.main.transform.position;//comente isso 
        posicaoNova = agente.nextPosition;
    }   
    private void FixedUpdate()
    {
        Walking();
        animator.SetBool("run", walking);
        animator.SetBool("attack", !walking);
        
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
