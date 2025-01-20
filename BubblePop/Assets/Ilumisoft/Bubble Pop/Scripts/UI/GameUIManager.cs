namespace Ilumisoft.BubblePop
{
    using System.Collections;
    using UnityEngine;

    public class GameUIManager : MonoBehaviour
    {
        [SerializeField]
        GameUI gameUI = null;

        [SerializeField]
        GameOverUI gameOverUI = null;

        [SerializeField]
        OverlayCanvas overlayCanvas = null;

        public GameUI GameUI => gameUI;

        public void ShowGameOverUI()
        {
            StartCoroutine(ShowGameOverUICoroutine());
        }
        public void HideGameOverUI()
        {
            StartCoroutine(HideGameOVerUI());
        }

        IEnumerator ShowGameOverUICoroutine()
        {
            yield return overlayCanvas.FadeIn();
            gameUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(true);
            yield return overlayCanvas.FadeOut();
        }
        IEnumerator HideGameOVerUI()
        {
            yield return overlayCanvas.FadeIn();
            gameUI.gameObject.SetActive(true);
            gameOverUI.gameObject.SetActive(false);
            yield return overlayCanvas.FadeOut();
        }
    }
}