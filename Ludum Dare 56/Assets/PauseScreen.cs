using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    //[SerializeField] KeyCode toPauce = KeyCode.Escape;
    [SerializeField] KeyCode toRestart = KeyCode.F5;
    
    void Update()
    {
		if (Input.GetKeyDown(toRestart))
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
    }
}
