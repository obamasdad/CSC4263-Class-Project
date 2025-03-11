using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    private CanvasGroup fadeScreen;
    private PlayerMovement playerMovement; // Reference to player script

    private void Start()
    {
        GameObject fadeObj = GameObject.Find("FadeScreen");
        if (fadeObj != null)
        {
            fadeScreen = fadeObj.GetComponent<CanvasGroup>();
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("FadeScreen not found in scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>(); // Get PlayerMovement script
            if (playerMovement != null)
            {
                playerMovement.canMove = false; // 🚀 Disable movement
                playerMovement.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // 🚀 Stop momentum
            }

            StartCoroutine(TransitionToNextLevel());
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        if (fadeScreen != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene(nextLevelName);
    }

    private IEnumerator FadeOut()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            fadeScreen.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeScreen.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            fadeScreen.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeScreen.alpha = 0f;
    }
}
