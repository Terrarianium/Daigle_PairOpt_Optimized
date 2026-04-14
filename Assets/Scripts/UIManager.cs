using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour 
{ 
    [SerializeField] GameObject deathScreen; 
    [SerializeField] GameObject startScreen; 
    [SerializeField] GameObject winScreen;
    [SerializeField] PlayerController playerController; 
    public bool startGame; 
    
    private void Start() 
    {
        ShowStartScreen();
    } 
    
    public void ShowStartScreen() 
    { 
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        deathScreen.SetActive(false); 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void CloseStartScreen() 
    { 
        startScreen.SetActive(false); 
        startGame = true; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    } 

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        startGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowDeathScreen() 
    { 
        deathScreen.SetActive(true); 
        startGame = false; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void Restart() 
    { 
        SceneManager.LoadScene("SampleScene");
    }
}