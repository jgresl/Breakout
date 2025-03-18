using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : SingletonMonoBehavior<UIManager>
{
    [SerializeField] private Text scoreText;
    private int displayedScore = 0;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverCanvas;

    public void UpdateScore(int newScore)
    {
        // Animate the score counting up
        DOTween.To(() => displayedScore, x =>
        {
            displayedScore = x;
            scoreText.text = "Score: " + displayedScore;
        }, newScore, 0.5f); // 0.5 seconds duration

        // Add a scaling effect for the score text
        scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
        {
            scoreText.transform.DOScale(1f, 0.2f);
        });
    }

    public void UpdateLives(int newLives)
    {
        livesText.SetText(newLives.ToString());
    }

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
    }
}