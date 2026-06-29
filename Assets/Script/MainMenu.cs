using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup titleScreenCanvasGroup;
    [SerializeField] private CanvasGroup creditsScreenCanvasGroup;
    [SerializeField] private CanvasGroup settingsScreenCanvasGroup;

    [SerializeField] private Button titleScreenPlayButton;
    [SerializeField] private Button titleScreenSettingsButton;
    [SerializeField] private Button titleScreenCreditsButton;
    [SerializeField] private Button titleScreenExitButton;

    [SerializeField] private Button creditsScreenBackButton;
    [SerializeField] private Button settingsScreenBackButton;

    private void Awake()
    {
#if UNITY_WEBGL
        titleScreenExitButton.interactable = false;
#endif

        titleScreenPlayButton.onClick.AddListener(HandleTitlePlayButtonClick);
        titleScreenSettingsButton.onClick.AddListener(HandleTitleSettingsButtonClick);
        titleScreenCreditsButton.onClick.AddListener(HandleTitleCreditsButtonClick);
        titleScreenExitButton.onClick.AddListener(HandleTitleExitButtonClick);
        creditsScreenBackButton.onClick.AddListener(HandleCreditsBackButtonClick);
        settingsScreenBackButton.onClick.AddListener(HandleSettingsBackButtonClick);
    }
    private void HandleTitlePlayButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void HandleTitleSettingsButtonClick()
    {
        UiUtils.SetCanvasActive(titleScreenCanvasGroup, false);
        UiUtils.SetCanvasActive(settingsScreenCanvasGroup, true);
    }

    private void HandleTitleCreditsButtonClick()
    {
        UiUtils.SetCanvasActive(titleScreenCanvasGroup, false);
        UiUtils.SetCanvasActive(creditsScreenCanvasGroup, true);
    }

    private void HandleCreditsBackButtonClick()
    {
        UiUtils.SetCanvasActive(titleScreenCanvasGroup, true);
        UiUtils.SetCanvasActive(creditsScreenCanvasGroup, false);
    }

    private void HandleSettingsBackButtonClick()
    {
        UiUtils.SetCanvasActive(titleScreenCanvasGroup, true);
        UiUtils.SetCanvasActive(settingsScreenCanvasGroup, false);
    }

    private void HandleTitleExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        titleScreenPlayButton.onClick.RemoveListener(HandleTitlePlayButtonClick);
        titleScreenSettingsButton.onClick.RemoveListener(HandleTitleSettingsButtonClick);
        titleScreenCreditsButton.onClick.RemoveListener(HandleTitleCreditsButtonClick);
        titleScreenExitButton.onClick.RemoveListener(HandleTitleExitButtonClick);
        creditsScreenBackButton.onClick.RemoveListener(HandleCreditsBackButtonClick);
        settingsScreenBackButton.onClick.RemoveListener(HandleSettingsBackButtonClick);
    }
}
