using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserRowUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text usernameText;
    public TMP_Text createdDateText;
    public Button deleteButton;

    private int _userId;
    private MainSceneManager _mainSceneManager;

    public void Setup(User user, MainSceneManager mainSceneManager)
    {
        _userId = user.UserID;
        _mainSceneManager = mainSceneManager;

        if (usernameText != null)
        {
            usernameText.text = user.Username;
        }

        if (createdDateText != null)
        {
            createdDateText.text = user.CreatedDate;
        }

        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(OnDeleteClicked);
        }
    }

    private void OnDeleteClicked()
    {
        if (_mainSceneManager != null)
        {
            _mainSceneManager.DeleteUserFromList(_userId);
        }
    }
}
