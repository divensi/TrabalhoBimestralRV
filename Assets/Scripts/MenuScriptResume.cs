using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScriptResume : MonoBehaviour
{
    public Canvas CanvasObject;
    private AudioSource audioSource;
    private bool inPause;
    void Start () {
 
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None; 
         audioSource = GetComponent<AudioSource>();
         inPause = false;

     }

    public void Update(){
        if(CanvasObject.enabled &&  inPause == false){
            inPause = true;
            audioSource.Play();
        }else if(!CanvasObject.enabled && inPause == true)  {
            inPause = false;
            audioSource.Stop();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

   


    }

    public void Sair() 
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Iniciar(int cena)
    {
        //SceneManager.LoadScene(cena,LoadSceneMode.Additive);
        CanvasObject.GetComponent<Canvas> ().enabled = false;
        Time.timeScale = 1.0f; 

    }
}
