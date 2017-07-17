using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : MonoBehaviour
{
    public Text livesText;
    public Text progressText;
    
    public void SetLives(int lives)
    {
        livesText.text = lives.ToString();
    }

    public void SetProgress(int progress)
    {
        progressText.text = progress.ToString() + '%';
    }

    void Awake()
    {
        
    }    
}
