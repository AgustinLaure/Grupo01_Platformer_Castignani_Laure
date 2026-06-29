using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button titleScreenPlayButton;
    [SerializeField] private Button titleScreenSettingsButton;
    [SerializeField] private Button titleScreenCreditsButton;
    [SerializeField] private Button titleScreenExitButton;

    private void Awake()
    {
#if UNITY_WEBGL
        titleScreenExitButton.interactable = false;
#endif

        titleScreenPlayButton.onClick.AddListener(HandlePlayButtonClick);
        titleScreenSettingsButton.onClick.AddListener(HandleSettingsButtonClick);
        titleScreenCreditsButton.onClick.AddListener(HandleCreditsButtonClick);
        titleScreenExitButton.onClick.AddListener(HandleExitButtonClick);
    }

    private void OnDestroy()
    {
        titleScreenPlayButton.onClick.RemoveListener(HandlePlayButtonClick);
        titleScreenSettingsButton.onClick.RemoveListener(HandleSettingsButtonClick);
        titleScreenCreditsButton.onClick.RemoveListener(HandleCreditsButtonClick);
        titleScreenExitButton.onClick.RemoveListener(HandleExitButtonClick);
    }

    private void HandlePlayButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void HandleSettingsButtonClick()
    {

    }

    private void HandleCreditsButtonClick()
    {

    }

    private void HandleExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
