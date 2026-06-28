using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private AreaTrigger winTrigger;

    private PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        playerController.OnPlayerPause += HandlePlayerPause;
        player.GetHealthPoints.OnDie += HandlePlayerDeath;
        winTrigger.OnTrigger += HandleWinTrigger;
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        playerController.OnPlayerPause -= HandlePlayerPause;
        player.GetHealthPoints.OnDie -= HandlePlayerDeath;
        winTrigger.OnTrigger -= HandleWinTrigger;
    }

    private void HandlePlayerDeath()
    {

    }

    private void HandleWinTrigger()
    {

    }

    private void HandlePlayerPause()
    {

    }
}
