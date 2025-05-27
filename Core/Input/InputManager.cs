using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace UntitledEngine.Core.Input;

public class InputManager
{
    // Globals
    public bool KeyDown(Key key) => _keysDown.Contains(key);
    public bool KeyPressed(Key key) => _keysPressed.Contains(key);
    
    // Private fields
    private IWindow _window;
    private IInputContext _inputContext;
    
    private IKeyboard _keyboard;
    private IMouse _mouse;

    private HashSet<Key> _keysPressed = new();
    private HashSet<Key> _keysDown = new();

    public InputManager(IWindow window)
    {
        _window = window;
        _inputContext = window.CreateInput();
        
        // Check if any keyboards exist before subscribing to events
        if (_inputContext.Keyboards.Count > 0)
        {
            _inputContext.Keyboards[0].KeyDown += OnKeyDown;
            _inputContext.Keyboards[0].KeyUp += OnKeyUp;
        }
        else
        {
            _keyboard = null;
        }
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int _)
    {
        _keysDown.Add(key);
        _keysPressed.Add(key);
    }
    
    private void OnKeyUp(IKeyboard keyboard, Key key, int _)
    {
        _keysDown.Remove(key);
    }
    
    public void Update()
    {
        _keysPressed.Clear();
    }
}