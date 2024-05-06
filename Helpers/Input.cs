using SharpDX.DirectInput;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Helpers
{
    public class Input : IDisposable
    {
        private DirectInput _directInput;
        //клавиатура
        private Keyboard _keyboardDevice;
        private KeyboardState _currentStateKeyboard;
        private KeyboardState _previousStateKeyboard;

        //мышь
        private Mouse _mouseDevice;
        private MouseState _currentStateMouse;
        public MouseState MouseState { get { return _currentStateMouse; } }
        private MouseState _previousStateMouse;
        private Point _currentMouse;

        public Input(IntPtr windowHandle)
        {
            _directInput = new DirectInput();

            _keyboardDevice = new Keyboard(_directInput);
            _keyboardDevice.Acquire();

            _mouseDevice = new Mouse(_directInput);
            _currentStateMouse = new MouseState();
            _previousStateMouse = new MouseState();
            _mouseDevice.Acquire();
        }

        public void Update()
        {
            _previousStateKeyboard = _currentStateKeyboard;
            _currentStateKeyboard = _keyboardDevice.GetCurrentState();

            MouseState previousState = _currentStateMouse;

            _currentStateMouse = _mouseDevice.GetCurrentState();

            // Проверяем, изменилась ли позиция мыши
            if (_currentStateMouse.X != previousState.X || _currentStateMouse.Y != previousState.Y)
            {
                _currentMouse.X = _currentStateMouse.X;
                _currentMouse.Y = _currentStateMouse.Y;
            }
        }
        public bool IsKeyPressed(Key key) => _currentStateKeyboard.IsPressed(key);

        public bool IsKeyReleased(Key key) => !_currentStateKeyboard.IsPressed(key) && _previousStateKeyboard.IsPressed(key);

        public bool IsKeyTriggered(Key key) => _currentStateKeyboard.IsPressed(key) && !_previousStateKeyboard.IsPressed(key);

        public bool IsKeyReleasedTriggered(Key key) => !_currentStateKeyboard.IsPressed(key) && _previousStateKeyboard.IsPressed(key);


        public int GetMouseDeltaX() => _currentStateMouse.X - _previousStateMouse.X;

        public int GetMouseDeltaY() => _currentStateMouse.Y - _previousStateMouse.Y;

        public Vector2 GetMousePosition() => new Vector2(_currentMouse.X, _currentMouse.Y);

        public void Dispose()
        {
            _keyboardDevice.Unacquire();
            _keyboardDevice.Dispose();
            _mouseDevice.Unacquire();
            _mouseDevice.Dispose();
            _directInput.Dispose();
        }
    }
}
