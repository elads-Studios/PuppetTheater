using PuppetTheater.Controllers;
using UnityEngine;

/// <summary>
/// Puppet Theater namespace
/// </summary>
namespace PuppetTheater
{
    /// <summary>
    /// An interface that represents a puppet theater camera manager
    /// </summary>
    public interface IPuppetTheaterCameraManager
    {
        /// <summary>
        /// Is entity X position locked
        /// </summary>
        bool IsEntityXPositionLocked { get; set; }

        /// <summary>
        /// Is entity Y position locked
        /// </summary>
        bool IsEntityYPositionLocked { get; set; }

        /// <summary>
        /// Is entity Z position locked
        /// </summary>
        bool IsEntityZPositionLocked { get; set; }

        /// <summary>
        /// Field of view
        /// </summary>
        float FieldOfView { get; set; }

        /// <summary>
        /// Root puppet theater entity controller
        /// </summary>
        PuppetTheaterEntityControllerScript RootPuppetTheaterEntityController { get; set; }

        /// <summary>
        /// Camera
        /// </summary>
        Camera Camera { get; }
    }
}
