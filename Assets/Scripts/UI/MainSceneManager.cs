using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }

    [Header("Main View")]
    public TMP_Text welcomeText;
    public Button logoutButton;
    public Button showUsersButton;
    public GameObject mainContentPanel;

    [Header("Users View")]
    public GameObject usersPanel;
    public Button refreshButton;
    public Transform usersContent;
    public UserRowUI userRowTemplate;
    public TMP_Text emptyUsersText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        RefreshWelcomeText();

        if (logoutButton != null && logoutButton.onClick.GetPersistentEventCount() == 0)
        {
            logoutButton.onClick.AddListener(OnLogoutClicked);
        }

        if (showUsersButton != null && showUsersButton.onClick.GetPersistentEventCount() == 0)
        {
            showUsersButton.onClick.AddListener(ShowUsersPanel);
        }

        if (refreshButton != null && refreshButton.onClick.GetPersistentEventCount() == 0)
        {
            refreshButton.onClick.AddListener(RefreshUsersList);
        }

        if (usersPanel != null)
        {
            usersPanel.SetActive(false);
        }

        if (mainContentPanel != null)
        {
            mainContentPanel.SetActive(true);
        }

        if (userRowTemplate != null)
        {
            userRowTemplate.gameObject.SetActive(false);
        }
    }

    public void PrepareMainPanel()
    {
        RefreshWelcomeText();
    }

    public void RefreshWelcomeText()
    {
        if (welcomeText == null)
        {
            return;
        }

        string username = PlayerPrefs.GetString("CurrentUsername", "Usuario");
        welcomeText.text = "Benvingut/da, " + username + "!";
    }

    public void OnLogoutClicked()
    {
        PlayerPrefs.DeleteKey("CurrentUserID");
        PlayerPrefs.DeleteKey("CurrentUsername");
        PlayerPrefs.Save();

        ClearUserRows();
        RefreshWelcomeText();

        if (VisibilityManager.Instance != null)
        {
            VisibilityManager.Instance.ChangeToLogin();
        }
        else
        {
            Debug.LogWarning("No se encontró VisibilityManager en la escena.");
        }
    }

    public void ShowUsersPanel()
    {
        if (mainContentPanel != null)
        {
            mainContentPanel.SetActive(false);
        }

        if (usersPanel != null)
        {
            usersPanel.SetActive(true);
        }

        RefreshUsersList();
    }


    public void RefreshUsersList()
    {
        if (usersContent == null || userRowTemplate == null)
        {
            Debug.LogWarning("UsersContent o UserRowTemplate no están asignados.");
            return;
        }

        if (DatabaseManager.Instance == null)
        {
            Debug.LogWarning("No se encontró DatabaseManager en la escena.");
            return;
        }

        ClearUserRows();

        List<User> users = DatabaseManager.Instance.GetAllUsers();

        if (emptyUsersText != null)
        {
            emptyUsersText.gameObject.SetActive(users.Count == 0);
        }

        for (int i = 0; i < users.Count; i++)
        {
            UserRowUI newRow = Instantiate(userRowTemplate, usersContent);
            newRow.gameObject.SetActive(true);
            newRow.Setup(users[i], this);
        }
    }

    public void DeleteUserFromList(int userId)
    {
        if (DatabaseManager.Instance == null)
        {
            Debug.LogWarning("No se encontró DatabaseManager en la escena.");
            return;
        }

        bool deleted = DatabaseManager.Instance.DeleteUser(userId);

        if (!deleted)
        {
            Debug.LogWarning("No se pudo borrar el usuario.");
            return;
        }

        int currentUserId = PlayerPrefs.GetInt("CurrentUserID", -1);

        if (currentUserId == userId)
        {
            OnLogoutClicked();
            return;
        }

        RefreshUsersList();
    }

    private void ClearUserRows()
    {
        if (usersContent == null)
        {
            return;
        }

        for (int i = usersContent.childCount - 1; i >= 0; i--)
        {
            Transform child = usersContent.GetChild(i);

            if (userRowTemplate != null && child == userRowTemplate.transform)
            {
                continue;
            }

            Destroy(child.gameObject);
        }
    }
}
