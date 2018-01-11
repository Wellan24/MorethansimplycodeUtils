// Copyright © 2010-2015 Bertrand Le Roy.  All Rights Reserved.
// This code released under the terms of the 
// MIT License http://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Cartif.IO
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A path. This class cannot be inherited. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public sealed class Path : PathBase<Path>
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates an empty Path object. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Path()
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of path strings. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="paths"> The list of path strings. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(params string[] paths)
            : base(paths)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of path strings. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="paths"> The list of path strings. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(params Path[] paths)
            : base(paths.ToList())
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of path strings. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="paths"> The list of path strings. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(IEnumerable<string> paths)
            : base(paths)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of paths. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="paths"> The list of paths. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(IEnumerable<Path> paths)
            : base(paths)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of path strings and a previous list of path
        ///           strings. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="path">          A path string. </param>
        /// <param name="previousPaths"> The list of path strings in the previous set. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(string path, Path previousPaths)
            : base(path, previousPaths)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates a collection of paths from a list of path strings and a previous list of path
        ///           strings. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="paths">         The list of path strings in the set. </param>
        /// <param name="previousPaths"> The list of path strings in the previous set. </param>
        ///--------------------------------------------------------------------------------------------------
        public Path(IEnumerable<string> paths, Path previousPaths)
            : base(paths, previousPaths)
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Explicit cast that converts the given Path to a string. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="path"> A path string. </param>
        /// <returns> The result of the operation. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static explicit operator string(Path path)
        {
            return path.FirstPath();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Explicit cast that converts the given string to a Path. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="path"> A path string. </param>
        /// <returns> The result of the operation. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static explicit operator Path(string path)
        {
            return new Path(path);
        }
    }
}
