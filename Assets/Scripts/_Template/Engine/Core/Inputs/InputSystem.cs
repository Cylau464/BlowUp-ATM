using UnityEngine;
using UnityEngine.UI;

namespace input
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] protected GraphicRaycaster _mainGraphicRaycaster;
        [SerializeField] protected bool _initEnableInput = false;
        [SerializeField] protected bool _passThroughUI = true;
        [SerializeField] protected float _smoothDrag = 0;

        #region mono
        protected virtual void Awake()
        {
            if (_mainGraphicRaycaster == null) throw new System.NullReferenceException("Graphic Raycaster has null reference.");

            ControllerInputs.Initialize(_mainGraphicRaycaster, _initEnableInput);

            InputEvents.Initialize(_mainGraphicRaycaster, _passThroughUI, _smoothDrag);
        }

        protected virtual void Update()
        {
            InputEvents.Update();
        }

        protected virtual void OnValidate()
        {
            if (_mainGraphicRaycaster == null)
                _mainGraphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        }
        #endregion
    }
}
