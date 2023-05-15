using UnityEngine;


public class GameSpeedManager : MonoBehaviour
{
    // Settings

    // Connections

    // State Variables
    [Header("Variables")]
    public float speedIncreaseMagnitude;
    public float speedIncreaseDuration;
    public float secondsToAddEachClick;

    float speedIncreaseDurationDefault;
    float timer;

    bool screenClicked;
    bool gameIsSpedUp;

    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        EventManager.ScreenClicked += OnScreenClicked;
    }
    void InitState()
    {
        timer = 0f;
        screenClicked = false;
        gameIsSpedUp = false;
        speedIncreaseDurationDefault = speedIncreaseDuration;
    }

    void Update()
    {
        if (screenClicked)
        {
            ResetSpeed();
        }
    }

    private void OnScreenClicked()
    {
        screenClicked = true;

        if (!gameIsSpedUp)
        {
            Time.timeScale *= speedIncreaseMagnitude;
            gameIsSpedUp = true;
        }
        else
        {
            speedIncreaseDuration += secondsToAddEachClick;
        }
    }

    private void ResetSpeed()
    {
        timer += Time.deltaTime;
        if (timer > speedIncreaseDuration)
        {
            timer = 0f;
            screenClicked = false;
            gameIsSpedUp = false;
            Time.timeScale /= speedIncreaseMagnitude;
            speedIncreaseDuration = speedIncreaseDurationDefault;
        }
    }

    private void OnDestroy()
    {
        EventManager.ScreenClicked -= OnScreenClicked;
    }
}

