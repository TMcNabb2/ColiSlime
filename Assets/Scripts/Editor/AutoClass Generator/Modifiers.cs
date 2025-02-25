using System;
using System.Linq;
using UnityEngine;

namespace AutoClass
{
    public enum Access
    {
        Private, Protected, Internal, Public
    }

    [Flags]
    public enum Modifiers
    {
        None = 0,
        Static = 1,
        Readonly = 2,
        Abstract = 4,
        Virtual = 8,
        Override = 16,
        Const = 32,
        Partial = 64,
        Event = 128,
    }

    public enum ClassType
    {
        Class, Interface, Struct
    }

    [Flags]
    public enum PropertyAccessors
    {
        None = 0,
        Get = 1,
        Set = 2,
        Init = 4
    }

    public static class ModifierExtensions
    {
        public static string AsKeyword(this Access access, bool addSpace = true) => access switch
        {
            _ => $"{access}{(addSpace ? " " : string.Empty)}".ToLower()
        };

        public static string AsKeyword(this ClassType type, bool addSpace = true) => type switch
        {
            _ => $"{type}{(addSpace ? " " : string.Empty)}".ToLower()
        };

        public static string AsKeyword(this Modifiers modifiers, bool addSpace = true) => modifiers switch
        {
            _ when modifiers is Modifiers.None => "",
            _ => Utils.Combine(" ", Utils.GetEnumValues<Modifiers>()
                    .Where(v => v != Modifiers.None && modifiers.HasFlag(v))
                    .Select(flag => $"{flag}".ToLower())
                    .ToArray()) + (addSpace ? " " : string.Empty)
        };
    }
}
