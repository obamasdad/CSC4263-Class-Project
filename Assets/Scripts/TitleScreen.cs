using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public string gameSceneName = "GameScene"; // Set this to your actual game scene name

    public void PlayGame()
    {
        Debug.Log("Play Button Clicked!"); // Check if this prints in Console
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!"); // For testing in Unity Editor
        Application.Quit(); // Quits the game (works in a built application)
    }

        public void TestButtonClick()
    {
        Debug.Log("Button Clicked!");
    }
}
