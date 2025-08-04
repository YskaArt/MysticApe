using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Nombre exacto de las escenas")]
    [SerializeField] private string villageTutorialScene = "VillageTutorial";
    [SerializeField] private string mainMenuScene = "MainMenu";

    
    public void StartGame()
    {
        SceneManager.LoadScene(villageTutorialScene);
    }

  
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

 
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
