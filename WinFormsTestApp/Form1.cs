using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            fontMaster = SadConsole.FontMaster.LoadFont("IBM.font");
            console = new SadConsole.Consoles.SurfaceEditor(new SadConsole.Consoles.TextSurface(80, 100));
            renderer = new SadConsole.Consoles.TextSurfaceRenderer();
        }

        int rotation = 0;
        byte color = 0;
        Color foreground = Color.Purple;
        SadConsole.FontMaster fontMaster;
        SadConsole.Consoles.SurfaceEditor console;
        SadConsole.Consoles.TextSurfaceRenderer renderer;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;

            SadConsole.FontMaster.LoadFont("IBM.font"); // Sets default font if not set

            var textSurface = new SadConsole.Consoles.TextSurface(80, 24);
            var console = new SadConsole.Consoles.SurfaceEditor(textSurface);
            var renderer = new SadConsole.Consoles.TextSurfaceRenderer();

            var doc = new SadConsole.Ansi.Document("QS-SIERR.ANS");
            var writer = new SadConsole.Ansi.AnsiWriter(doc, console);
            writer.ReadEntireDocument();

            // Get times scrolled
            if (console.TimesShiftedUp != 0)
            {
                textSurface.Dispose();
                console.TextSurface = textSurface = new SadConsole.Consoles.TextSurface(80, 24 + console.TimesShiftedUp);

                console.ClearShiftValues();

                writer = new SadConsole.Ansi.AnsiWriter(doc, console);
                writer.ReadEntireDocument();
            }

            // Create graphics image
            //Bitmap outputImage = new Bitmap(console.TextSurface.Width * SadConsole.FontMaster.DefaultFont.Size.X,
            //                                 console.TextSurface.Height * SadConsole.FontMaster.DefaultFont.Size.Y);
            //Graphics graphics = Graphics.FromImage(outputImage);
            graphics.Clear(Color.Black);

            // Draw ansi surface to image
            renderer.RenderTarget = graphics;
            renderer.Render((SadConsole.Consoles.TextSurface)console.TextSurface, new Point(0, 0), false);

            // Save image
            //outputImage.Save("ansi.png", ImageFormat.Png);

            // Clean up
            //graphics.Dispose();
            //outputImage.Dispose();
            textSurface.Dispose();
        }
    }
}
