///*
//using SadConsole.Consoles;
//using System;
//using SadConsole;
//using Microsoft.Xna.Framework;
//using SadConsole.Input;
//*/

//using System.Drawing;

//namespace StarterProject
//{
//    class Program
//    {
//        //private static Windows.CharacterViewer _characterWindow;
//        //private static int currentConsoleIndex;



//        static void Main(string[] args)
//        {
//            //var a = SharpDX.Direct2D1.SolidColorBrush;
//            //a.Color

//            //// Setup the engine and creat the main window.
//            //SadConsole.Engine.Initialize("IBM.font", 80, 25);

//            //// Hook the start event so we can add consoles to the system.
//            //SadConsole.Engine.EngineStart += Engine_EngineStart;

//            //// Hook the update event that happens each frame so we can trap keys and respond.
//            //SadConsole.Engine.EngineUpdated += Engine_EngineUpdated;

//            //SadConsole.Engine.EngineDrawFrame += Engine_EngineDrawFrame;

//            //// Start the game.
//            //SadConsole.Engine.Run();

//            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(200, 200);
//            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
//            graphics.FillRectangle(System.Drawing.Brushes.Green, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
//            SadConsole.FontMaster fontMaster = SadConsole.FontMaster.LoadFont("IBM.font");
//            var font = fontMaster.GetFont(SadConsole.Font.FontSizes.One);

//            System.Drawing.SpriteBatch batch = new System.Drawing.SpriteBatch();
//            var cell = new SadConsole.Cell() { Background = Color.Yellow, Foreground = Color.Purple, GlyphIndex = 'A' };
//            batch.Reset(graphics, new System.Drawing.Drawing2D.Matrix());
//            batch.DrawCell(cell, new Rectangle(0, 0, font.Size.X, font.Size.Y), font.SolidGlyphRectangle, Color.Orange, font);
//            batch.End();

//            bitmap.Save(@"c:\temp\output.png", System.Drawing.Imaging.ImageFormat.Png);

//            bitmap.Dispose();
//            graphics.Dispose();
//            fontMaster.Image.Dispose();

//            //OpenTK.Graphics.

//            //OpenTK.GameWindow win = new OpenTK.GameWindow(200, 200, OpenTK.Graphics.GraphicsMode.Default, "test", OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default);
//            //win.Run();

//        }

//        /*
//        private static void Engine_EngineDrawFrame(object sender, EventArgs e)
//        {
//            // Custom drawing. You don't usually have to do this.
//        }

//        private static void Engine_EngineUpdated(object sender, EventArgs e)
//        {
//            if (!_characterWindow.IsVisible)
//            {
//                // This block of code cycles through the consoles in the SadConsole.Engine.ConsoleRenderStack, showing only a single console
//                // at a time. This code is provided to support the custom consoles demo. If you want to enable the demo, uncomment one of the lines
//                // in the Initialize method above.
//                if (SadConsole.Engine.Keyboard.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F1))
//                {
//                    MoveNextConsole();
//                }
//                else if (SadConsole.Engine.Keyboard.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F2))
//                {
//                    _characterWindow.Show(true);
//                }
//                else if (SadConsole.Engine.Keyboard.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F3))
//                {
//                }
//            }
//        }

//        private static void Engine_EngineStart(object sender, EventArgs e)
//        {
//            // Setup our custom theme.
//            Theme.SetupThemes();

//            // By default SadConsole adds a blank ready-to-go console to the rendering system. 
//            // We don't want to use that for the sample project so we'll remove it.
//            SadConsole.Engine.ConsoleRenderStack.Clear();
//            SadConsole.Engine.ActiveConsole = null;

//            // We'll instead use our demo consoles that show various features of SadConsole.
//            SadConsole.Engine.ConsoleRenderStack
//                = new ConsoleList() {
//                    new CustomConsoles.SubConsoleCursor(),
//                                        new CustomConsoles.ControlsTest(),
//                                        new CustomConsoles.SplashScreen() { SplashCompleted = () => { MoveNextConsole(); } },
//                                        new CustomConsoles.StretchedConsole(),
//                                        //new CustomConsoles.CachedConsoleConsole(),
//                                        new CustomConsoles.StringParsingConsole(),
//                                        //new CustomConsoles.CursorConsole(),
//                                        //new CustomConsoles.DOSConsole(),
//                                        //new CustomConsoles.SceneProjectionConsole(),
//                                        new CustomConsoles.ScrollableConsole(10, 10, 20),
//                                        new CustomConsoles.ViewsAndSubViews(),
//                                        new CustomConsoles.StaticConsole(),
//                                        new CustomConsoles.BorderedConsole(),
//                                        new CustomConsoles.GameObjectConsole(),
//                                        new CustomConsoles.RandomScrollingConsole(),
//                                        new CustomConsoles.WorldGenerationConsole(),
//                                    };

//            // Show the first console (by default all of our demo consoles are hidden)
//            SadConsole.Engine.ConsoleRenderStack[0].IsVisible = true;

//            // Set the first console in the console list as the "active" console. This allows the keyboard to be processed on the console.
//            SadConsole.Engine.ActiveConsole = SadConsole.Engine.ConsoleRenderStack[0];

//            // Initialize the windows
//            _characterWindow = new Windows.CharacterViewer();

//            //SadConsole.Effects.Fade a = new SadConsole.Effects.Fade();
//            //a.DestinationForeground = Microsoft.Xna.Framework.Color.Turquoise;
//            //SadConsole.Engine.MonoGameInstance.Components.Add(new FPSCounterComponent(SadConsole.Engine.MonoGameInstance));

//        }

//        private static void MoveNextConsole()
//        {
//            currentConsoleIndex++;

//            if (currentConsoleIndex >= SadConsole.Engine.ConsoleRenderStack.Count)
//                currentConsoleIndex = 0;

//            for (int i = 0; i < SadConsole.Engine.ConsoleRenderStack.Count; i++)
//                SadConsole.Engine.ConsoleRenderStack[i].IsVisible = currentConsoleIndex == i;

//            Engine.ActiveConsole = SadConsole.Engine.ConsoleRenderStack[currentConsoleIndex];
//        }
//        */
//    }
//}

////using System.Drawing;
////using System.Drawing.Imaging;
////using OpenTK.Graphics;
////using OpenTK.Graphics.OpenGL;

////using SDPixelFormat = System.Drawing.Imaging.PixelFormat;
////using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

////namespace StarterProject
////{
////    static class Program
////    {
////        static void Main(string[] args)
////        {
////            var mode = new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 0, ColorFormat.Empty, 1);
////            var win = new OpenTK.GameWindow(640, 480, mode, "", OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 3, 0, GraphicsContextFlags.Default);
////            win.Visible = false;
////            win.MakeCurrent();
////            /* START OF YOUR ACTUAL GL CODE */
////            GL.ClearColor(0.7f, 0.7f, 1.0f, 1.0f);
////            GL.Clear(ClearBufferMask.ColorBufferBit);
////            /* END OF YOUR ACTUAL GL CODE */
////            GL.Flush();
////            using (var bmp = new Bitmap(640, 480, SDPixelFormat.Format32bppArgb))
////            {
////                var mem = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.WriteOnly, SDPixelFormat.Format32bppArgb);
////                GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
////                GL.ReadPixels(0, 0, 640, 480, PixelFormat.Bgra, PixelType.UnsignedByte, mem.Scan0);
////                bmp.UnlockBits(mem);
////                bmp.Save(@"D:\test.png", ImageFormat.Png);
////            }
////        }
////    }
////}
