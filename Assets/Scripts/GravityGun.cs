using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GravityGun : MonoBehaviour
{
    public Camera cam;
    public float interactDist;

    public Transform holdPos;
    public float attractSpeed;

    public float minThrowForce;
    public float maxThrowForce;
    private float throwForce;

    private GameObject objectIHave;
    private Rigidbody objectRB;

    private bool hasObject = false;
    public Canvas CanvasObject;
    private float actualScale;

    private PlayerEnergyController gameControl;

    private IEnumerator WaitForObstacleActive(NavMeshObstacle navmeshobs) 
    {
        yield return new WaitForSeconds(1);
        navmeshobs.enabled= true; 
    
    }


    private void Start()
    {
        throwForce = minThrowForce;
        CanvasObject.GetComponent<Canvas>().enabled = false;
        gameControl = GetComponent(typeof(PlayerEnergyController)) as  PlayerEnergyController;
        actualScale = gameControl.GetScale();
    }

    private void Update()
    {
     float scale =  gameControl.GetScale();
    if (actualScale != scale ){
            float difScale = scale - actualScale;
            transform.localScale +=  new Vector3(difScale,difScale,difScale);
            actualScale = scale;
        
    }
    //Screen.showCursor = false;
    //Screen.lockCursor = true;
     if(Input.GetKeyDown("escape"))     
     {
                 
         if (Time.timeScale == 1.0f)
         {            
             Time.timeScale = 0.0f;
             //Screen.showCursor = true;
             //Screen.lockCursor = false;
             CanvasObject.GetComponent<Canvas>().enabled = true;
         }       
             
         else
         {
             CanvasObject.GetComponent<Canvas>().enabled = false;
             Time.timeScale = 1.0f;    
             //Screen.showCursor = false;
             //Screen.lockCursor = true;                    
         }
         
     }
        if (Input.GetMouseButtonDown(1) && !hasObject)
        {
            DoRay();
        } 
        else if (Input.GetMouseButtonDown(1) && hasObject)
        {
            ReleaseObject();
        }

        if (Input.GetMouseButton(0) && hasObject)
        {
            throwForce += 0.1f;
        } 
        else if (Input.GetMouseButtonUp(0) && hasObject)
        {
            ShootObj();
        }


        if (hasObject)
        {
            // RotateObj();

            if(CheckDist() >= 1f)
            {
                MoveObjToPos();
            }
        }



    }


    //----------------Functinoal Stuff

    public float CheckDist()
    {
        float dist = Vector3.Distance(objectIHave.transform.position, holdPos.transform.position);
        return dist;
    }

    private void MoveObjToPos()
    {
        objectIHave.transform.position = Vector3.Lerp(objectIHave.transform.position, holdPos.position, attractSpeed * Time.deltaTime);
    }

    private void ReleaseObject()
    {
        objectRB.constraints = RigidbodyConstraints.None;
        var navmeshobs= objectIHave.GetComponent<NavMeshObstacle>();
        StartCoroutine(WaitForObstacleActive(navmeshobs));

        
        objectIHave.transform.parent = null;
        objectIHave = null;
        objectRB.detectCollisions = true;
        

        hasObject = false;
    }

    private void ShootObj()
    {
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);
        objectRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
        throwForce = minThrowForce;
        ReleaseObject();
    }

    // Traceja a linha de visão do player e vê se o objeto é "Pegável"
    private void DoRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDist))
        {
            if (hit.collider.CompareTag("Pegavel"))
            {

                objectIHave = hit.collider.gameObject;
                objectRB = objectIHave.GetComponent<Rigidbody>();
                var objectMass = objectRB.mass;
                if(objectMass <= gameControl.GetPower()){
                    gameControl.RemovePower(objectMass);
                    
                    // define o objeto como filho do Player para movimentar corretamente
                    objectIHave.transform.SetParent(holdPos);
                    // desabilita o NavMeshObstacle evitando bug de desvio do zombie
                    var navmeshobs= objectIHave.GetComponent<NavMeshObstacle>();
                    navmeshobs.enabled= false;
                    

                    objectRB.constraints = RigidbodyConstraints.FreezeAll;
                    objectRB.detectCollisions = false;

                    hasObject = true;
                }else{
                    Debug.Log("erro falta de energia");
                }
            }
        }

    }

}