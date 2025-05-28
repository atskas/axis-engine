using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace UntitledEngine.Core.Input;

public class InputManager
{
    // Globals
    public bool KeyDown(Key key) => keysDown.Contains(key);
    public bool KeyPressed(Key key) => keysPressed.Contains(key);
    public IInputContext InputContext;
    
    // Private fields
    private IWindow window;
    private IKeyboard keyboard;
    private IMouse mouse;

    private HashSet<Key> keysPressed = new();
    private HashSet<Key> keysDown = new();

    public InputManager(IWindow window)
    {
        this.window = window;
        InputContext = window.CreateInput();
        
        // Check if any keyboards exist before subscribing to events
        if (InputContext.Keyboards.Count > 0)
        {
            InputContext.Keyboards[0].KeyDown += OnKeyDown;
            InputContext.Keyboards[0].KeyUp += OnKeyUp;
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
    
    public void Update()
    {
        keysPressed.Clear();
    }
}