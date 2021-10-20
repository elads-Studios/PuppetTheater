using UnityEngine;
using UnityPatterns;

/// <summary>
/// Puppet Theater namespace
/// </summary>
namespace PuppetTheater
{
    /// <summary>
    /// An interface that represents a puppet theater entity controller
    /// </summary>
    public interface IPuppetTheaterEntityController : IController
    {
        /// <summary>
        /// Relative to root entity original position
        /// </summary>
        Vector3 RelativeToRootEntityOriginalPosition { get; set; }

        /// <summary>
        /// Original position
        /// </summary>
        Vector3 OriginalPosition { get; }

        /// <summary>
        /// Moves this puppet entity
        /// </summary>
        /// <param name="movement">Movement vector</param>
        void Move(Vector3 movement);
    }
}
