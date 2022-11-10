using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InspectorProgramming
{
    public class CursorReplacement : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursorTexture;
        
        private bool _isHideCursor = false;

        private CursorReplacement _cursorReplacement;

        private void Awake()
        {
            _cursorReplacement = GetComponent<CursorReplacement>();
            _cursorReplacement.enabled = false;
        }

        private void Update()
        {
            if (_isHideCursor)
            {
                Cursor.visible = false;
            }
        }

        public void TurnCursor()
        {
            _isHideCursor = !_isHideCursor;
            _cursorReplacement.enabled = _isHideCursor;
        }

        [NaughtyAttributes.Button("Set Texture Cursor")]
        public void SetTextureToCursor()
        {
            if (_isHideCursor)
            {
                TurnCursor();
            }
            
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}