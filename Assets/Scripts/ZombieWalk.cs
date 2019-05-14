using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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
    private float zombieDistance = 60;
    public GameObject darkAnimation;

    private IEnumerator WaitForSceneLoad() 
    {
	    yield return new WaitForSeconds(3);
	    //SceneManager.LoadScene(0); 
	    Application.LoadLevel (0);
 	}



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
        if(!walking){
        	 if (Player != null) {
	        	if(Vector3.Distance (Player.transform.position,  transform.position  ) < 2.5){ // distancia, isso permite pular e nao morrer
	        		// ToDo: adicionar animação de morte ( tela ficar escura, som de tripas sendo estouradas)
	        		//dark.animation.SetActive(true);
	        		Destroy(Player,1.0f);//trocar por tela ficar escura e som de tripas estourando
	        		//chamar a cena do menu
	        		StartCoroutine(WaitForSceneLoad());
     

	        	}
	        }	
        	
        }
        
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
