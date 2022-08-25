using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    string mouseHoverSound = "ButtonHover";
    [SerializeField]
    string buttonPressSound = "ButtonPress";

    private AudioManager audioManager;


    void Start()
    {
        //caching sounds
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("GameOverUI: No AudioManager found in the scene!");
        }
    }


    public void Quit()
    {
        audioManager.PlaySound(buttonPressSound);

        Debug.Log("Game Quitted.");
        Application.Quit();
    }


    public void Retry()
    {
        audioManager.PlaySound(buttonPressSound);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
