using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;

namespace LoveHeart
{
    public abstract class AbstractLove
    {
        private readonly MyTime clock = new MyTime();
        private FormWindowState _currentFormWindowState;
        private bool _disposed;
        private Form _form;
        private float _frameAccumulator;
        private int _frameCount;
        private Configuration _configuration;

        ~AbstractLove()
        {
            if (!_disposed)
            {
                Dispose(false);
                _disposed = true;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (_form != null)
                    _form.Dispose();
            }
        }

        protected IntPtr DisplayHandle
        {
            get
            {
                return _form.Handle;
            }
        }

        public Configuration Config
        {
            get
            {
                return _configuration;
            }
        }

        public float FrameDelta { get; private set; }

        public float FramePerSecond { get; private set; }

        protected virtual Form CreateForm(Configuration config)
        {
            return new RenderForm(config.Title)
            {
                Icon = Icon.ExtractAssociatedIcon("MyCrush\\MyCrush.ico"),
                ClientSize = new System.Drawing.Size(config.Width, config.Height)
            };
        }

        public void Run()
        {
            Run(new Configuration());
        }

        public void Run(Configuration demoConfiguration)
        {
            _configuration = demoConfiguration ?? new Configuration();
            _form = CreateForm(_configuration);
            Initialize(_configuration);

            bool isFormClosed = false;
            bool formIsResizing = false;

            _form.MouseClick += HandleMouseClick;
            _form.KeyDown += HandleKeyDown;
            _form.KeyUp += HandleKeyUp;
            _form.Resize += (o, args) =>
            {
                if (_form.WindowState != _currentFormWindowState)
                {
                    HandleResize(o, args);
                }

                _currentFormWindowState = _form.WindowState;
            };

            _form.ResizeBegin += (o, args) => { formIsResizing = true; };
            _form.ResizeEnd += (o, args) =>
            {
                formIsResizing = false;
                HandleResize(o, args);
            };

            _form.Closed += (o, args) => { isFormClosed = true; };

            LoadContent();

            clock.Start();
            BeginRun();
            RenderLoop.Run(_form, () =>
            {
                if (isFormClosed)
                {
                    return;
                }

                OnUpdate();
                if (!formIsResizing)
                    Render();
            });

            UnloadContent();
            EndRun();

            Dispose();
        }

        protected abstract void Initialize(Configuration demoConfiguration);

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }
        protected virtual void Update(MyTime time)
        {
        }
        protected virtual void Draw(MyTime time)
        {
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        protected virtual void BeginDraw()
        {
        }

        protected virtual void EndDraw()
        {
        }

        public void Exit()
        {
            _form.Close();
        }

        private void OnUpdate()
        {
            FrameDelta = (float)clock.Update();
            Update(clock);
        }

        protected System.Drawing.Size RenderingSize
        {
            get
            {
                return _form.ClientSize;
            }
        }

        private void Render()
        {
            _frameAccumulator += FrameDelta;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramePerSecond = _frameCount / _frameAccumulator;

                _form.Text = _configuration.Title + " - FPS: " + FramePerSecond;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }

            BeginDraw();
            Draw(clock);
            EndDraw();
        }

        protected virtual void MouseClick(MouseEventArgs e)
        {
        }

        protected virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Exit();
        }

        protected virtual void KeyUp(KeyEventArgs e)
        {
        }

        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick(e);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Minimized)
            {
                return;
            }
        }
    }
}
