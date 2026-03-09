using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public Button registerButton;
    public Button loginButton;
    public GameObject errorPopup;
    public TMP_Text errorMessage;

    private void Start()
    {
        if (registerButton != null && registerButton.onClick.GetPersistentEventCount() == 0)
        {
            registerButton.onClick.AddListener(OnRegisterClicked);
        }

        if (loginButton != null && loginButton.onClick.GetPersistentEventCount() == 0)
        {
            loginButton.onClick.AddListener(OnLoginClicked);
        }
    }

    private void OnRegisterClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ShowError("Por favor, completa todos los campos");
            return;
        }

        if (password.Length < 8)
        {
            ShowError("La contraseña debe tener al menos 8 caracteres");
            return;
        }

        if (password != confirmPassword)
        {
            ShowError("Las contraseñas no coinciden");
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            ShowError("No se encontró la base de datos en la escena");
            return;
        }

        if (DatabaseManager.Instance.UserExists(username))
        {
            ShowError("El nombre de usuario ya existe");
            return;
        }

        bool success = DatabaseManager.Instance.RegisterUser(username, password);

        if (success)
        {
            if (VisibilityManager.Instance != null)
            {
                VisibilityManager.Instance.ChangeToLogin();
            }
        }
        else
        {
            ShowError("Error al registrar usuario");
        }
    }

    private void OnLoginClicked()
    {
        if (VisibilityManager.Instance != null)
        {
            VisibilityManager.Instance.ChangeToLogin();
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
}
