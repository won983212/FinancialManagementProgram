using System.IO;

namespace FinancialManagementProgram
{
    public interface IPropertiesSerializable
    {
        void Deserialize(BinaryReader reader);

        void Serialize(BinaryWriter writer);
    }
}
