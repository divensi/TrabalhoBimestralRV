using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyController : MonoBehaviour
{
	private  static float power= 0.0f;
    //Updatepower ();
    void Start(){

        power= 2.0f;

    }
    public  void AddPower (float valor)
    {
        power += valor;
    }

    public  void RemovePower (float valor)
    {
        power -= valor;
    }
  	public  float GetPower ()
    {
        return power;
     
    }
}
