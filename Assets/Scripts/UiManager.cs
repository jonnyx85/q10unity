using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public RectTransform MainMenu, Settings;


    public void Start()
    {
        MainMenu.DOAnchorPosX(0, 1f).SetEase(Ease.InFlash).SetUpdate(true);
    }

    public void OptionButton()
    {
        Settings.gameObject.SetActive(true);
        SoundManager.Instance.PlayPopSound();
        Settings.DOScale(1f,.4f).SetEase(Ease.InFlash).SetUpdate(true);
    }

    public void OptionOK()
    {
        SoundManager.Instance.PlayClickSound();
        Settings.DOScale(0f, .4f).SetEase(Ease.InFlash).SetUpdate(true).OnComplete(() =>
        Settings.gameObject.SetActive(false)

        );
    }

    public void Play()
    {
        SoundManager.Instance.PlayClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Quit()
    {
        SoundManager.Instance.PlayClickSound();
        Application.Quit();
    }
}
