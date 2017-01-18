#if SFML
using FrameworkColor = SFML.Graphics.Color;
using FrameworkPoint = SFML.System.Vector2i;
using FrameworkRect = SFML.Graphics.IntRect;
#elif MONOGAME
using FrameworkColor = Microsoft.Xna.Framework.Color;
using FrameworkPoint = Microsoft.Xna.Framework.Point;
using FrameworkRect = Microsoft.Xna.Framework.Rectangle;
using FrameworkSpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects;
#endif


namespace SadConsole
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Common serialization tasks for SadConsole.
    /// </summary>
    public static partial class Serializer
    {
        /// <summary>
        /// Serializes the <paramref name="instance"/> instance to the specified file.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="instance">The object to serialize.</param>
        /// <param name="file">The file to save the object to.</param>
        /// <param name="knownTypes">Optional list of known types for serialization.</param>
        public static void Save<T>(T instance, string file, IEnumerable<Type> knownTypes = null)
        {
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), knownTypes);

            using (var stream = System.IO.File.OpenWrite(file))
                serializer.WriteObject(stream, instance);
        }

        /// <summary>
        /// Deserializes a new instance of <typeparamref name="T"/> from the specified file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="file">The file to load from.</param>
        /// <param name="knownTypes">Known types used during deserialization.</param>
        /// <returns>A new object instance.</returns>
        public static T Load<T>(string file, IEnumerable<Type> knownTypes = null)
        {
            if (System.IO.File.Exists(file))
            {
                using (var fileObject = System.IO.File.OpenRead(file))
                {
                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), knownTypes);

                    return (T)serializer.ReadObject(fileObject);
                }
            }

            throw new System.IO.FileNotFoundException("File not found.", file);
        }
    }
    
}