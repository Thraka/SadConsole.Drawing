#if DRAWING
using SadConsole;
//using SadConsole.Consoles;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
    public class SpriteBatch
    {
        //private Vertex[] singleDrawVerticies = new Vertex[4];
        private Matrix transform;

        private DrawCall lastDrawCall = new DrawCall();

        private List<DrawCall> drawCalls;

        private Graphics target;

        private Imaging.ImageAttributes attribs = new Imaging.ImageAttributes();
        private Imaging.ColorMatrix colorMatrix = new Imaging.ColorMatrix(new float[][] { new float[] { 1, 0, 0, 0, 0 },
                                                                                          new float[] { 0, 1, 0, 0, 0 },
                                                                                          new float[] { 0, 0, 1, 0, 0 },
                                                                                          new float[] { 0, 0, 0, 1, 0 },
                                                                                          new float[] { 1, 1, 1, 0, 1 }});

        //private RenderStates state;

        private int maxIndex = 800;

        public void Reset(Graphics target, Matrix renderingTransform)
        {
            target.InterpolationMode = InterpolationMode.NearestNeighbor;
            target.Transform = renderingTransform;
            transform = renderingTransform;
            //drawCalls = new List<DrawCall>();
            //lastDrawCall = new DrawCall();
            lastDrawCall.VertIndex = 0;
            this.target = target;
            //attribs.SetColorMatrix(colorMatrix, Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Bitmap);
            //this.state = state;
            //this.state.Transform *= renderingTransform;
        }

        private class DrawCall
        {
            public Image Texture;
            public DrawCommand[] Verticies = new DrawCommand[1000];
            public int VertIndex;
        }

        [Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, Pack = 1, Size=40)]
        private struct DrawCommand
        {
            public Rectangle Source;
            public Rectangle Target;
            public Color Color;
        }

        public void DrawQuad(Rectangle screenRect, Rectangle textCoords, Color color, Image texture)
        {
            if (lastDrawCall.Texture != texture && lastDrawCall.Texture != null)
            {
                //drawCalls.Add(lastDrawCall);
                End();
                lastDrawCall.VertIndex = 0;
            }

            lastDrawCall.Texture = texture;

            if (lastDrawCall.VertIndex >= maxIndex)
            {
                global::System.Array.Resize(ref lastDrawCall.Verticies, lastDrawCall.Verticies.Length + lastDrawCall.Verticies.Length / 2);
                maxIndex = lastDrawCall.Verticies.Length - 200;
            }

            lastDrawCall.Verticies[lastDrawCall.VertIndex].Source = textCoords;
            lastDrawCall.Verticies[lastDrawCall.VertIndex].Target = screenRect;
            lastDrawCall.Verticies[lastDrawCall.VertIndex].Color = color;
            lastDrawCall.VertIndex++;
        }

        public void DrawCell(Cell cell, Rectangle screenRect, Rectangle solidRect, Color defaultBackground, SadConsole.Font font)
        {
            if (lastDrawCall.Texture != font.Image && lastDrawCall.Texture != null)
            {
                End();
                lastDrawCall.VertIndex = 0;
            }

            lastDrawCall.Texture = font.Image;

            if (lastDrawCall.VertIndex >= maxIndex)
            {
                global::System.Array.Resize(ref lastDrawCall.Verticies, lastDrawCall.Verticies.Length + lastDrawCall.Verticies.Length / 2);
                maxIndex = lastDrawCall.Verticies.Length - 200;
            }

            var glyphRect = font.GlyphIndexRects[cell.ActualGlyphIndex];

            //if ((cell.ActualSpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            //{
            //    var temp = glyphRect.X;
            //    glyphRect.X = glyphRect.Width;
            //    glyphRect.Width = temp;
            //}

            //if ((cell.ActualSpriteEffect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
            //{
            //    var temp = glyphRect.Y;
            //    glyphRect.Y = glyphRect.Height;
            //    glyphRect.Height = temp;
            //}

//public static Image FlipImage(Image image, bool flipHorizontally, bool flipVertically)
//{
//    Bitmap flippedImage = new Bitmap(image.Width, image.Height);

//    using (Graphics g = Graphics.FromImage(flippedImage))
//    {
//        //Matrix transformation
//        Matrix m = null;
//        if (flipVertically && flipHorizontally)
//        {
//            m = new Matrix(-1, 0, 0, -1, 0, 0);
//            m.Translate(flippedImage.Width, flippedImage.Height, MatrixOrder.Append);
//        }
//        else if (flipVertically)
//        {
//            m = new Matrix(1, 0, 0, -1, 0, 0);
//            m.Translate(0, flippedImage.Height, MatrixOrder.Append);
//        }
//        else if (flipHorizontally)
//        {
//            m = new Matrix(-1, 0, 0, 1, 0, 0);
//            m.Translate(flippedImage.Width, 0, MatrixOrder.Append);
//        }

//        //Draw
//        g.Transform = m;
//        g.DrawImage(image, 0, 0);

//        //clean up
//        m.Dispose();
//    }

//    return (Image)flippedImage;
//}


            if (cell.ActualBackground != Color.Transparent && cell.ActualBackground != defaultBackground)
            {
                // Background
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Source = solidRect;
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Target = screenRect;
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Color = cell.ActualBackground;
                lastDrawCall.VertIndex++;
            }


            if (cell.ActualForeground != Color.Transparent)
            {
                // Foreground
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Source = glyphRect;
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Target = screenRect;
                lastDrawCall.Verticies[lastDrawCall.VertIndex].Color = cell.ActualForeground;
                lastDrawCall.VertIndex++;
            }
        }

        public void End()
        {
            if (lastDrawCall.VertIndex != 0)
            {
                for (int i = 0; i < lastDrawCall.VertIndex; i++)
                {
                    var call = lastDrawCall.Verticies[i];
                    colorMatrix.Matrix40 = lastDrawCall.Verticies[i].Color.R / 255f;
                    colorMatrix.Matrix41 = lastDrawCall.Verticies[i].Color.G / 255f;
                    colorMatrix.Matrix42 = lastDrawCall.Verticies[i].Color.B / 255f;
                    colorMatrix.Matrix33 = lastDrawCall.Verticies[i].Color.A / 255f;

                    //attribs.SetWrapMode(WrapMode.TileFlipXY);

                    attribs.SetColorMatrix(colorMatrix, Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Bitmap);
                    target.DrawImage(lastDrawCall.Texture, call.Target, call.Source.X, call.Source.Y, call.Source.Width, call.Source.Height, GraphicsUnit.Pixel, attribs);
                }
            }
            target.Transform = new Matrix();
        }
    }

    /* Other sprite batch from SFML
    
    public class SpriteBatch2
    {
        Vertex[] m_verticies;
        int vertIndexCounter;
        Texture texture;
        Matrix transform;

        public SpriteBatch2()
        {
            m_verticies = new Vertex[0];
        }

        public void Reset(Matrix renderingTransform)
        {
            transform = renderingTransform;
            m_verticies = new Vertex[10000];
            vertIndexCounter = 0;
        }

        public void Reset()
        {
            Reset(Matrix.Identity);
        }
        

        public unsafe void DrawQuad(Rectangle screenRect, Rectangle textCoords, Color color, Texture texture)
        {
            fixed (Vertex* verts = m_verticies)
            {
                verts[vertIndexCounter].Position.X = screenRect.Left;
                verts[vertIndexCounter].Position.Y = screenRect.Top;
                verts[vertIndexCounter].TexCoords.X = textCoords.Left;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Top;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                verts[vertIndexCounter].Position.Y = screenRect.Top;
                verts[vertIndexCounter].TexCoords.X = textCoords.Width;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Top;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Width;
                verts[vertIndexCounter].Position.Y = screenRect.Height;
                verts[vertIndexCounter].TexCoords.X = textCoords.Width;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Height;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Left;
                verts[vertIndexCounter].Position.Y = screenRect.Height;
                verts[vertIndexCounter].TexCoords.X = textCoords.Left;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Height;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;
            }

            this.texture = texture;
        }

        public unsafe void DrawCell(Cell cell, Rectangle screenRect, Rectangle solidRect, Color defaultBackground, SadConsole.Font font)
        {
            if (cell.IsVisible)
            {
                var glyphRect = font.GlyphIndexRects[cell.ActualGlyphIndex];

                if ((cell.ActualSpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                {
                    var temp = glyphRect.Left;
                    glyphRect.Left = glyphRect.Width;
                    glyphRect.Width = temp;
                }

                if ((cell.ActualSpriteEffect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
                {
                    var temp = glyphRect.Top;
                    glyphRect.Top = glyphRect.Height;
                    glyphRect.Height = temp;
                }

                fixed (Vertex* verts = m_verticies)
                {
                    if (cell.ActualBackground != Color.Transparent && cell.ActualBackground != defaultBackground)
                    {
                        // Background
                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;
                    }

                    if (cell.ActualForeground != Color.Transparent)
                    {
                        // Foreground
                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;


                    }
                }


            }

            this.texture = font.FontImage;
        }

        public void End(RenderTarget target, RenderStates state)
        {
            state.Transform *= transform;
            state.Texture = texture;
            target.Draw(m_verticies, 0, (uint)vertIndexCounter, PrimitiveType.Quads, state);
        }
    }

    public class SpriteBatch_ORG
    {
        Vertex[] m_verticies;
        int vertIndexCounter;
        Texture texture;
        Rectangle solidRect;
        Rectangle fillRect;
        Matrix transform;

        public SpriteBatch_ORG()
        {
            m_verticies = new Vertex[0];
        }

        public void Start(int renderQuads, Texture texture, Matrix transform)
        {
            int count = 4 * renderQuads;

            if (m_verticies.Length != count)
                m_verticies = new Vertex[count];

            vertIndexCounter = 0;
            this.texture = texture;
            this.transform = transform;
        }

        public void Start(ITextSurfaceRendered surface, Matrix transform, int additionalDraws = 250)
        {
            fillRect = surface.AbsoluteArea;
            texture = surface.Font.FontImage;
            solidRect = surface.Font.GlyphIndexRects[surface.Font.SolidGlyphIndex];
            this.transform = transform;

            int count = ((surface.RenderCells.Length + 8 + additionalDraws) * 4 * 2);
            if (m_verticies.Length != count)
                m_verticies = new Vertex[count];

            vertIndexCounter = 0;
        }

        public unsafe void DrawSurfaceFill(Color color, Color filter)
        {
            if (color != filter)
            {
                fixed (Vertex* verts = m_verticies)
                {
                    verts[vertIndexCounter].Position.X = fillRect.Left;
                    verts[vertIndexCounter].Position.Y = fillRect.Top;
                    verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                    verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                    verts[vertIndexCounter].Color = color;
                    vertIndexCounter++;

                    verts[vertIndexCounter].Position.X = fillRect.Width; // SadConsole w/SFML changed Width to be left + width...
                    verts[vertIndexCounter].Position.Y = fillRect.Top;
                    verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                    verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                    verts[vertIndexCounter].Color = color;
                    vertIndexCounter++;

                    verts[vertIndexCounter].Position.X = fillRect.Width;
                    verts[vertIndexCounter].Position.Y = fillRect.Height;
                    verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                    verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                    verts[vertIndexCounter].Color = color;
                    vertIndexCounter++;

                    verts[vertIndexCounter].Position.X = fillRect.Left;
                    verts[vertIndexCounter].Position.Y = fillRect.Height;
                    verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                    verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                    verts[vertIndexCounter].Color = color;
                    vertIndexCounter++;
                }
            }
        }

        public unsafe void DrawQuad(Rectangle screenRect, Rectangle textCoords, Color color)
        {
            fixed (Vertex* verts = m_verticies)
            {
                verts[vertIndexCounter].Position.X = screenRect.Left;
                verts[vertIndexCounter].Position.Y = screenRect.Top;
                verts[vertIndexCounter].TexCoords.X = textCoords.Left;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Top;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                verts[vertIndexCounter].Position.Y = screenRect.Top;
                verts[vertIndexCounter].TexCoords.X = textCoords.Width;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Top;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Width;
                verts[vertIndexCounter].Position.Y = screenRect.Height;
                verts[vertIndexCounter].TexCoords.X = textCoords.Width;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Height;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;

                verts[vertIndexCounter].Position.X = screenRect.Left;
                verts[vertIndexCounter].Position.Y = screenRect.Height;
                verts[vertIndexCounter].TexCoords.X = textCoords.Left;
                verts[vertIndexCounter].TexCoords.Y = textCoords.Height;
                verts[vertIndexCounter].Color = color;
                vertIndexCounter++;
            }
        }

        public unsafe void DrawCell(Cell cell, Rectangle screenRect, Color defaultBackground, SadConsole.Font font)
        {
            if (cell.IsVisible)
            {
                var glyphRect = font.GlyphIndexRects[cell.ActualGlyphIndex];

                if ((cell.ActualSpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                {
                    var temp = glyphRect.Left;
                    glyphRect.Left = glyphRect.Width;
                    glyphRect.Width = temp;
                }

                if ((cell.ActualSpriteEffect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
                {
                    var temp = glyphRect.Top;
                    glyphRect.Top = glyphRect.Height;
                    glyphRect.Height = temp;
                }

                fixed (Vertex* verts = m_verticies)
                {
                    if (cell.ActualBackground != Color.Transparent && cell.ActualBackground != defaultBackground)
                    {
                        // Background
                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = solidRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = solidRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualBackground;
                        vertIndexCounter++;
                    }

                    if (cell.ActualForeground != Color.Transparent)
                    {
                        // Foreground
                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width; // SadConsole w/SFML changed Width to be left + width...
                        verts[vertIndexCounter].Position.Y = screenRect.Top;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Top;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Width;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Width;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        verts[vertIndexCounter].Position.X = screenRect.Left;
                        verts[vertIndexCounter].Position.Y = screenRect.Height;
                        verts[vertIndexCounter].TexCoords.X = glyphRect.Left;
                        verts[vertIndexCounter].TexCoords.Y = glyphRect.Height;
                        verts[vertIndexCounter].Color = cell.ActualForeground;
                        vertIndexCounter++;

                        
                    }
                }


            }
        }

        public void End(RenderTarget target, RenderStates state)
        {
            state.Transform *= transform;
            state.Texture = texture;
            target.Draw(m_verticies, 0, (uint)vertIndexCounter, PrimitiveType.Quads, state);
        }
    }

    */
}
#endif
