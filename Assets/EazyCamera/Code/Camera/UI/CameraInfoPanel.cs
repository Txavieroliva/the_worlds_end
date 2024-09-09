using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyCamera
{
    using EazyCamera.Events;
    public class CameraInfoPanel : MonoBehaviour, IEventListener
    {
        [SerializeField] private GameObject _rootPanel = null;
        [SerializeField] private GameObject[] _targetCommands = System.Array.Empty<GameObject>();

        private void Awake()
        {
            BindEvents();
        }

        private void Start()
        {
            SetTargetInfoState(EnabledState.Disabled);
        }

        private void OnDestroy()
        {
            UnbindEvents();
        }

        public void BindEvents()
        {
            EazyEventManager.BindToEvent(EazyEventKey.OnEnterFocasableRange, ShowTargetingInfo);
            EazyEventManager.BindToEvent(EazyEventKey.OnExitFocasableRange, HideTargetingInfo);
            EazyEventManager.BindToEvent(EazyEventKey.OnUiToggled, ToggleUiVisibility);
        }

        public void UnbindEvents()
        {
            EazyEventManager.UnbindFromEvent(EazyEventKey.OnEnterFocasableRange, ShowTargetingInfo);
            EazyEventManager.UnbindFromEvent(EazyEventKey.OnEnterFocasableRange, HideTargetingInfo);
            EazyEventManager.UnbindFromEvent(EazyEventKey.OnUiToggled, ToggleUiVisibility);
        }

        private void ShowTargetingInfo(EventData data)
        {
            SetTargetInfoState(EnabledState.Enabled);
        }

        private void HideTargetingInfo(EventData data)
        {
            SetTargetInfoState(EnabledState.Disabled);
        }

        private void SetTargetInfoState(EnabledState state)
        {
            int numObjs = _targetCommands.Length;

            for (int i = 0; i < numObjs; ++i)
            {
                _targetCommands[i].SetActive(state == EnabledState.Enabled ? true : false);
            }
        }

        private void ToggleUiVisibility(EventData data)
        {
            _rootPanel.SetActive(!_rootPanel.activeSelf);
        }
    }
}
