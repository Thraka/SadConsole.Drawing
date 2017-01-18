using System;
using System.Collections.Generic;

namespace System.Drawing
{
    public static class ColorAnsi
    {
        public static readonly Color Black;
        public static readonly Color Red;
        public static readonly Color Green;
        public static readonly Color Yellow;
        public static readonly Color Blue;
        public static readonly Color Magenta;
        public static readonly Color Cyan;
        public static readonly Color White;

        public static readonly Color BlackBright;
        public static readonly Color RedBright;
        public static readonly Color GreenBright;
        public static readonly Color YellowBright;
        public static readonly Color BlueBright;
        public static readonly Color MagentaBright;
        public static readonly Color CyanBright;
        public static readonly Color WhiteBright;

        static ColorAnsi()
        {
            Black = Color.FromArgb(0, 0, 0);
            Red = Color.FromArgb(170, 0, 0);
            Green = Color.FromArgb(0, 170, 0);
            Yellow = Color.FromArgb(170, 85, 0);
            Blue = Color.FromArgb(0, 0, 170);
            Magenta = Color.FromArgb(170, 0, 170);
            Cyan = Color.FromArgb(0, 170, 170);
            White = Color.FromArgb(170, 170, 170);

            BlackBright = Color.FromArgb(85, 85, 85);
            RedBright = Color.FromArgb(255, 85, 85);
            GreenBright = Color.FromArgb(85, 255, 85);
            YellowBright = Color.FromArgb(255, 255, 85);
            BlueBright = Color.FromArgb(85, 85, 255);
            MagentaBright = Color.FromArgb(255, 85, 255);
            CyanBright = Color.FromArgb(85, 255, 255);
            WhiteBright = Color.FromArgb(255, 255, 255);
    }
    }

    public static class ColorExtensions
    {
        /// <summary>
        /// Custom color mappings for the <see cref="FromParser(Color, string, out bool, out bool, out bool, out bool, out bool)"/> method.
        /// </summary>
        public static Dictionary<string, Color> ColorMappings = new Dictionary<string, Color>(16) { { "ansiblack", ColorAnsi.Black },
                                                                                                    { "ansired", ColorAnsi.Red },
                                                                                                    { "ansigreen", ColorAnsi.Green },
                                                                                                    { "ansiyellow", ColorAnsi.Yellow },
                                                                                                    { "ansiblue", ColorAnsi.Blue },
                                                                                                    { "ansimagenta", ColorAnsi.Magenta },
                                                                                                    { "ansicyan", ColorAnsi.Cyan },
                                                                                                    { "ansiwhite", ColorAnsi.White },
                                                                                                    { "ansiblackbright", ColorAnsi.BlackBright },
                                                                                                    { "ansiredbright", ColorAnsi.RedBright },
                                                                                                    { "ansigreenbright", ColorAnsi.GreenBright },
                                                                                                    { "ansiyellowbright", ColorAnsi.YellowBright },
                                                                                                    { "ansibluebright", ColorAnsi.BlueBright },
                                                                                                    { "ansimagentabright", ColorAnsi.MagentaBright },
                                                                                                    { "ansicyanbright", ColorAnsi.CyanBright },
                                                                                                    { "ansiwhitebright", ColorAnsi.WhiteBright } };

        public static uint ToInteger(this Color color)
        {
            //return color.PackedValue;
            return 0;
        }

        public static Color[] LerpSteps(this Color color, Color endingColor, int steps)
        {
            Color[] colors = new Color[steps];

            float stopStrength = 1f / (steps - 1);

            float lerpTotal = 0f;

            colors[0] = color;
            colors[steps - 1] = endingColor;

            for (int i = 1; i < steps - 1; i++)
            {
                lerpTotal += stopStrength;
                
                colors[i] = ColorHelper.Lerp(color, endingColor, lerpTotal);
            }

            return colors;
        }
        

