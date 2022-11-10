using events;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace input
{
    #region draging data
    public struct InputInfo
    {
        public Vector3 lastPosition;
        public Vector3 initPoint;

        public Vector3 currentPosition => Input.mousePosition;
        public Vector3 daltaDrag => currentPosition - initPoint;

        public float lengthDrag => Vector3.Distance(currentPosition, initPoint);
        public Vector3 lastDaltaDrag => Input.mousePosition - lastPosition;
    }
    #endregion

    [Serializable]
    public static class InputEvents
    {
        #region Events
        private static GameEvent<IClick> _click = new GameEvent<IClick>();
        private static GameEvent<IBeginDrag> _beginDrag = new GameEvent<IBeginDrag>();
        private static GameEvent<IDrag> _drag = new GameEvent<IDrag>();
        private static GameEvent<IEndDrag> _endDrag = new GameEvent<IEndDrag>();
        #endregion

        #region variables
        private static bool s_PassThroughUI = true;
        private static float s_SmoothDrag = 0;

        private static GraphicRaycaster s_GraphicRaycaster;

        public static bool isClicked { get; private set; }
        public static bool isDragging { get; private set; }

        private static InputInfo s_DragingData;
        private static bool s_IsClickedDown;
        private static bool s_IsInited = false;
        #endregion

        #region internal
        internal static void Initialize(GraphicRaycaster graphicRaycaster, bool passThroughUI, float smoothDrag)
        {
            _click.CleanSubscribes();
            _beginDrag.CleanSubscribes();
            _drag.CleanSubscribes();
            _endDrag.CleanSubscribes();

            s_PassThroughUI = passThroughUI;
            s_SmoothDrag = smoothDrag;

            s_GraphicRaycaster = graphicRaycaster;

            // Init data
            s_IsClickedDown = false;
            isClicked = false;
            isDragging = false;
            s_DragingData = new InputInfo();

            // is inited
            s_IsInited = true;
        }

        internal static void Update()
        {
            if (s_IsInited == false || !ControllerInputs.s_EnableInputs)
            {
                if (s_IsClickedDown && ControllerInputs.OnMouse(MouseStatue.Up))
                {
                    s_IsClickedDown = false;
                    OnMouseUp();
                }
                return;
            }

            isClicked = false;
            if (ControllerInputs.OnMouse(MouseStatue.Down, s_PassThroughUI))
            {
                s_IsClickedDown = true;
                OnMouseDown();
            }
            else
            if (s_IsClickedDown && ControllerInputs.OnMouse(MouseStatue.Up))
            {
                s_IsClickedDown = false;
                OnMouseUp();
            }
            else
            if (s_IsClickedDown && ControllerInputs.OnMouse(MouseStatue.Idle))
                OnMouse();

        }
        #endregion

        #region private
        private static void OnMouseDown()
        {
            s_DragingData.lastPosition = s_DragingData.initPoint = Input.mousePosition;
            isDragging = false;
        }

        private static void OnMouseUp()
        {
            if (isDragging)
            {
                InvokeEndDrag(s_DragingData);
            }
            else
            if (!ControllerInputs.IsRaycastedUI(s_GraphicRaycaster))
            {
                isClicked = true;

                InvokeClick(s_DragingData);
            }
            isDragging = false;
        }

        private static void OnMouse()
        {
            /// On drag
            if (s_SmoothDrag < Vector3.Distance(s_DragingData.lastPosition, Input.mousePosition))
            {
                if (!isDragging)
                {
                    InvokeBeginDrag(s_DragingData);
                    isDragging = true;
                }

                InvokeDrag(s_DragingData);

                s_DragingData.lastPosition = Input.mousePosition;
            }
        }
        #endregion

        #region Click
        public static void SubscribeClick(IClick click)
        {
            _click.Subscribe(click);
        }

        public static void UnsubscribeClick(IClick click)
        {
            _click.Unsubscribe(click);
        }

        private static void InvokeClick(InputInfo data)
        {
            _click.Invoke(onClick => onClick?.OnClick(data));
        }
        #endregion

        #region Begin
        public static void SubscribeBegineDrag(IBeginDrag beginDrag)
        {
            _beginDrag.Subscribe(beginDrag);
        }

        public static void UnsubscribeBegineDrag(IBeginDrag beginDrag)
        {
            _beginDrag.Unsubscribe(beginDrag);
        }

        private static void InvokeBeginDrag(InputInfo data)
        {
            _beginDrag.Invoke(beginDrag => beginDrag?.OnBeginDrag(data));
        }
        #endregion

        #region Drag
        public static void SubscribeDrag(IDrag drag)
        {
            _drag.Subscribe(drag);
        }

        public static void UnsubscribeDrag(IDrag drag)
        {
            _drag.Unsubscribe(drag);
        }

        private static void InvokeDrag(InputInfo data)
        {
            _drag.Invoke(drag => drag?.OnDrag(data));
        }
        #endregion

        #region End
        public static void SubscribeEndDrag(IEndDrag endDrag)
        {
            _endDrag.Subscribe(endDrag);
        }

        public static void UnsubscribeEndDrag(IEndDrag endDrag)
        {
            _endDrag.Subscribe(endDrag);
        }

        private static void InvokeEndDrag(InputInfo data)
        {
            _endDrag.Invoke(endDrag => endDrag?.OnEndDrag(data));
        }
        #endregion
    }
}
