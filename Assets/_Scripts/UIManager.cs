using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehavior<UIManager>
{
    [SerializeField] private Text scoreText;
    
    public void UpdateScore(int newScore)
    {
            scoreText.text = "Score: " + newScore;
    }
}
