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

    void Start()
    {
        destino = Camera.main.transform;
        agente = GetComponent<NavMeshAgent>();
        agente.destination = destino.position;
        animator = GetComponent<Animator>();
        posicaoNova = agente.nextPosition;
    }

    private void FixedUpdate()
    {
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
