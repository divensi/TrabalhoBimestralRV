using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyController : MonoBehaviour
{
	private  static float power= 0.0f;
    private  static float scale= 1.0f;
    //Updatepower ();
    void Start(){

        power= 2.0f;
        scale= 1.0f;

    }
    public void AddScale (float valor)
    {
        scale += valor;
    }

    public  void RemoveScale (float valor)
    {
        scale -= valor;
    }

    public  float GetScale ()
    {
        return scale;
     
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
        //*scale usado para permitir na fase final com o poder de aumento de scala poder pegar objetos com grande massa.
        return power*scale;
     
    }
}
