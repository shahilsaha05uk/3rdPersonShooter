using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    private Scene scene;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject backButton;
    [SerializeField] private AudioSource source;

    private void OnEnable()
    {

        backButton.SetActive(false);

        mainMenuPanel.SetActive(true);
        optionPanel.SetActive(false);   
        creditsPanel.SetActive(false);
    }
    public Scene GetCurrentScene()
    {
        scene = SceneManager.GetActiveScene();
        return scene;
    }

    //Main Menu Buttons
    public void NewGameButton()
    {
        SceneManager.LoadScene("Game Copy 1", LoadSceneMode.Single);
    }
    public void OptionsButton()
    {
        mainMenuPanel.SetActive(false);
        optionPanel.SetActive(true);
        creditsPanel.SetActive(false);

        backButton.SetActive(true);
    }
    public void CreditsButton()
    {
        mainMenuPanel.SetActive(false);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(true);

        backButton.SetActive(true);

    }
    public void BackButton()
    {
        mainMenuPanel.SetActive(true);
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);

        backButton.SetActive(false);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

}
