using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject winCondition;

    [SerializeField] private CanvasGroup endScreenCanvasGroup;
    [SerializeField] private CanvasGroup pauseScreenCanvasGroup;
    [SerializeField] private Button pauseScreenResumeButton;
    [SerializeField] private Button pauseScreenMainMenuButton;
    [SerializeField] private Button endScreenContinueButton;
    [SerializeField] private Button endScreenMainMenuButton;
    [SerializeField] private TextMeshProUGUI endScreenTitleText;

    private const string loseText = "You lost!";
    private const string winText = "You win!";

    private bool isPaused = false;
    private bool hasLost = false;

    private AreaTrigger winConditionTrigger;
    private PlayerController playerController;

    public bool GetIsPaused { get { return isPaused; } }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        winConditionTrigger = winCondition.GetComponent<AreaTrigger>();

        playerController.OnPlayerPause += HandlePlayerPause;
        player.GetHealthPoints.OnDie += HandlePlayerDeath;
        winConditionTrigger.OnTrigger += HandleWinTrigger;

        pauseScreenResumeButton.onClick.AddListener(HandleResumeButtonClick);
        pauseScreenMainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
        endScreenContinueButton.onClick.AddListener(HandleContinueButtonClick);
        endScreenMainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
    }

    private void OnDestroy()
    {
        playerController.OnPlayerPause -= HandlePlayerPause;
        player.GetHealthPoints.OnDie -= HandlePlayerDeath;
        winConditionTrigger.OnTrigger -= HandleWinTrigger;

        pauseScreenResumeButton.onClick.RemoveListener(HandleResumeButtonClick);
        pauseScreenMainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
        endScreenContinueButton.onClick.RemoveListener(HandleContinueButtonClick);
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

    private void EndGame()
    {
        UiUtils.SetCanvasActive(endScreenCanvasGroup, true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        endScreenTitleText.text = hasLost ? loseText : winText;
    }

    private void HandlePlayerDeath()
    {
        hasLost = true;

        EndGame();
    }

    private void HandleWinTrigger(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EndGame();
        }
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
    private void HandleContinueButtonClick()
    {

    }
}
