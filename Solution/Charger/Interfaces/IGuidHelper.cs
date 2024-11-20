using System;

namespace Charger.Interfaces
{
    public interface IGuidHelper
    {
        Guid CreateGuid();
        byte[] CreateGuidAsBytes();
        ushort CreateGuidAsUShort();
    }
}