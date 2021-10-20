using System;
using UnityEngine;
using UnityPatterns.Managers;

/// <summary>
/// Puppet Theater managers namespace
/// </summary>
namespace PuppetTheater.Controllers
{
    /// <summary>
    /// A class that describes a puppet theater camera manager script
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PuppetTheaterCameraManagerScript : AManagerScript<PuppetTheaterCameraManagerScript>, IPuppetTheaterCameraManager
    {
        /// <summary>
        /// Is entity X position locked
        /// </summary>
        [SerializeField]
        private bool isEntityXPositionLocked;

        /// <summary>
        /// Is entity Y position locked
        /// </summary>
        [SerializeField]
        private bool isEntityYPositionLocked;

        /// <summary>
        /// Is entity Z position locked
        /// </summary>
        [SerializeField]
        private bool isEntityZPositionLocked;

        /// <summary>
        /// Field of view
        /// </summary>
        [SerializeField]
        [Range(0.0f, 180.0f)]
        private float fieldOfView = 60.0f;

        /// <summary>
        /// Root puppet theater entity controller
        /// </summary>
        [SerializeField]
        private PuppetTheaterEntityControllerScript rootPuppetTheaterEntityController = default;

        /// <summary>
        /// Is entity X position locked
        /// </summary>
        public bool IsEntityXPositionLocked
        {
            get => isEntityXPositionLocked;
            set => isEntityXPositionLocked = value;
        }

        /// <summary>
        /// Is entity Y position locked
        /// </summary>
        public bool IsEntityYPositionLocked
        {
            get => isEntityYPositionLocked;
            set => isEntityYPositionLocked = value;
        }

        /// <summary>
        /// Is entity Z position locked
        /// </summary>
        public bool IsEntityZPositionLocked
        {
            get => isEntityZPositionLocked;
            set => isEntityZPositionLocked = value;
        }

        /// <summary>
        /// Field of view
        /// </summary>
        public float FieldOfView
        {
            get => fieldOfView;
            set => fieldOfView = Mathf.Clamp(value, 0.0f, 180.0f);
        }

        /// <summary>
        /// Root puppet theater entity controller
        /// </summary>
        public PuppetTheaterEntityControllerScript RootPuppetTheaterEntityController
        {
            get => rootPuppetTheaterEntityController;
            set
            {
                if (!value)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                rootPuppetTheaterEntityController = value;
            }
        }

        /// <summary>
        /// Camera
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Gets invoked when script has been started
        /// </summary>
        private void Start()
        {
            if (!rootPuppetTheaterEntityController)
            {
                Debug.LogError("Please assign a root puppet theater entity controller to this component.", this);
            }
            if (TryGetComponent(out Camera camera))
            {
                Camera = camera;
            }
            else
            {
                Debug.LogError("Please attach a camera component to this game object.", this);
            }
        }
    }
}
