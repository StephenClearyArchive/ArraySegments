using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// All changes to this file should be pushed to the PortableClassLibraryFakeContracts solution.

namespace System.Diagnostics.Contracts
{
    internal static class Contract
    {
        public static T Result<T>() { return default(T); }
        public static T OldValue<T>(T value) { return value; }

        [Conditional("never")]
        public static void Ensures(bool condition) { }

        [Conditional("never")]
        public static void Requires(bool condition) { }

        [Conditional("never")]
        public static void Assume(bool condition) { }

        [Conditional("never")]
        public static void Assert(bool condition) { }

        [Conditional("never")]
        public static void Invariant(bool condition) { }
    }

    internal sealed class ContractInvariantMethodAttribute : Attribute
    {
    }
}
