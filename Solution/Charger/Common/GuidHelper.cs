using Charger.Interfaces;
using System;

namespace Charger.Common
{
    public class GuidHelper : IGuidHelper
    {
        public Guid CreateGuid()
        {
            return Guid.NewGuid();
        }

        public byte[] CreateGuidAsBytes()
        {
            Guid guid = Guid.NewGuid();
            byte[] buffer = guid.ToByteArray();

            return buffer;
        }
        public ushort CreateGuidAsUShort()
        {
            Guid guid = Guid.NewGuid();
            byte[] buffer = guid.ToByteArray();
            Array.Reverse(buffer, 0, buffer.Length);

            return BitConverter.ToUInt16(buffer, 0);
        }
    }
}
