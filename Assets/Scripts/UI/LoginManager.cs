using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (loginButton != null && loginButton.onClick.GetPersistentEventCount() == 0)
        {
            loginButton.onClick.AddListener(OnLoginClicked);
        }

        if (registerButton != null && registerButton.onClick.GetPersistentEventCount() == 0)
        {
            registerButton.onClick.AddListener(OnRegisterClicked);
        }

        if (usernameInput != null)
        {
            usernameInput.Select();
        }
    }

    private void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Por favor, completa todos los campos");
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            ShowError("No se encontró la base de datos en la escena");
            return;
        }

        var result = DatabaseManager.Instance.LoginUser(username, password);

        if (result.success)
        {
            PlayerPrefs.SetInt("CurrentUserID", result.userId);
            PlayerPrefs.SetString("CurrentUsername", username);
            PlayerPrefs.Save();

            if (MainSceneManager.Instance != null)
            {
                MainSceneManager.Instance.PrepareMainPanel();
            }

            if (VisibilityManager.Instance != null)
            {
                VisibilityManager.Instance.ChangeToMainScene();
            }
        }
        else
        {
            ShowError("Usuario no encontrado o contraseña incorrecta");
        }
    }

    private void OnRegisterClicked()
    {
        if (VisibilityManager.Instance != null)
        {
            VisibilityManager.Instance.ChangeToRegister();
        }
    }

    private void ShowError(string message)
    {
        if (errorMessage != null)
        {
            errorMessage.text = message;
        }

        if (errorPopup != null)
        {
            errorPopup.SetActive(true);
        }

        if (VisibilityManager.Instance != null)
        {
            VisibilityManager.Instance.ChangeToError();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginClicked();
        }
    }
}
