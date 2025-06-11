#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniversalEntities.Logging
{
    internal static class UniversalEntitiesLogger
    {
        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining), HideInCallstack]
        internal static void Log(string message)
        {
            Debug.Log($"[UNIVERSAL_ENTITIES] {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining), HideInCallstack]
        internal static void LogWarning(string message)
        {
            Debug.LogWarning($"[UNIVERSAL_ENTITIES] {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining), HideInCallstack]
        internal static void LogError(string message)
        {
            Debug.LogError($"[UNIVERSAL_ENTITIES] {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining), HideInCallstack]
        internal static void LogException(Exception e)
        {
            Debug.LogException(e);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining), HideInCallstack]
        internal static void DebugLog(string message)
        {
            Debug.Log($"[UNIVERSAL_ENTITIES] {message}");
        }
    };
}

#endif