        private static float Hue_2_RGB(float v1, float v2, float vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
            if ((2 * vH) < 1) return (v2);
            if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2 / 3) - vH) * 6);
            return (v1);
        }

        public static Color GetRandomColor(this Color color, Random random)
        {
            return Color.FromArgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
        }

        /// <summary>
        /// Returns a new Color using only the Red value of this color.
        /// </summary>
        /// <param name="color">Object instance.</param>
        /// <returns></returns>
        public static Color RedOnly(this Color color)
        {
            return Color.FromArgb(color.R, 0, 0);
        }

        /// <summary>
        /// Returns a new Color using only the Green value of this color.
        /// </summary>
        /// <param name="color">Object instance.</param>
        /// <returns></returns>
        public static Color GreenOnly(this Color color)
        {
            return Color.FromArgb(0, color.G, 0);
        }

        /// <summary>
        /// Returns a new Color using only the Blue value of this color.
        /// </summary>
        /// <param name="color">Object instance.</param>
        /// <returns></returns>
        public static Color BlueOnly(this Color color)
        {
            return Color.FromArgb(0, 0, color.B);
        }

        public static float GetLuma(this Color color)
        {
            return (color.R + color.R + color.B + color.G + color.G + color.G) / 6f;
        }

        #region Color methods taken from mono source code
        public static float GetBrightness(this Color color)
        {
            byte minval = Math.Min(color.R, Math.Min(color.G, color.B));
            byte maxval = Math.Max(color.R, Math.Max(color.G, color.B));

            return (float)(maxval + minval) / 510;
        }


        public static float GetSaturation(this Color color)
        {
            byte minval = (byte)Math.Min(color.R, Math.Min(color.G, color.B));
            byte maxval = (byte)Math.Max(color.R, Math.Max(color.G, color.B));


            if (maxval == minval)
                return 0.0f;


            int sum = maxval + minval;
            if (sum > 255)
                sum = 510 - sum;


            return (float)(maxval - minval) / sum;
        }


        public static float GetHue(this Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;
            byte minval = (byte)Math.Min(r, Math.Min(g, b));
            byte maxval = (byte)Math.Max(r, Math.Max(g, b));


            if (maxval == minval)
                return 0.0f;


            float diff = (float)(maxval - minval);
            float rnorm = (maxval - r) / diff;
            float gnorm = (maxval - g) / diff;
            float bnorm = (maxval - b) / diff;


            float hue = 0.0f;
            if (r == maxval)
                hue = 60.0f * (6.0f + bnorm - gnorm);
            if (g == maxval)
                hue = 60.0f * (2.0f + rnorm - bnorm);
            if (b == maxval)
                hue = 60.0f * (4.0f + gnorm - rnorm);
            if (hue > 360.0f)
                hue = hue - 360.0f;


            return hue;
        }
        #endregion

        /// <summary>
        /// Converts a color to the format used by <see cref="SadConsole.ParseCommandRecolor"/> command.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A string in this format R,G,B,A so for <see cref="Color.Green"/> you would get <code>0,128,0,255</code>.</returns>
        public static string ToParser(this Color color)
        {
            return $"{color.R},{color.G},{color.B},{color.A}";
        }

        /// <summary>
        /// Gets a color in the format of <see cref="SadConsole.ParseCommandRecolor"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color FromParser(this Color color, string value, out bool keepR, out bool keepG, out bool keepB, out bool keepA, out bool useDefault)
        {
            useDefault = false;
            keepR = false;
            keepG = false;
            keepB = false;
            keepA = false;

            ArgumentException exception = new ArgumentException("Cannot parse color string");
            Color returnColor = color;

            if (value.Contains(","))
            {
                string[] channels = value.Trim(' ').Split(',');

                if (channels.Length >= 3)
                {

                    byte colorValue;

                    // Red
                    if (channels[0] == "x")
                        keepR = true;
                    else if (byte.TryParse(channels[0], out colorValue))
                        returnColor = Color.FromArgb(returnColor.A, colorValue, returnColor.G, returnColor.B);
                    else
                        throw exception;

                    // Green
                    if (channels[1] == "x")
                        keepG = true;
                    else if (byte.TryParse(channels[1], out colorValue))
                        returnColor = Color.FromArgb(returnColor.A, returnColor.R, colorValue, returnColor.B);
                    else
                        throw exception;

                    // Blue
                    if (channels[2] == "x")
                        keepB = true;
                    else if (byte.TryParse(channels[2], out colorValue))
                        returnColor = Color.FromArgb(returnColor.A, returnColor.R, returnColor.G, colorValue);
                    else
                        throw exception;

                    if (channels.Length == 4)
                    {
                        // Alpha
                        if (channels[3] == "x")
                            keepA = true;
                        else if (byte.TryParse(channels[3], out colorValue))
                            returnColor = Color.FromArgb(colorValue, returnColor.R, returnColor.G, returnColor.B);
                        else
                            throw exception;
                    }
                    else
                        returnColor = Color.FromArgb(255, returnColor.R, returnColor.G, returnColor.B);

                    return returnColor;
                }
                else
                    throw exception;
            }
            else if (value == "default")
            {
                useDefault = true;
                return returnColor;
            }
            else
            {
                value = value.ToLower();

                if (ColorMappings.ContainsKey(value))
                    return ColorMappings[value];
                else
                {
                    // Lookup color in framework
                    Type colorType = typeof(ColorHelper);

                    global::System.Reflection.PropertyInfo[] propInfoList =
                        colorType.GetProperties(global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.DeclaredOnly | global::System.Reflection.BindingFlags.Public);


                    if (propInfoList.Length != 0)
                    {
                        for (int i = 0; i < propInfoList.Length; i++)
                        {
                            if (propInfoList[i].Name.ToLower() == value)
                            {
                                return (Color)propInfoList[i].GetValue(null, null);
                            }
                        }
                    }
                    else
                    {
                        var fieldInfoList =
                        colorType.GetFields(global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.DeclaredOnly | global::System.Reflection.BindingFlags.Public);

                        for (int i = 0; i < fieldInfoList.Length; i++)
                        {
                            if (fieldInfoList[i].Name.ToLower() == value)
                            {
                                return (Color)fieldInfoList[i].GetValue(null);
                            }
                        }
                    }

                    throw exception;
                }
            }
        }
    }
    public static class ColorHelper
    {
        /// <summary>
        /// Performs linear interpolation of <see cref="Color"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Color"/>.</param>
        /// <param name="value2">Destination <see cref="Color"/>.</param>
        /// <param name="amount">Interpolation factor.</param>
        /// <returns>Interpolated <see cref="Color"/>.</returns>
        public static Color Lerp(Color value1, Color value2, float amount)
        {
            amount = SadConsole.MathHelper.Clamp(amount, 0, 1);
            var col = Color.FromArgb(
                (byte)SadConsole.MathHelper.Lerp(value1.A, value2.A, amount),
                (byte)SadConsole.MathHelper.Lerp(value1.R, value2.R, amount),
                (byte)SadConsole.MathHelper.Lerp(value1.G, value2.G, amount),
                (byte)SadConsole.MathHelper.Lerp(value1.B, value2.B, amount)
                );

            return col;
        }
        
    }

}
