using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup titleScreenCanvasGroup;
    [SerializeField] private CanvasGroup creditsScreenCanvasGroup;

    [SerializeField] private Button titleScreenPlayButton;
    [SerializeField] private Button titleScreenSettingsButton;
    [SerializeField] private Button titleScreenCreditsButton;
    [SerializeField] private Button titleScreenExitButton;

    [SerializeField] private Button creditsScreenBackButton;

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
    }
    private void HandleTitlePlayButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void HandleTitleSettingsButtonClick()
    {

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
    }
}
