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
    private float zombieDistance = 50;
    private bool ativarCarregamento;
    private float tempoCarregamento;
    public Texture textura;
    private bool inPause;
    private Object obSpawnPoints;
    private Transform spawnPoints;

    

    private IEnumerator WaitForSceneLoad() 
    {
	    yield return new WaitForSeconds(3);
	    //SceneManager.LoadScene(0); 
	    Application.LoadLevel (0);
 	}



    void Start()
    {
        inPause = false;
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


        spawnPoints = GameObject.Find ("ZombiesSpawnPoints").transform;
        //dar tapa no zombie

    }

   

    /*private void OnCollisionEnter(Collision collision)
    {
        
        if(collision != null){
            Debug.Log("colisao");
            animator.SetBool("Die", true);
            morreu = true;
            audioSource.Stop();
            agente.isStopped = true;
            Die();
            
            }
    } */

    void UpdateZombieDestination()
    {   
        if(agente.isOnNavMesh){
        	if(Vector3.Distance (Player.transform.position,  transform.position  ) > zombieDistance){
                agente.isStopped= true;
                animator.SetBool("Idle", true);
                SpawnRandomZombie();
                Destroy(gameObject);
                return;
            }
            if (morreu){
                return;
            }
            if(agente.isStopped == true){
                agente.isStopped= false;

                }

            
            if (Player != null) {
                agente.destination = Player.transform.position; 
            }

            posicaoNova = agente.nextPosition;
            Walking();
            if (!animator.GetBool("Die")){
            animator.SetBool("Walk", walking);
            animator.SetBool("Attack", !walking);
            }
            if(!walking){
            	 if (Player != null) {
                    var distancia = 2.5;
                    if(gameObject.CompareTag("Zombie2")) distancia = 2;

    	        	if(Vector3.Distance (Player.transform.position,  transform.position  ) <= distancia && ativarCarregamento== false){ // distancia, isso permite pular e nao morrer
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
    }   

    void OnGUI(){
    	cor.a =(int)(tempoCarregamento);
    	GUI.color = cor;
    	GUI.DrawTexture(new Rect (0,0,Screen.width,Screen.height),textura);
    }


    void Update()
    {
        
         //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") || animator.GetCurrentAnimatorStateInfo(0).IsName("down")){
        if (animator.GetBool("Die") && !morreu){
            Debug.Log("MORREU");
            SpawnNewZombie();
            morreu = true;
            audioSource.Stop();
            if(agente.isOnNavMesh){
                agente.isStopped = true;
            }
            
            Die();
        } 
        
        if (ativarCarregamento == true){

        	tempoCarregamento += Time.deltaTime;

        	if(tempoCarregamento>=5){
        		ativarCarregamento = false;
        		Application.LoadLevel(0);
        	}
        }
        
        //pause sound in menu
        
        if (Time.timeScale != 1.0f && inPause == false)
         {
            inPause = true;
            audioSource.Stop();//zombie som
            audioSourceAttack.Stop(); // attack
            audioSourceGrito.Stop(); // grito

         }else if (Time.timeScale == 1.0f && inPause == true){
            
            inPause = false;
            audioSource.Play();
         }
        
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

    private void SpawnRandomZombie()
    {
        
            Transform closest = spawnPoints.GetChild(0);
            // Find the closest spawn point.

            List<Transform> transformList = new List<Transform>();  

            for (int i = 0; i < spawnPoints.childCount; ++i)
            {
                Transform thisTransform = spawnPoints.GetChild(i);

                float distanceToClosest = Vector3.Distance(closest.position, Player.transform.position);
                float distanceToThis = Vector3.Distance(thisTransform.position, Player.transform.position);


                if (distanceToThis < 40)
                {
                    transformList.Add(thisTransform);
                }
            }
            if( transformList.Count > 0){

                GameObject instance = Instantiate(Resources.Load("Zombie", typeof(GameObject))) as GameObject;
                //instance.transform.position =  closest.position;
                //Debug.Log(transformList.Count);
                closest = transformList[Random.Range(0, transformList.Count)];
                NavMeshHit hit;
                if(NavMesh.SamplePosition(new Vector3(closest.position.x+Random.Range(0.0f, 1.5f), closest.position.y+Random.Range(0.0f, 1.5f), closest.position.z+Random.Range(0.0f, 1.5f)), out hit, 2.0f, NavMesh.AllAreas)){
                    instance.transform.position= hit.position;
                }
                else{
                    //instanciar o zombie
                    instance.transform.position = new Vector3(closest.position.x+Random.Range(0.0f, 0.5f), closest.position.y + 0.4f+Random.Range(0.0f, 0.2f), closest.position.z +Random.Range(0.0f, 0.5f));
                    //transform.position = new Vector3(transform.position.x+Random.Range(0.0f, 0.5f), transform.position.y+Random.Range(0.0f, 0.5f), transform.position.z+Random.Range(0.0f, 0.5f)); 
                    
                }
            }
        }

        
        



    private void SpawnNewZombie()
    {

            Transform closest = spawnPoints.GetChild(0);
            // Find the closest spawn point.
            for (int i = 0; i < spawnPoints.childCount; ++i)
            {
                Transform thisTransform = spawnPoints.GetChild(i);
                float distanceToClosest = Vector3.Distance(closest.position, Player.transform.position);
                float distanceToThis = Vector3.Distance(thisTransform.position, Player.transform.position);

                if (distanceToThis < distanceToClosest)
                {
                    closest = thisTransform;
                }
            }
            GameObject instance;
            NavMeshHit hit;

            if(gameObject.CompareTag("Zombie")){
                instance = Instantiate(Resources.Load("Zombie", typeof(GameObject))) as GameObject;
            } 
            else{
                instance = Instantiate(Resources.Load("meltyzombie", typeof(GameObject))) as GameObject;
                instance.transform.localScale +=  new Vector3(1.0f,1.0f,1.0f);
            } 

            
            if(NavMesh.SamplePosition(new Vector3(closest.position.x, closest.position.y, closest.position.z), out hit, 1.0f, NavMesh.AllAreas)){
                instance.transform.position= hit.position;
            }
            else{

                instance.transform.position = new Vector3(closest.position.x, closest.position.y + 0.4f, closest.position.z);
            }
            //instance.transform.position =  closest.position;
            
            //instanciar o zombie

        }


}
