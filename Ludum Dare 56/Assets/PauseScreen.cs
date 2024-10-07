using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    //[SerializeField] KeyCode toPauce = KeyCode.Escape;
    [SerializeField] KeyCode toRestart = KeyCode.F5;

	[SerializeField] GameObject pauseScreen;


    void Update()
    {
		if (Input.GetKeyDown(toRestart))
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pauseScreen.activeInHierarchy)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				pauseScreen.SetActive(false);
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				pauseScreen.SetActive(true);
			}
		}
	}
}
