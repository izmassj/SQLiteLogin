using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text welcomeText;
    public Button logoutButton;

    private void Start()
    {
        string username = GameSceneManager.Instance.GetCurrentUsername();
        welcomeText.text = "Benvingut/da, " + username + "!";
    }

    public void OnLogoutClicked()
    {
        VisibilityManager.Instance.ChangeToLogin();
    }
}