using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private GameObject ballObject;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private int score = 0;
    [SerializeField] private int pointsPerBrick = 100;
    private int currentBrickCount;
    private int totalBrickCount;
    private int maxLives = 3;
    private int currentLives;

    private void Start()
    {
        // Set current lives at the start of game 
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level 1")
        {
            currentLives = maxLives;
        } else
        {   
            // Get the current lives 
            currentLives = SceneHandler.Instance.GetCurrentLives();
        }

        // Update the UI
        UIManager.Instance.UpdateLives(currentLives);
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        // implement particle effect here
        // add camera shake here
        score += pointsPerBrick;
        UpdateScoreUI();
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if (currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        if (currentLives > 0)
        {
            // Decrement current lives
            currentLives--;

            // Update the UI
            UpdateLivesUI();

            // Reset the ball position
            ball.ResetBall();
        }
        else
        {
            // Inactivate the ball
            ballObject.SetActive(false);

            // Game over UI
            UIManager.Instance.ShowGameOver();

            // Exit to main menu after delay
            StartCoroutine(RestartGame());
        }
    }
    private void UpdateScoreUI()
    {
        UIManager.Instance.UpdateScore(score);
    }

    private void UpdateLivesUI()
    {
        // Update the UI
        UIManager.Instance.UpdateLives(currentLives);

        // Also store the current lives globally
        SceneHandler.Instance.SetCurrentLives(currentLives);
    }

    private IEnumerator RestartGame()
    {
        // Freeze time for 1.5 seconds
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1.0f;

        // Move to the main menu
        SceneHandler.Instance.LoadMenuScene();
    }
}