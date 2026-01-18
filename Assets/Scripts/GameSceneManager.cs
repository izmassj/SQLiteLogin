using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("Scene Names")]
    public string loginScene = "LoginScene";
    public string mainScene = "MainScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("CurrentUserID");
        PlayerPrefs.DeleteKey("CurrentUsername");
        PlayerPrefs.Save();

        Debug.Log("Im changing");
        VisibilityManager.Instance.ChangeToLogin();
    }

    public void CheckSession()
    {
        if (SceneManager.GetActiveScene().name == mainScene)
        {
            if (!PlayerPrefs.HasKey("CurrentUserID"))
            {
                Logout();
            }
        }
    }

    public string GetCurrentUsername()
    {
        return PlayerPrefs.GetString("CurrentUsername", "Usuario");
    }
}