using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private PlayerEnergyController gameControl;
    public float Power= 2.0f;
	void Start() {
        gameControl= (PlayerEnergyController) GameObject.Find("Player").GetComponent(typeof(PlayerEnergyController));
        
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("colisão");
    	if(other.CompareTag("Player")){
    		gameControl.AddPower(Power);
            Destroy(gameObject);
    	}
    } 
    void Update()
    {
        
    }
}
