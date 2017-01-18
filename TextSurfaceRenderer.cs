using System.Drawing;
using System.Drawing.Drawing2D;

using System;
using System.Runtime.Serialization;

namespace SadConsole.Consoles
{
    /// <summary>
    /// Draws a text surface to the screen.
    /// </summary>
    [DataContract]
    public class TextSurfaceRenderer : ITextSurfaceRenderer
    {
        //private Matrix absolutePositionTransform;
        //private Matrix positionTransform;
        //private Point oldPosition;
        //private Point oldAbsolutePosition;

        public bool CallBatchEnd = true;

        /// <summary>
        /// The sprite batch used for drawing to the screen.
        /// </summary>
        public SpriteBatch Batch { get; private set; }


        /// <summary>
        /// A method called when the <see cref="SpriteBatch"/> has been created and transformed, but before any text characters are drawn.
        /// </summary>
        public Action<SpriteBatch> BeforeRenderCallback { get; set; }

        /// <summary>
        /// A method called when all text characters have been drawn and any tinting has been applied.
        /// </summary>
        public Action<SpriteBatch> AfterRenderCallback { get; set; }

        public Graphics RenderTarget { get; set; }

        /// <summary>
        /// Creates a new renderer.
        /// </summary>
        public TextSurfaceRenderer()
        {
            Batch = new SpriteBatch();
        }

        /// <summary>
        /// Renders a surface to the screen.
        /// </summary>
        /// <param name="surface">The surface to render.</param>
        /// <param name="renderingMatrix">Display matrix for the rendered console.</param>
        public virtual void Render(ITextSurfaceRendered surface, Matrix renderingMatrix)
        {
            if (RenderTarget != null)
                Batch.Reset(RenderTarget, renderingMatrix);
            else
                Batch.Reset(surface.Backbuffer, renderingMatrix);

            BeforeRenderCallback?.Invoke(Batch);

            if (surface.Tint.A != 255)
            {
                Cell cell;

                if (surface.DefaultBackground.A != 0)
                    Batch.DrawQuad(surface.AbsoluteArea, surface.Font.SolidGlyphRectangle, surface.DefaultBackground, surface.Font.Image);

                for (int i = 0; i < surface.RenderCells.Length; i++)
                {
                    cell = surface.RenderCells[i];

                    if (cell.IsVisible)
                    {
                        Batch.DrawCell(cell, surface.RenderRects[i], surface.Font.SolidGlyphRectangle, surface.DefaultBackground, surface.Font);
                    }
                }

            }
            AfterRenderCallback?.Invoke(Batch);
        

            if (surface.Tint.A != 0)
                Batch.DrawQuad(surface.AbsoluteArea, surface.Font.SolidGlyphRectangle, surface.Tint, surface.Font.Image);

            if (CallBatchEnd)
                Batch.End();
        }

        /// <summary>
        /// Renders a surface to the screen.
        /// </summary>
        /// <param name="surface">The surface to render.</param>
        /// <param name="position">Calculates the rendering position on the screen based on the size of the <paramref name="surface"/> parameter.</param>
        /// <param name="usePixelPositioning">Ignores the <paramref name="surface"/> font for positioning and instead treats the <paramref name="position"/> parameter in pixels.</param>
        public void Render(ITextSurfaceRendered surface, Point position, bool usePixelPositioning = false)
        {
            Matrix matrix;

            if (usePixelPositioning)
            {
                //if (oldAbsolutePosition != position)
                //{
                //    absolutePositionTransform = GetPositionTransform(position, surface.Font.Size, true);
                //    oldAbsolutePosition = position;
                //}

                //matrix = absolutePositionTransform;

                matrix = GetPositionTransform(position, surface.Font.Size, true);
            }
            else
            {
                //if (position != oldPosition)
                //{
                //    positionTransform = GetPositionTransform(position, surface.Font.Size, false);
                //    oldPosition = position;
                //}

                //matrix = positionTransform;
                
                matrix = GetPositionTransform(position, surface.Font.Size, false);
            }

            Render(surface, matrix);
        }

        /// <summary>
        /// Gets the Matrix transform that positions the console on the screen.
        /// </summary>
        /// <returns>The transform.</returns>
        public virtual Matrix GetPositionTransform(Point position, Point CellSize, bool absolutePositioning)
        {
            Point worldLocation;

            if (absolutePositioning)
                worldLocation = position;
            else
                worldLocation = position.ConsoleLocationToWorld(CellSize.X, CellSize.Y);

            return worldLocation.ToPositionMatrix(CellSize, absolutePositioning);
        }

        [OnDeserialized]
        private void AfterDeserialized(StreamingContext context)
        {
#if SFML
            Batch = new SpriteBatch();
#elif MONOGAME
            Batch = new SpriteBatch(Engine.Device);
#endif
        }
    }
}
