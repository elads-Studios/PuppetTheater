using UnityEngine;
using UnityPatterns.Controllers;

/// <summary>
/// Puppet Theater controllers namespace
/// </summary>
namespace PuppetTheater.Controllers
{
    /// <summary>
    /// An abstract class that describes a puppet theater entity controller script
    /// </summary>
    public abstract class APuppetTheaterEntityControllerScript : AControllersControllerScript<APuppetTheaterEntityControllerScript>, IPuppetTheaterEntityController
    {
        /// <summary>
        /// Relative to root entity original position
        /// </summary>
        private Vector3 relativeToRootEntityOriginalPosition;

        /// <summary>
        /// Relative to root entity original position
        /// </summary>
        public Vector3 RelativeToRootEntityOriginalPosition
        {
            get => relativeToRootEntityOriginalPosition;
            set
            {
                if (relativeToRootEntityOriginalPosition != value)
                {
                    relativeToRootEntityOriginalPosition = value;
                    UpdatePosition();
                }
            }
        }

        /// <summary>
        /// Original position
        /// </summary>
        public Vector3 OriginalPosition { get; private set; }

        /// <summary>
        /// Gets the perspection corrected position
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Perspectiom corrected position</returns>
        private Vector3 GetPerspectionCorrectedPosition(Vector3 position)
        {
            Vector3 camera_position = PuppetTheaterCameraManagerScript.Instance.transform.position;
            Vector3 looking_vector = position - camera_position;
            Vector2 x_planar_looking_vector = new Vector2(looking_vector.x, looking_vector.z);
            Vector2 y_planar_looking_vector = new Vector2(looking_vector.y, looking_vector.z);
            float x_angle = Vector2.SignedAngle(Vector2.right, x_planar_looking_vector.normalized) * Mathf.Deg2Rad;
            float y_angle = Vector2.SignedAngle(Vector2.right, y_planar_looking_vector.normalized) * Mathf.Deg2Rad;
            float correction_multiplier = Mathf.PI * PuppetTheaterCameraManagerScript.Instance.Camera.orthographicSize * 0.5f / (PuppetTheaterCameraManagerScript.Instance.FieldOfView * Mathf.Deg2Rad);
            float half_pi = Mathf.PI * 0.5f;
            return new Vector3
            (
                camera_position.x + ((x_planar_looking_vector.sqrMagnitude > float.Epsilon) ? (Mathf.Cos(x_angle) * (correction_multiplier / Mathf.Cos(x_angle - half_pi))) : 0.0f),
                camera_position.y + ((y_planar_looking_vector.sqrMagnitude > float.Epsilon) ? (Mathf.Cos(y_angle) * (correction_multiplier / Mathf.Cos(y_angle - half_pi))) : 0.0f),
                position.z
            );
        }

        /// <summary>
        /// Updates position
        /// </summary>
        private void UpdatePosition()
        {
            if (relativeToRootEntityOriginalPosition.sqrMagnitude > float.Epsilon)
            {
                PuppetTheaterCameraManagerScript puppet_theater_camera_manager = PuppetTheaterCameraManagerScript.Instance;
                if (puppet_theater_camera_manager && puppet_theater_camera_manager.RootPuppetTheaterEntityController && puppet_theater_camera_manager.RootPuppetTheaterEntityController.isActiveAndEnabled && puppet_theater_camera_manager.Camera)
                {
                    if (puppet_theater_camera_manager.RootPuppetTheaterEntityController == this)
                    {
                        Vector3 other_puppet_theater_entities_movement = new Vector3
                        (
                            puppet_theater_camera_manager.IsEntityXPositionLocked ? -relativeToRootEntityOriginalPosition.x : 0.0f,
                            puppet_theater_camera_manager.IsEntityYPositionLocked ? -relativeToRootEntityOriginalPosition.y : 0.0f,
                            puppet_theater_camera_manager.IsEntityZPositionLocked ? -relativeToRootEntityOriginalPosition.z : 0.0f
                        );
                        relativeToRootEntityOriginalPosition = new Vector3
                        (
                            puppet_theater_camera_manager.IsEntityXPositionLocked ? 0.0f : relativeToRootEntityOriginalPosition.x,
                            puppet_theater_camera_manager.IsEntityYPositionLocked ? 0.0f : relativeToRootEntityOriginalPosition.y,
                            puppet_theater_camera_manager.IsEntityZPositionLocked ? 0.0f : relativeToRootEntityOriginalPosition.z
                        );
                        PerformPositionUpdate(GetPerspectionCorrectedPosition(OriginalPosition + relativeToRootEntityOriginalPosition));
                        if (other_puppet_theater_entities_movement != Vector3.zero)
                        {
                            foreach (APuppetTheaterEntityControllerScript puppet_theater_entity_controller in EnabledControllers)
                            {
                                if (puppet_theater_entity_controller != this)
                                {
                                    puppet_theater_entity_controller.Move(other_puppet_theater_entities_movement);
                                }
                            }
                        }
                    }
                    else
                    {
                        PerformPositionUpdate(GetPerspectionCorrectedPosition(puppet_theater_camera_manager.RootPuppetTheaterEntityController.OriginalPosition + relativeToRootEntityOriginalPosition));
                    }
                }
            }
        }

        /// <summary>
        /// Performs a position update
        /// </summary>
        /// <param name="position">Position</param>
        protected abstract void PerformPositionUpdate(Vector3 position);

        /// <summary>
        /// Moves this puppet entity
        /// </summary>
        /// <param name="movement">Movement vector</param>
        public virtual void Move(Vector3 movement) => RelativeToRootEntityOriginalPosition += movement;

        /// <summary>
        /// Gets invoked when script has been enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            PuppetTheaterCameraManagerScript puppet_theater_camera_manager = PuppetTheaterCameraManagerScript.Instance;
            OriginalPosition = transform.position;
            if (puppet_theater_camera_manager && puppet_theater_camera_manager.RootPuppetTheaterEntityController && puppet_theater_camera_manager.RootPuppetTheaterEntityController.isActiveAndEnabled)
            {
                if (puppet_theater_camera_manager.RootPuppetTheaterEntityController == this)
                {
                    foreach (APuppetTheaterEntityControllerScript puppet_theater_entity_controller in EnabledControllers)
                    {
                        if (puppet_theater_entity_controller != this)
                        {
                            puppet_theater_entity_controller.RelativeToRootEntityOriginalPosition -= OriginalPosition;
                        }
                    }
                }
                else
                {
                    relativeToRootEntityOriginalPosition = OriginalPosition - puppet_theater_camera_manager.RootPuppetTheaterEntityController.OriginalPosition;
                }
            }
            else
            {
                relativeToRootEntityOriginalPosition = OriginalPosition;
            }
            UpdatePosition();
        }

        /// <summary>
        /// Gets invoked when script performs a physics update
        /// </summary>
        protected virtual void FixedUpdate() => UpdatePosition();
    }
}
