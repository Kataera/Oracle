using System;

namespace Oracle.Enumerations
{
    [Serializable]
    internal enum FateSelectMode
    {
        Closest,

        TypePriority,

        ChainPriority,

        TypeAndChainPriority
    }
}