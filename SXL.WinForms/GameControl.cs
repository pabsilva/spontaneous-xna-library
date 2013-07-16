#region File Description
//-----------------------------------------------------------------------------
// SpriteFontControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SXL.WinForms
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to use ContentManager
    /// inside a WinForms application. It loads a SpriteFont object through the
    /// ContentManager, then uses a SpriteBatch to draw text. The control is not
    /// animated, so it only redraws itself in response to WinForms paint messages.
    /// </summary>
    public class GameControl : GraphicsDeviceControl
    {
        public readonly TimeSpan TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);

        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private Stopwatch _timer;

        private TimeSpan _previousTotalTime;

        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont.
        /// </summary>
        protected override void Initialize()
        {
            //initialize the content manager
            _content = new ContentManager(Services, "Content");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Start the animation timer.
            _timer = Stopwatch.StartNew();
            _previousTotalTime = new TimeSpan();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            LoadContent(_content);
        }


        protected virtual void LoadContent(ContentManager manager)
        {
        }


        /// <summary>
        /// Disposes the control, unloading the ContentManager.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content.Unload();
            }

            base.Dispose(disposing);
        }
        

        /// <summary>
        /// Redraws the control in response to a WinForms paint message.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            TimeSpan totalTime = _timer.Elapsed;
            TimeSpan elapsedTime = totalTime - _previousTotalTime;

            if (elapsedTime >= TargetElapsedTime)
            {
                ActualPaint(e, new GameTime(totalTime, elapsedTime));

                _previousTotalTime = totalTime;
            }
        }


        private void ActualPaint(PaintEventArgs e, GameTime gameTime)
        {
            string beginDrawError = BeginDraw();

            if (string.IsNullOrEmpty(beginDrawError))
            {
                // Draw the control using the GraphicsDevice.
                Update(gameTime);
                Draw(gameTime, _spriteBatch);
                EndDraw();
            }
            else
            {
                // If BeginDraw failed, show an error message using System.Drawing.
                PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }


        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
        }


        #region Properties
        
        public ContentManager Content
        {
            get { return _content; }
        }

        public Stopwatch Timer
        {
            get { return _timer; }
        }

        #endregion
    }
}
