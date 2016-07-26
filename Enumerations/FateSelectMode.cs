using System;

namespace Oracle.Enumerations
{
    [Serializable]
    internal enum FateSelectMode
    {
        Closest = 0,

        TypePriority = 1,

        ChainPriority = 2,

        TypeAndChainPriority = TypePriority + ChainPriority
    }
}