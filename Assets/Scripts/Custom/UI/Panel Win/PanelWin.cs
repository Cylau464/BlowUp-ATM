using main.level;
using System.Collections;
using UnityEngine;

namespace main.ui
{
    public class PanelWin : Panel
    {
        [SerializeField] private Star[] _stars;
        [SerializeField] private Reward _reward;
        [SerializeField] private float _showStarsDelay = .5f;

        private void OnEnable()
        {
            StartCoroutine(ShowStars());
        }

        public void NextLevel()
        {
            GameScenes.ReloadScene();
        }

        private IEnumerator ShowStars()
        {
            for(int i = 0; i < _stars.Length && i < LevelsManager.currentLevel.Stars; i++)
            {
                _stars[i].Fill();
                yield return new WaitForSeconds(_showStarsDelay);
            }

            _reward.Show();
        }
    }
}
