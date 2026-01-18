using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public GameObject errorPopup;
    public TMP_Text errorMessage;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);

        if (usernameInput != null)
            usernameInput.Select();
    }

    private void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        // Validaciones básicas
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Por favor, completa todos los campos");
            return;
        }

        var result = DatabaseManager.Instance.LoginUser(username, password);

        if (result.success)
        {
            PlayerPrefs.SetInt("CurrentUserID", result.userId);
            PlayerPrefs.SetString("CurrentUsername", username);
            PlayerPrefs.Save();

            VisibilityManager.Instance.ChangeToMainScene();
        }
        else
        {
            ShowError("Usuario no encontrado o contraseña incorrecta");
        }
    }

    private void OnRegisterClicked()
    {
        VisibilityManager.Instance.ChangeToRegister();
    }

    private void ShowError(string message)
    {
        errorMessage.text = message;
        errorPopup.SetActive(true);

        VisibilityManager.Instance.ChangeToError();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginClicked();
        }
    }
}