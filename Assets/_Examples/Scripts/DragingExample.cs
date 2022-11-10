using input;
using physic;
using UnityEngine;

namespace examples
{
    public class DragingExample : MonoBehaviour, IBeginDrag, IEndDrag, IDrag
    {
        public Camera _camera;
        public GameObject _cube;
        public CrossRay _crossRay;

        // Start is called before the first frame update
        protected void OnEnable()
        {
            _crossRay = new CrossRay(_camera, NormalAxis.Horizontal, 0);

            InputEvents.SubscribeBegineDrag(this);
            InputEvents.SubscribeDrag(this);
            InputEvents.SubscribeEndDrag(this);
        }

        protected void OnDisable()
        {
            InputEvents.UnsubscribeBegineDrag(this);
            InputEvents.UnsubscribeDrag(this);
            InputEvents.UnsubscribeEndDrag(this);
        }

        public void OnBeginDrag(InputInfo data)
        {
            _cube?.SetActive(true);
        }

        public void OnDrag(InputInfo data)
        {
            _crossRay.ThrowRaycast();

            if (_crossRay.isReached == true)
                _cube.transform.position = _crossRay.point;
        }

        public void OnEndDrag(InputInfo data)
        {
            _cube.SetActive(false);
        }
    }
}
