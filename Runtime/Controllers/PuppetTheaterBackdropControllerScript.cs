using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Puppet Theater controllers namespace
/// </summary>
namespace PuppetTheater.Controllers
{
    /// <summary>
    /// A class that describes a puppet theater backdrop controller script
    /// </summary>
    public class PuppetTheaterBackdropControllerScript : APuppetTheaterEntityControllerScript, IPuppetTheaterBackdropController
    {
        /// <summary>
        /// Backdrop size
        /// </summary>
        [SerializeField]
        private Vector3 backdropSize;

        /// <summary>
        /// Backdrop transforms
        /// </summary>
        [SerializeField]
        private Transform[] backdropTransforms = Array.Empty<Transform>();

        /// <summary>
        /// Original backdrop positions
        /// </summary>
        private Vector3[] originalBackdropPositions = Array.Empty<Vector3>();

        /// <summary>
        /// Backdrops center position
        /// </summary>
        private Vector3 backdropsCenterPosition;

        /// <summary>
        /// Backdrop size
        /// </summary>
        public Vector3 BackdropSize
        {
            get => backdropSize;
            set
            {
                Vector3 minimal_backdrop_size = MinimalBackdropSize;
                backdropSize = new Vector3
                (
                    Mathf.Max(value.x, minimal_backdrop_size.x),
                    Mathf.Max(value.y, minimal_backdrop_size.y),
                    Mathf.Max(value.z, minimal_backdrop_size.z)
                );
            }
        }

        /// <summary>
        /// Backdrop transforms
        /// </summary>
        public Transform[] BackdropTransforms
        {
            get => backdropTransforms ??= Array.Empty<Transform>();
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                backdropTransforms = (Transform[])value.Clone();
            }
        }

        /// <summary>
        /// Minimal backdrop size
        /// </summary>
        public Vector3 MinimalBackdropSize
        {
            get
            {
                Vector3 ret = Vector3.zero;
                InitializeOriginalPositions();
                foreach (Transform backdrop_transform in backdropTransforms)
                {
                    if (backdrop_transform)
                    {
                        Vector3 backdrop_position = backdrop_transform.position;
                        foreach (Transform other_backdrop_transform in backdropTransforms)
                        {
                            if (other_backdrop_transform && (backdrop_transform != other_backdrop_transform))
                            {
                                Vector3 other_backdrop_position = other_backdrop_transform.position;
                                ret = new Vector3
                                (
                                    Mathf.Max(Mathf.Abs(other_backdrop_position.x - backdrop_position.x), ret.x),
                                    Mathf.Max(Mathf.Abs(other_backdrop_position.y - backdrop_position.y), ret.y),
                                    Mathf.Max(Mathf.Abs(other_backdrop_position.z - backdrop_position.z), ret.z)
                                );
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Original backdrop positions
        /// </summary>
        public IReadOnlyList<Vector3> OriginalBackdropPositions
        {
            get
            {
                InitializeOriginalPositions();
                return originalBackdropPositions;
            }
        }

        /// <summary>
        /// Backdrops center position
        /// </summary>
        public Vector3 BackdropsCenterPosition
        {
            get
            {
                InitializeOriginalPositions();
                return backdropsCenterPosition;
            }
        }

        /// <summary>
        /// Initializes original positions
        /// </summary>
        private void InitializeOriginalPositions()
        {
            if
            (
#if UNITY_EDITOR
                !EditorApplication.isPlaying ||
#endif
                (BackdropTransforms.Length != originalBackdropPositions.Length)
            )
            {
                uint count = 0U;
                originalBackdropPositions = new Vector3[backdropTransforms.Length];
                backdropsCenterPosition = Vector3.zero;
                for (int backdrop_index = 0; backdrop_index < originalBackdropPositions.Length; backdrop_index++)
                {
                    Transform backdrop_transform = backdropTransforms[backdrop_index];
                    if (backdrop_transform)
                    {
                        ref Vector3 original_backdrop_positions = ref originalBackdropPositions[backdrop_index];
                        original_backdrop_positions = backdrop_transform.position;
                        backdropsCenterPosition += original_backdrop_positions;
                        ++count;
                    }
                }
                if (count > 0U)
                {
                    backdropsCenterPosition /= count;
                }
            }
        }

        /// <summary>
        /// Performs a position update
        /// </summary>
        /// <param name="position">Position</param>
        protected override void PerformPositionUpdate(Vector3 position)
        {
            InitializeOriginalPositions();
            Vector3 backdrop_size = BackdropSize;
            Vector3 half_backdrop_size = backdrop_size * 0.5f;
            Vector3 top_left_behind = backdropsCenterPosition - half_backdrop_size;
            Vector3 relative_position = position - OriginalPosition;
            for (int backdrop_index = 0; backdrop_index < backdropTransforms.Length; backdrop_index++)
            {
                Transform backdrop_transform = backdropTransforms[backdrop_index];
                if (backdrop_transform)
                {
                    Vector3 original_backdrop_position = originalBackdropPositions[backdrop_index];
                    backdrop_transform.position = new Vector3
                    (
                        Mathf.Repeat(original_backdrop_position.x + relative_position.x - top_left_behind.x, (backdrop_size.x > float.Epsilon) ? backdrop_size.x : float.Epsilon) + top_left_behind.x,
                        Mathf.Repeat(original_backdrop_position.y + relative_position.y - top_left_behind.y, (backdrop_size.y > float.Epsilon) ? backdrop_size.y : float.Epsilon) + top_left_behind.y,
                        Mathf.Repeat(original_backdrop_position.z + relative_position.z - top_left_behind.z, (backdrop_size.z > float.Epsilon) ? backdrop_size.z : float.Epsilon) + top_left_behind.z
                    );
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Gets invoked when gizmos need to be drawn and this game object has been selected
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Color color = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(BackdropsCenterPosition, backdropSize);
            if (backdropTransforms != null)
            {
                Gizmos.color = Color.red;
                foreach (Transform backdrop_transform in backdropTransforms)
                {
                    if (backdrop_transform)
                    {
                        Gizmos.DrawSphere(backdrop_transform.position, 0.125f);
                    }
                }
            }
            Gizmos.color = color;
        }

        /// <summary>
        /// Gets invoked when script needs to be validated
        /// </summary>
        private void OnValidate()
        {
            Vector3 minimal_backdrop_size = MinimalBackdropSize;
            backdropSize = new Vector3
            (
                Mathf.Max(backdropSize.x, minimal_backdrop_size.x),
                Mathf.Max(backdropSize.y, minimal_backdrop_size.y),
                Mathf.Max(backdropSize.z, minimal_backdrop_size.z)
            );
        }
#endif
    }
}
