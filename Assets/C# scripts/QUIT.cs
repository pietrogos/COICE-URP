using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUIT : MonoBehaviour
{
    // Start is called before the first frame update
    void QuitGame()
    {
        Debug.Log ("quit"); 
        Application.Quit ();
    }

  
}
