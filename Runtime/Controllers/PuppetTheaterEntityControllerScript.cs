using UnityEngine;

/// <summary>
/// Puppet Theater controllers namespace
/// </summary>
namespace PuppetTheater.Controllers
{
    /// <summary>
    /// A class that describes a puppet theater entity controller script
    /// </summary>
    public class PuppetTheaterEntityControllerScript : APuppetTheaterEntityControllerScript
    {
        /// <summary>
        /// Performs a position update
        /// </summary>
        /// <param name="position">Position</param>
        protected override void PerformPositionUpdate(Vector3 position) => transform.position = position;
    }
}
