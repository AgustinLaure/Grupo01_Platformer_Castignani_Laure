using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject winCondition;

    [SerializeField] private CanvasGroup pauseScreenCanvasGroup;
    [SerializeField] private CanvasGroup endScreenCanvasGroup;
    [SerializeField] private Button pauseScreenResumeButton;
    [SerializeField] private Button pauseScreenMainMenuButton;
    [SerializeField] private Button endScreenResumeButton;
    [SerializeField] private Button endScreenMainMenuButton;

    private bool isPaused = false;

    private AreaTrigger winConditionTrigger;
    private PlayerController playerController;

    public bool GetIsPaused { get { return isPaused; } }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        winConditionTrigger = winCondition.GetComponent<AreaTrigger>();

        playerController.OnPlayerPause += HandlePlayerPause;
        player.GetHealthPoints.OnDie += HandlePlayerDeath;
        winConditionTrigger.OnTrigger += HandleWinTrigger;

        pauseScreenResumeButton.onClick.AddListener(HandleResumeButtonClick);
        pauseScreenMainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
        endScreenResumeButton.onClick.AddListener(HandleResumeButtonClick);
        endScreenMainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
    }

    private void OnDestroy()
    {
        playerController.OnPlayerPause -= HandlePlayerPause;
        player.GetHealthPoints.OnDie -= HandlePlayerDeath;
        winConditionTrigger.OnTrigger -= HandleWinTrigger;

        pauseScreenResumeButton.onClick.RemoveListener(HandleResumeButtonClick);
        pauseScreenMainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
        endScreenResumeButton.onClick.RemoveListener(HandleResumeButtonClick);
        endScreenMainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
    }

    private void SetPause(bool state)
    {
        UiUtils.SetCanvasActive(pauseScreenCanvasGroup, state);

        Time.timeScale = state ? 0f : 1f;
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        isPaused = state;
    }

    private void HandlePlayerDeath()
    {

    }

    private void HandleWinTrigger()
    {

    }

    private void HandlePlayerPause()
    {
        SetPause(!isPaused);
    }

    private void HandleResumeButtonClick()
    {
        SetPause(false);
    }

    private void HandleMainMenuButtonClick()
    {

    }
}
