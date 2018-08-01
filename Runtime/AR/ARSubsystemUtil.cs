using System;
using System.Collections.Generic;
using UnityEngine.Experimental;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// A collection of utilities for working with the AR-related Subsystems.
    /// </summary>
    /// <remarks>
    /// You would not normally use this directly.
    /// The <see cref="ARSubsystemManager"/> manages the individual subsystems.
    /// </remarks>
    public static class ARSubsystemUtil
    {
        /// <summary>
        /// Creates a <c>XRSessionSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRSessionSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRSessionSubsystem CreateSessionSubsystem(string id = null)
        {
            return CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRCameraSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRCameraSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRCameraSubsystem CreateCameraSubsystem(string id = null)
        {
            return CreateSubsystem<XRCameraSubsystemDescriptor, XRCameraSubsystem>(s_CameraSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRInputSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRInputSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRInputSubsystem CreateInputSubsystem(string id = null)
        {
            return CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRDepthSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRDepthSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRDepthSubsystem CreateDepthSubsystem(string id = null)
        {
            return CreateSubsystem<XRDepthSubsystemDescriptor, XRDepthSubsystem>(s_DepthSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRPlaneSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRPlaneSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRPlaneSubsystem CreatePlaneSubsystem(string id = null)
        {
            return CreateSubsystem<XRPlaneSubsystemDescriptor, XRPlaneSubsystem>(s_PlaneSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRReferencePointSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRReferencePointSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRReferencePointSubsystem CreateReferencePointSubsystem(string id = null)
        {
            return CreateSubsystem<XRReferencePointSubsystemDescriptor, XRReferencePointSubsystem>(s_ReferencePointSubsystemDescriptors, id);
        }

        /// <summary>
        /// Creates a <c>XRRaycastSubsystem</c>.
        /// </summary>
        /// <param name="id">(Optional) The name of the subsystem to create.</param>
        /// <returns>A <c>XRRaycastSubsystem</c> if successful, <c>null</c> otherwise.</returns>
        public static XRRaycastSubsystem CreateRaycastSubsystem(string id = null)
        {
            return CreateSubsystem<XRRaycastSubsystemDescriptor, XRRaycastSubsystem>(s_RaycastSubsystemDescriptors, id);
        }

        /// <summary>
        /// Create a <c>Subsystem</c> of a specified type.
        /// </summary>
        /// <typeparam name="TDescriptor">The type of <c>SubsystemDescriptor</c> which describes the <c>Subsystem</c>.</typeparam>
        /// <typeparam name="TSubsystem">The type of <c>Subsystem</c> to create.</typeparam>
        /// <param name="descriptors">A resuable list of <c>SubsystemDescriptors</c>. Used to avoid repeated heap allocations.</param>
        /// <param name="id">(Optional) Looks for a <c>SubystemDescriptor</c> with a particular name.
        /// If not specified, the first <c>SubsystemDescriptor</c> is used.</param>
        /// <returns>If a matching <c>SubsystemDescriptor</c> is found, it creates a <c>Subsystem</c> and returns it. Otherwise, null.</returns>
#if UNITY_2018_3_OR_NEWER
        static TSubsystem CreateSubsystem<TDescriptor, TSubsystem>(List<TDescriptor> descriptors, string id = null)
            where TDescriptor : IntegratedSubsystemDescriptor<TSubsystem>
            where TSubsystem : IntegratedSubsystem<TDescriptor>
#else
        static TSubsystem CreateSubsystem<TDescriptor, TSubsystem>(List<TDescriptor> descriptors, string id = null)
            where TDescriptor : SubsystemDescriptor<TSubsystem>
            where TSubsystem : Subsystem<TDescriptor>
#endif
        {
            if (descriptors == null)
                throw new ArgumentNullException("descriptors");

            SubsystemManager.GetSubsystemDescriptors<TDescriptor>(descriptors);

            if (descriptors.Count > 0)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    foreach (var descriptor in descriptors)
                    {
                        if(descriptor.id.IndexOf(id, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return descriptor.Create();
                        }
                    }
                }
                else
                {
                    var descriptorToUse = descriptors[0];
                    if (descriptors.Count > 1)
                    {
                        Type typeOfD = typeof(TDescriptor);
                        Debug.LogWarningFormat("Found {0} {1}s. Using \"{2}\"",
                            descriptors.Count, typeOfD.Name, descriptorToUse.id);
                    }

                    return descriptorToUse.Create();
                }
            }

            return null;
        }

        static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();

        static List<XRCameraSubsystemDescriptor> s_CameraSubsystemDescriptors = new List<XRCameraSubsystemDescriptor>();

        static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();

        static List<XRDepthSubsystemDescriptor> s_DepthSubsystemDescriptors = new List<XRDepthSubsystemDescriptor>();

        static List<XRPlaneSubsystemDescriptor> s_PlaneSubsystemDescriptors = new List<XRPlaneSubsystemDescriptor>();

        static List<XRReferencePointSubsystemDescriptor> s_ReferencePointSubsystemDescriptors = new List<XRReferencePointSubsystemDescriptor>();

        static List<XRRaycastSubsystemDescriptor> s_RaycastSubsystemDescriptors = new List<XRRaycastSubsystemDescriptor>();
    }
}
