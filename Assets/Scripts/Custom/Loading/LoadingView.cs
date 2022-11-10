using UnityEngine;
using UnityEngine.Video;

namespace loading
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _player;

        private AsyncOperation _loadingScene;

        protected void Start()
        {
            _player.loopPointReached += End;
            _loadingScene = GameScenes.LoadMainScene();
            
            if (_loadingScene != null)
                _loadingScene.allowSceneActivation = false;
        }

        private void OnDestroy()
        {
            _player.loopPointReached -= End;
        }

        private void End(VideoPlayer source)
        {
            _player.loopPointReached -= End;

            if (_loadingScene != null)
                _loadingScene.allowSceneActivation = true;
        }
    }
}
