using InputManagement;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    private static InputControls controls;

    [SerializeField] private InputAction mouseDelta_Action;
	[Header("Info")]
    public Vector2 movement;
    public Vector2 mousePos;
    public Vector2 mouseDelta;
    public Vector2 scrollDelta;
    [Tooltip("This is calculated on update")]
    public Vector2 inputDir;

    public static event Action OnScrollDelta;

#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
    public static bool IsOnSmartPhone { get; private set; } = false;
#else
    public static bool IsOnSmartPhone { get; private set; } = true;
#endif
    private void Awake()
    {
		if (instance != null)
			return;
			
        instance = this;
        controls = new InputControls();
        EventManager.Setup(controls);
    }

    private void Start()
    {
        InitControls();
    }

    Vector2 vector2;

    private void Update()
    {
        vector2.x = Input.GetAxis("Horizontal");
        vector2.y = Input.GetAxis("Vertical");
        inputDir = vector2.normalized;
        scrollDelta = Input.mouseScrollDelta;
    }

    private void InitControls()
    {

        controls.Player.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => mouseDelta = Vector2.zero;
        
        //controls.Player.Point.performed += ctx => mousePos = ctx.ReadValue<Vector2>();


        if (!IsOnSmartPhone) //  add this to pc only
            controls.Player.Look.AddBinding(mouseDelta_Action.bindings[0]);


        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => movement = Vector2.zero;

        controls.Player.Reload.performed += _ => EventManager.ActionHandler(EventManager.Controls.Reload);

        controls.Player.Fire.started += _ => EventManager.ActionStartHandler(EventManager.Controls.Fire);
        controls.Player.Fire.canceled += _ => EventManager.ActionCancledHandler(EventManager.Controls.Fire);

        controls.Player.Thruster.started += _ => EventManager.ActionStartHandler(EventManager.Controls.Thruster);
        controls.Player.Thruster.canceled += _ => EventManager.ActionCancledHandler(EventManager.Controls.Thruster);

    }

    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
		controls?.Disable();
    }

    public static void DisableInput()
    {
        controls?.Player.Disable();
        print("player Controls have been <color=red>Disabled</color>");
    }
    
    public static void EnableInput()
    {
        controls?.Player.Enable();
        print("player Controls have been <color=green>Enabled</color>");
    }

    #region Utilites
    public void SwitchActionMap(string actionMapName)
    {
        var actionMap = controls.asset.FindActionMap(actionMapName);
        // Swap to new action map
    }
    #endregion
}
