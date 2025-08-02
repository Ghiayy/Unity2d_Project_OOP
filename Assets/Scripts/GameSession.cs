using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText; //gameover
    [SerializeField] AudioClip gameOverSFX;
    [SerializeField] float gameOverDelay = 2f;//gameover

    AudioSource audioSource;
    //ScreenFader screenFader;
    bool isGameOver = false; //go

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);// go

        audioSource = GetComponent<AudioSource>();
        //screenFader = FindObjectOfType<ScreenFader>();
    }

    public void ProcessPlayerDeath()
    {
        if (isGameOver) return; //go

        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            GameOver();
        }
    }

    void TakeLife()
    {
        /*playerLives--;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
        */

        playerLives--;
        livesText.text = playerLives.ToString();
        StartCoroutine(ReloadSceneAfterDelay(0.5f));
    }

    IEnumerator ReloadSceneAfterDelay(float delay) //go
    {
        yield return new WaitForSeconds(delay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /*IEnumerator GameOverSequence()
    {
        // Play game over sound
        if (audioSource != null && gameOverSFX != null)
        {
            audioSource.PlayOneShot(gameOverSFX);
        }

        // Fade screen + show "Game Over" text
        if (gameOverText != null) //y
        {
            gameOverText.gameObject.SetActive(true);//y
        }

        yield return new WaitForSeconds(2f);

        ResetGameSession();
    }
    */

    void GameOver()
    {
        isGameOver = true;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        if (audioSource != null && gameOverSFX != null)
            audioSource.PlayOneShot(gameOverSFX);

        StartCoroutine(ResetAfterDelay(gameOverDelay));
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGameSession();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>()?.ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }
}
