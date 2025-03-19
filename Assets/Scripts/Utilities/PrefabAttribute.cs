using System;
using UnityEngine;

/// <summary>
/// Ensures objects assigned to this field are prefabs.
/// </summary>
/// <remarks>
/// Fields of a type other than <see cref="GameObject"/> are not supported.
/// </remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class PrefabAttribute : PropertyAttribute { }
