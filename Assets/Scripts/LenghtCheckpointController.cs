using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenghtCheckpointController : MonoBehaviour
{
    private PlayerEnergyController gameControl;
    public float Scale= 0.5f;
    private float spinSpeed = 70.0f;
	void Start() {
        gameControl= (PlayerEnergyController) GameObject.Find("Player").GetComponent(typeof(PlayerEnergyController));
        
    }

    void OnTriggerEnter(Collider other){
        //Debug.Log("colisão");
    	if(other.CompareTag("Player")){
    		gameControl.AddScale(Scale);
            Destroy(gameObject);
    	}
    } 
    void Update()
    {
        transform.Rotate(0,0,spinSpeed*Time.deltaTime);
    }
}
