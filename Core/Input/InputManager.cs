using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace UntitledEngine.Core.Input;

internal class InputManager
{
    // Public input context (could be readonly if not modified outside)
    public IInputContext InputContext { get; }

    // Public mouse position property
    public Vector2 MousePosition { get; private set; }

    // Private fields
    private readonly IWindow window;
    private IKeyboard? keyboard;
    private IMouse? mouse;

    // Keyboard state tracking
    private readonly HashSet<Key> keysDown = new();
    private readonly HashSet<Key> keysPressed = new();

    // Mouse state tracking
    private readonly HashSet<MouseButton> mouseButtonsDown = new();
    private readonly HashSet<MouseButton> mouseButtonsPressed = new();

    // Initialize input and subscribe to events
    public InputManager(IWindow window)
    {
        this.window = window;
        InputContext = window.CreateInput();

        InitializeKeyboard();
        InitializeMouse();
    }
    

    private void InitializeKeyboard()
    {
        if (InputContext.Keyboards.Count > 0)
        {
            keyboard = InputContext.Keyboards[0];
            keyboard.KeyDown += OnKeyDown;
            keyboard.KeyUp += OnKeyUp;
        }
        else
        {
            keyboard = null;
        }
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int _)
    {
        keysDown.Add(key);
        keysPressed.Add(key);
    }

    private void OnKeyUp(IKeyboard keyboard, Key key, int _)
    {
        keysDown.Remove(key);
    }
    
    
    public bool KeyDown(Key key) => keysDown.Contains(key);
    public bool KeyPressed(Key key) => keysPressed.Contains(key);
    
    

    private void InitializeMouse()
    {
        if (InputContext.Mice.Count > 0)
        {
            mouse = InputContext.Mice[0];
            mouse.MouseDown += OnMouseDown;
            mouse.MouseUp += OnMouseUp;
            mouse.MouseMove += OnMouseMove;
            // mouse.Scroll += OnScroll;
        }
        else
        {
            mouse = null;
        }
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        mouseButtonsDown.Add(button);
        mouseButtonsPressed.Add(button);
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        mouseButtonsDown.Remove(button);
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        MousePosition = position;
    }
    
    public bool MouseButtonDown(MouseButton button) => mouseButtonsDown.Contains(button);
    public bool MouseButtonPressed(MouseButton button) => mouseButtonsPressed.Contains(button);
    

    /// Called once per frame to clear pressed states
    public void Update()
    {
        keysPressed.Clear();
        mouseButtonsPressed.Clear();
    }
}
