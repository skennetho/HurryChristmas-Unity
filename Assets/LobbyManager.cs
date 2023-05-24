using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public ParticleSystem SnowParticle;
    public Button PlayButton;
    public Button ExitButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(OnPlayGame);
        ExitButton.onClick.AddListener(OnExitGame);
        SnowParticle.Play();
    }

    private void InitializeWindowSetting()
    {
        // set full screen
    }

    public void OnPlayGame()
    {
        SceneManager.LoadScene("InGame",LoadSceneMode.Single);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
