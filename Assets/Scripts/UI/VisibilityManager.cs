using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    [Header("General Visual Components")]
    public GameObject mainPanel;

    [Header("UI Login Visual Components")]
    public GameObject loginPanel;
    public CanvasGroup loginCanvasGroup;
    public GameObject loginVerticalLayout;

    [Header("UI Register Visual Components")]
    public GameObject registerPanel;
    public CanvasGroup registerCanvasGroup;
    public GameObject registerVerticalLayout;

    [Header("UI Error Visual Components")]
    public GameObject errorPanel;
    public CanvasGroup errorCanvasGroup;
    public GameObject errorVerticalLayout;

    [Header("UI Success Visual Components")]
    public GameObject mainScenePanel;
    public CanvasGroup mainSceneCanvasGroup;
    public GameObject mainSceneFullLayout;

    public static VisibilityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ChangeToRegister()
    {
        loginCanvasGroup.DOFade(0, 0.25f);
        loginPanel.SetActive(false);
        mainPanel.GetComponent<RectTransform>().DOSizeDelta(registerVerticalLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
        {
            registerPanel.SetActive(true);
            registerCanvasGroup.DOFade(1, 0.25f);
        });
    }

    public void ChangeToLogin()
    {
        if (registerPanel.activeInHierarchy)
        {
            registerCanvasGroup.DOFade(0, 0.25f);
            registerPanel.SetActive(false);
        }
        else if (mainScenePanel.activeInHierarchy)
        {
            mainSceneCanvasGroup.DOFade(0, 0.25f);
            mainScenePanel.SetActive(false);
        }

        mainPanel.GetComponent<RectTransform>().DOSizeDelta(loginVerticalLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
        {
            loginPanel.SetActive(true);
            loginCanvasGroup.DOFade(1, 0.25f);
        });
    }

    public void ChangeToMainScene()
    {
        if (loginPanel.activeInHierarchy)
        {
            loginCanvasGroup.DOFade(0, 0.25f);
            loginPanel.SetActive(false);
        }
        else if (registerPanel.activeInHierarchy)
        {
            registerCanvasGroup.DOFade(0, 0.25f);
            registerPanel.SetActive(false);
        }

        mainPanel.GetComponent<RectTransform>().DOSizeDelta(mainSceneFullLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
        {
            mainScenePanel.SetActive(true);
            mainSceneCanvasGroup.DOFade(1, 0.25f);
        });
    }

    public void ChangeToError()
    {
        if (loginPanel.activeInHierarchy)
        {
            loginCanvasGroup.DOFade(0, 0.25f);
            loginCanvasGroup.interactable = false;
        }
        else if (registerPanel.activeInHierarchy)
        {
            registerCanvasGroup.DOFade(0, 0.25f);
            registerCanvasGroup.interactable = false;
        }

        mainPanel.GetComponent<RectTransform>().DOSizeDelta(errorVerticalLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
        {
            errorPanel.SetActive(true);
            errorCanvasGroup.DOFade(1, 0.25f);
        });
    }

    public void ReturnFromError()
    {
        errorCanvasGroup.DOFade(0, 0.25f);
        errorPanel.SetActive(false);

        if (loginPanel.activeInHierarchy)
        {
            loginCanvasGroup.DOFade(0, 0.25f);
            loginCanvasGroup.interactable = true;
            mainPanel.GetComponent<RectTransform>().DOSizeDelta(loginVerticalLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
            {
                loginPanel.SetActive(true);
                loginCanvasGroup.DOFade(1, 0.25f);
            });
        }
        else if (registerPanel.activeInHierarchy)
        {
            registerCanvasGroup.DOFade(0, 0.25f);
            registerCanvasGroup.interactable = true;
            mainPanel.GetComponent<RectTransform>().DOSizeDelta(registerVerticalLayout.GetComponent<RectTransform>().sizeDelta, 0.25f).OnComplete(() =>
            {
                registerPanel.SetActive(true);
                registerCanvasGroup.DOFade(1, 0.25f);
            });
        }
    }
}
