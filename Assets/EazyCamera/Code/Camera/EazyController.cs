using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace EazyCamera
{
    using EazyCamera.Events;
    using Util = EazyCameraUtility;

    public class EazyController : MonoBehaviour
    {
        [SerializeField] private EazyCam _controlledCamera = null;

        private void Start()
        {
            Debug.Assert(_controlledCamera != null, "Attempting to use a controller on a GameOjbect without an EazyCam component");
#if ENABLE_INPUT_SYSTEM
            SetupInput();
#endif
        }

        private void Update()
        {
            float dt = Time.deltaTime;

#if ENABLE_INPUT_SYSTEM
            HandleInput(dt);
#else
            HandleLegacyInput(dt);
#endif // ENABLE_INPUT_SYSTEM
        }

        public void SetControlledCamera(EazyCam cam)
        {
            _controlledCamera = cam;
        }

        private void ToggleLockOn()
        {
            _controlledCamera.ToggleLockOn();
        }

        private void CycleTargets()
        {
            _controlledCamera.CycleTargets();
        }

        private void CycleRight()
        {
            _controlledCamera.CycleTargetsRight();
        }

        private void CycleLeft()
        {
            _controlledCamera.CycleTargetsLeft();
        }

        private void ToggleUi()
        {
            EazyEventManager.TriggerEvent(EazyEventKey.OnUiToggled);
        }

#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputAction _toggleLockOn = new InputAction("ToggleLock");
        [SerializeField] private InputAction _cycleTargets = new InputAction("Cycle");
        [SerializeField] private InputAction _cycleRight = new InputAction("CycleRight");
        [SerializeField] private InputAction _cycleLeft = new InputAction("CycleLeft");
        [SerializeField] private InputAction _zoom = new InputAction("Zoom");
        [SerializeField] private InputAction _orbit = new InputAction("Orbit");
        [SerializeField] private InputAction _toggleUi = new InputAction("ToggleUi");
        [SerializeField] private InputAction _reset = new InputAction("ResetCamera");

        private Vector2 _rotation = new Vector2();

        public void HandleInput(float dt)
        {
            _controlledCamera.IncreaseRotation(_rotation.x, _rotation.y, dt);
            
        }

        public void SetupInput()
        {
            Validate();

            _toggleLockOn.canceled += ctx => ToggleLockOn();
            _toggleLockOn.Enable();

            _cycleTargets.performed += ctx => CycleTargets();
            _cycleTargets.Enable();

            _cycleRight.performed += ctx => CycleRight();
            _cycleRight.Enable();

            _cycleLeft.performed += ctx => CycleLeft();
            _cycleLeft.Enable();

            _toggleUi.canceled += ctx => ToggleUi();
            _toggleUi.Enable();

            _reset.canceled += ctx => _controlledCamera.ResetPositionAndRotation();
            _reset.Enable();

            _zoom.performed += OnZoom;
            _zoom.canceled += OnZoom;
            _zoom.Enable();

            _orbit.performed += OnOrbit;
            _orbit.canceled += OnOrbit;
            _orbit.Enable();
        }

        private void OnZoom(InputAction.CallbackContext ctx)
        {
            _controlledCamera.IncreaseZoomDistance(ctx.ReadValue<Vector2>().y, Time.deltaTime);
        }

        private void OnOrbit(InputAction.CallbackContext ctx)
        {
            _rotation = ctx.ReadValue<Vector2>();
        }

        private void Validate()
        {
            if (_toggleLockOn.bindings.Count == 0)
            {
                _toggleLockOn.AddBinding(Keyboard.current.tKey);
                _toggleLockOn.AddBinding(Gamepad.current.leftTrigger);
            }

            if (_cycleTargets.bindings.Count == 0)
            {
                _cycleTargets.AddBinding(Keyboard.current.spaceKey);
            }

            if (_cycleRight.bindings.Count == 0)
            {
                _cycleRight.AddBinding(Keyboard.current.eKey);
                _cycleRight.AddBinding(Gamepad.current.rightShoulder);
            }

            if (_cycleLeft.bindings.Count == 0)
            {
                _cycleLeft.AddBinding(Keyboard.current.qKey);
                _cycleLeft.AddBinding(Gamepad.current.leftShoulder);
            }

            if (_zoom.bindings.Count == 0)
            {
                _zoom.AddBinding(Mouse.current.scroll);
            }

            if (_orbit.bindings.Count == 0)
            {
                _orbit.AddBinding(Mouse.current.delta);
                _orbit.AddBinding(Gamepad.current.rightStick);
            }

            if (_toggleUi.bindings.Count == 0)
            {
                _toggleUi.AddBinding(Keyboard.current.uKey);
                _toggleUi.AddBinding(Gamepad.current.startButton);
            }

            if (_reset.bindings.Count == 0)
            {
                _reset.AddBinding(Keyboard.current.rKey);
                _reset.AddBinding(Gamepad.current.rightStickButton);
            }
        }

        private void OnValidate()
        {
            Validate();
        }

#else
        public void HandleLegacyInput(float dt)
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta > Constants.DeadZone || scrollDelta < -Constants.DeadZone)
            {
                _controlledCamera.IncreaseZoomDistance(scrollDelta, dt);
            }

            float horz = Input.GetAxis(Util.MouseX);
            float vert = Input.GetAxis(Util.MouseY);
            _controlledCamera.IncreaseRotation(horz, vert, dt);

            if (Input.GetKeyDown(KeyCode.R))
            {
                _controlledCamera.ResetPositionAndRotation();
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                ToggleLockOn();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CycleTargets();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CycleLeft();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CycleRight();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                ToggleUi();
            }
        }
#endif // ENABLE_INPUT_SYSTEM
    }


}
