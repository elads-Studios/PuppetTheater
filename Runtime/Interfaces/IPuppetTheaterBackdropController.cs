using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Puppet Theater namespace
/// </summary>
namespace PuppetTheater
{
    /// <summary>
    /// An interface that represents a puppet theater backdrop controller
    /// </summary>
    public interface IPuppetTheaterBackdropController : IPuppetTheaterEntityController
    {
        /// <summary>
        /// Backdrop size
        /// </summary>
        Vector3 BackdropSize { get; set; }

        /// <summary>
        /// Backdrop transforms
        /// </summary>
        Transform[] BackdropTransforms { get; set; }

        /// <summary>
        /// Minimal backdrop size
        /// </summary>
        Vector3 MinimalBackdropSize { get; }

        /// <summary>
        /// Original backdrop positions
        /// </summary>
        IReadOnlyList<Vector3> OriginalBackdropPositions { get; }

        /// <summary>
        /// Backdrops center position
        /// </summary>
        Vector3 BackdropsCenterPosition { get; }
    }
}
