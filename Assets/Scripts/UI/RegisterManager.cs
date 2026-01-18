using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
        registerButton.onClick.AddListener(OnRegisterClicked);
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    private void OnRegisterClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
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

        if (DatabaseManager.Instance.UserExists(username))
        {
            ShowError("El nombre de usuario ya existe");
            return;
        }

        bool success = DatabaseManager.Instance.RegisterUser(username, password);

        if (success)
        {
            VisibilityManager.Instance.ChangeToLogin();
        }
        else
        {
            ShowError("Error al registrar usuario");
        }
    }

    private void OnLoginClicked()
    {
        VisibilityManager.Instance.ChangeToLogin();
    }

    private void ShowError(string message)
    {
        errorMessage.text = message;
        errorPopup.SetActive(true);

        VisibilityManager.Instance.ChangeToError();
    }
}