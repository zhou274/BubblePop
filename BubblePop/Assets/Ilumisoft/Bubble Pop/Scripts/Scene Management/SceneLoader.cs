﻿namespace Ilumisoft.BubblePop
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        OverlayCanvas overlayCanvas = null;

        IEnumerator Start()
        {
            yield return overlayCanvas.FadeOut();
        }

        public void LoadScene(string name)
        {
            StopAllCoroutines();
            StartCoroutine(LoadSceneCoroutine(name));
        }

        IEnumerator LoadSceneCoroutine(string name)
        {
            yield return overlayCanvas.FadeIn();

            SceneManager.LoadScene(name);
        }
    }
}