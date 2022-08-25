using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;



    void Start ()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("MenuManager: No AudioManager found!");
        }
    }


    public void StartGame()
    {
        audioManager.PlaySound(pressButtonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //This will load the 'next' scene (the Main Scene of the game)
        audioManager.StopSound("MainMenuMusic");
        audioManager.PlaySound("MainLevelMusic");
    }


    public void QuitGame()
    {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("Game Quitted.");
        Application.Quit();
    }


    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }



}
