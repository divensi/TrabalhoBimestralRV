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
    private AudioSource audioSourceAttack;
    private AudioSource audioSourceGrito;
    private Color cor = Color.black;
    private float zombieDistance = 30;
    private bool ativarCarregamento;
    private float tempoCarregamento;
    public Texture textura;
    

    private IEnumerator WaitForSceneLoad() 
    {
	    yield return new WaitForSeconds(3);
	    //SceneManager.LoadScene(0); 
	    Application.LoadLevel (0);
 	}



    void Start()
    {
        morreu = false;
        tempoCarregamento = 0.0f;
        ativarCarregamento=false;
        Player = GameObject.FindGameObjectWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        var audioSources = GetComponents<AudioSource>();
        InvokeRepeating("UpdateZombieDestination", 0.0f, 0.3f);
        audioSource = audioSources[0];//zombie som
        audioSourceAttack = audioSources[1]; // attack
        audioSourceGrito = audioSources[2]; // grito

        
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
	        	if(Vector3.Distance (Player.transform.position,  transform.position  ) <= 2.5 && ativarCarregamento== false){ // distancia, isso permite pular e nao morrer
	        		// ToDo: adicionar animação de morte ( tela ficar escura, som de tripas sendo estouradas)
	        		audioSource.Stop();
 					audioSourceAttack.Play();
 					audioSourceGrito.Play();
	        		ativarCarregamento = true;
	        		//chamar a cena do menu
	        		//StartCoroutine(WaitForSceneLoad());
     

	        	}
	        }	
        	
        }
        
    }   

    void OnGUI(){
    	cor.a =(int)(tempoCarregamento);
    	GUI.color = cor;
    	GUI.DrawTexture(new Rect (0,0,Screen.width,Screen.height),textura);
    }


    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die")){
            morreu = true;
            audioSource.Stop();
            agente.isStopped = true;
            Die();
        }
        
        if (ativarCarregamento == true){

        	tempoCarregamento += Time.deltaTime;

        	if(tempoCarregamento>=5){
        		ativarCarregamento = false;
        		Application.LoadLevel (0);
        	}
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
