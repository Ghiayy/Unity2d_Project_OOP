using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip portalSFX;

    AudioSource audioSource;
    ScreenFader screenFader;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        screenFader = FindObjectOfType<ScreenFader>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (portalSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(portalSFX);
            }

            StartCoroutine(HandleLevelExit());
        }
    }

    IEnumerator HandleLevelExit()
    {
        if (screenFader != null)
        {
            yield return screenFader.FadeOut(); // Wait for fade out
        }

        yield return new WaitForSeconds(levelLoadDelay); // Optional extra delay

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
