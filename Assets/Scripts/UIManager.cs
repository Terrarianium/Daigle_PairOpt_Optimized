using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour 
{ 
    [SerializeField] GameObject deathScreen; 
    [SerializeField] GameObject startScreen; 
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject hTP;
    [SerializeField] PlayerController playerController;

    // Use event to notify other scripts when the game is active or not instead of bool
    public delegate void ActiveGame(bool bIsActive); 
    public static event ActiveGame OnActiveGame;
    
    private void Start() 
    {
        ShowStartScreen();
    } 
    
    public void ShowStartScreen() 
    { 
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        deathScreen.SetActive(false);
        hTP.SetActive(false);
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void CloseStartScreen() 
    { 
        startScreen.SetActive(false);
        OnActiveGame?.Invoke(true); // Call active game as true
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    } 

    public void HTP()
    {
        hTP.SetActive(true);
        startScreen.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        OnActiveGame?.Invoke(false); // Call active game as false
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowDeathScreen() 
    { 
        deathScreen.SetActive(true);
        OnActiveGame?.Invoke(false);  // Call active game as false
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    } 

    public void Restart() 
    { 
        SceneManager.LoadScene("SampleScene");
    }
}