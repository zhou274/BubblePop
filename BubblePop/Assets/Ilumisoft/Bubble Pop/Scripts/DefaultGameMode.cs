namespace Ilumisoft.BubblePop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using StarkSDKSpace;
    using TTSDK.UNBridgeLib.LitJson;
    using TTSDK;
    public class DefaultGameMode : GameMode
    {
        // Reference to the grid
        [SerializeField]
        BubbleGrid grid = null;

        [SerializeField]
        SFXPlayer sfxPlayer = null;

        // Reference to the game UI manager
        GameUIManager gameUIManager;

        // Selection holds all selected bubbles
        Selection selection;

        MouseRaycast<Bubble> mouseRaycast;

        private StarkAdManager starkAdManager;

        public string clickid;
        private void Awake()
        {
            gameUIManager = FindObjectOfType<GameUIManager>();

            selection = new Selection();

            mouseRaycast = new MouseRaycast<Bubble>(Camera.main);
        }

        public override IEnumerator StartGame()
        {
            Score.Reset();

            grid.SpawnRandomBubbles();
            
            yield return null;
        }
        public override IEnumerator ContinueGame()
        {
            gameUIManager.HideGameOverUI();

            grid.SpawnRandomBubbles();

            yield return null;
        }
        public override IEnumerator RunGame()
        {
            var operations = new List<ICoroutine>();

            // Run the game as long as a selectable bubble exists
            while (grid.HasSelectable())
            {
                // Wait for input
                yield return new WaitForInput();

                // Clear the list of operations
                operations.Clear();

                // Has a bubble been clicked
                if (mouseRaycast.Perform(out Bubble bubble))
                {
                    // Does the selection already contains the clicked bubble? Pop bubbles
                    if (selection.Contains(bubble))
                    {
                        operations.Add(new HideRevenuePreview(gameUIManager));
                        operations.Add(new PopSelectedBubbles(selection, sfxPlayer));
                        operations.Add(new ProcessVerticalMovement(grid));
                        operations.Add(new ProcessHorizontalMovement(grid));
                        operations.Add(new FillEmptyColumns(grid));
                    }
                    // Otherwise deselect any currently selected bubbles and select new group of bubbles
                    else
                    {
                        operations.Add(new DeselectBubbles(selection));
                        operations.Add(new SelectBubbles(grid, selection, bubble, sfxPlayer));
                        operations.Add(new UpdateRevenuePreview(gameUIManager, selection));
                    }
                }
                // No bubble clicked? Deselect all
                else
                {
                    operations.Add(new HideRevenuePreview(gameUIManager));
                    operations.Add(new DeselectBubbles(selection));
                }

                // Execute all operations
                foreach (var operation in operations)
                {
                    yield return operation.Execute();
                }
            }

            yield return null;
        }

        public override IEnumerator EndGame()
        {
            gameUIManager.ShowGameOverUI();
            ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
            // Wait until the overlay is faded in
            yield return new WaitForSeconds(1f);

            grid.Clear();
        }

        /// <summary>
        /// 播放插屏广告
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="errorCallBack"></param>
        /// <param name="closeCallBack"></param>
        public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
        {
            starkAdManager = StarkSDK.API.GetStarkAdManager();
            if (starkAdManager != null)
            {
                var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
                mInterstitialAd.Load();
                mInterstitialAd.Show();
            }
        }

    }
}