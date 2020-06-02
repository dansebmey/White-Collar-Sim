using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator _screenFadeFilterAnimator;

    private void Awake()
    {
        _screenFadeFilterAnimator = GetComponentInChildren<Animator>(true);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        _screenFadeFilterAnimator.Play("FadeToBlack");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    internal void Play(string animationName)
    {
        _screenFadeFilterAnimator.gameObject.SetActive(true);
        GameManager.GetInstance().CurrentState = GameManager.State.CUTSCENE;
        _screenFadeFilterAnimator.Play(animationName);
    }
}
