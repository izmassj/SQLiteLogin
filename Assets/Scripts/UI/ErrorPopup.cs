using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorPopup : MonoBehaviour
{
    public TMP_Text errorText;
    public Button closeButton;
    public float autoHideTime = 3f;

    private void Start()
    {
        closeButton.onClick.AddListener(HidePopup);
    }

    public void HidePopup()
    {
        VisibilityManager.Instance.ReturnFromError();

        gameObject.SetActive(false);
    }
}