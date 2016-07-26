using System;

namespace Oracle.Enumerations
{
    [Serializable]
    internal enum FateSupportLevel
    {
        FullSupport,

        CustomSupport,

        Problematic,

        Unsupported,

        NotInGame
    }
}