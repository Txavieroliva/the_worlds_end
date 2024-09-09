using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System.Collections;

// #DG: separate input logic into input handler, one for each system
// instantiate the correct one at runtime

namespace EazyCamera.Legacy
{
    using Extensions;
    /// <summary>
    /// This is the main class for controlling both camera and player. It is recommended to attach this to the player or camera in the scene, but not necessary
    /// </summary>
    public class EzPlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _camera = null;
        private Transform _cameraTransform = null;
        [SerializeField] private EzMotor _controlledPlayer = null;


#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputAction _move;
        [SerializeField] private InputAction _sprint = new InputAction("Sprint", InputActionType.Button);

        private Vector3 _moveVector = Vector3.zero;
        private bool _isSprinting = false;
#endif // ENABLE_INPUT_SYSTEM

        private void Awake()
        {
            _cameraTransform = _camera.transform;

#if ENABLE_INPUT_SYSTEM
            SetupInput();
#endif
        }

        private void Start()
        {
            // if either the player or camera are null, attempt to find them
            SetUpControlledPlayer();
        }

        private void Update()
        {
            if (_controlledPlayer != null && _camera != null)
            {
#if ENABLE_INPUT_SYSTEM
                HandleInput();
#else
                HandleLegacyInput();
#endif // ENABLE_INPUT_SYSTEM
            }
        }

        private void SetUpControlledPlayer()
        {
            if (_controlledPlayer == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    _controlledPlayer = playerObj.GetComponent<EzMotor>();
                }
            }
        }

#if ENABLE_INPUT_SYSTEM

        private void SetupInput()
        {
            ValidateControls();

            _move.performed += OnMoveInput;
            _move.canceled += OnMoveInput;
            _move.Enable();

            _sprint.started += ctx => _isSprinting = true;
            _sprint.canceled += ctx => _isSprinting = false;
            _sprint.Enable();
        }

        private void HandleInput()
        {
            _controlledPlayer.MovePlayer(_moveVector.x, _moveVector.z, _isSprinting);
        }

        private void OnMoveInput(InputAction.CallbackContext cxt)
        {
            if (_controlledPlayer != null)
            {
                Vector2 inputVector = cxt.ReadValue<Vector2>();
                _moveVector = EazyCameraUtility.ConvertMoveInputToCameraSpace(_cameraTransform, inputVector.x, inputVector.y);
            }
        }

        private void OnValidate()
        {
            ValidateControls();
        }

        private void ValidateControls()
        {
            if (_move.bindings.Count == 0)
            {
                _move.AddCompositeBinding("2DVector")
                    .With("Up", "<Keyboard>/w")
                    .With("Down", "<Keyboard>/s")
                    .With("Left", "<Keyboard>/a")
                    .With("Right", "<Keyboard>/d");

                _move.AddBinding(Gamepad.current.leftStick);
            }

            if (_sprint.bindings.Count == 0)
            {
                _sprint.AddBinding(Keyboard.current.leftShiftKey);
                _sprint.AddBinding(Gamepad.current.buttonEast);
            }
        }
#else

        private void HandleLegacyInput()
        {
            // Update player movement first
            // cache the inputs
            float horz = Input.GetAxis(ExtensionMethods.HORIZONTAL);
            float vert = Input.GetAxis(ExtensionMethods.VERITCAL);

            // Convert movement to camera space
            Vector3 moveVector = EazyCameraUtility.ConvertMoveInputToCameraSpace(_cameraTransform, horz, vert);

            // Move the Player
            _controlledPlayer.MovePlayer(moveVector.x, moveVector.z, Input.GetKey(KeyCode.LeftShift));
        }
#endif // ENABLE_INPUT_SYSTEM
    }
}
