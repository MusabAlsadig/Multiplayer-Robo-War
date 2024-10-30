using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public static void ReturnToMainMenu()
    {
        SceneManager.LoadScene(GameManager.MainMenu);
    }

    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

}
