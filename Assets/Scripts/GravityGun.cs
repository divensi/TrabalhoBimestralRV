using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    private void Start()
    {
        throwForce = minThrowForce;
    }

    private void Update()
    {
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
                // define o objeto como filho do Player para movimentar corretamente
                objectIHave.transform.SetParent(holdPos);

                objectRB = objectIHave.GetComponent<Rigidbody>();
                objectRB.constraints = RigidbodyConstraints.FreezeAll;
                objectRB.detectCollisions = false;

                hasObject = true;
            }
        }

    }

}