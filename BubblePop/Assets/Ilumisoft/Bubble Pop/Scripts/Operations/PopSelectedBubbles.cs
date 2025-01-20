namespace Ilumisoft.BubblePop
{
    using System.Collections;
    using UnityEngine;

    public class PopSelectedBubbles : ICoroutine
    {
        Selection selection;
        SFXPlayer sfxPlayer;

        public PopSelectedBubbles(Selection selection, SFXPlayer sfxPlayer)
        {
            this.sfxPlayer = sfxPlayer;
            this.selection = selection;
        }

        public IEnumerator Execute()
        {
            sfxPlayer.PlayPopSFX();

            Score.Add(new ScoreRevenue(selection).Value);

            foreach (var bubble in selection.Selected)
            {
                bubble.Pop();
            }

            selection.Clear();

            yield return new WaitForSeconds(0.25f);
        }
    }
}