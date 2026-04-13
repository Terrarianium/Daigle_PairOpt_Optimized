using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour 
{ 
    [SerializeField] GameObject deathScreen; 
    [SerializeField] GameObject startScreen; 
    [SerializeField] PlayerController playerController; 
    public bool startTimer; private void Start() 
    { 
        ShowStartScreen();
        //playerController = Object.FindFirstObjectByType<PlayerController>();
    } 
    
    public void ShowStartScreen() 
    { 
        startScreen.SetActive(true); 
        deathScreen.SetActive(false); 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void CloseStartScreen() 
    { 
        startScreen.SetActive(false); 
        startTimer = true; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    } 

    public void ShowDeathScreen() 
    { 
        deathScreen.SetActive(true); 
        startTimer = false; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void Restart() 
    { 
        SceneManager.LoadScene("SampleScene");
    }
}