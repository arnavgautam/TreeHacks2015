namespace FabrikamInsurance.Models
{
    using System.Collections.Generic;

    public interface IAutomobileDataRepository
    {
        IEnumerable<KeyValuePair<string, string>> GetMakes();

        IEnumerable<KeyValuePair<string, string>> GetModels(string makeId);

        IEnumerable<Factor> GetBodyStyles();

        IEnumerable<Factor> GetBrakeTypes();

        IEnumerable<Factor> GetSafetyEquipment();

        IEnumerable<Factor> GetAntiTheftDevices();

        decimal GetBookValue(string makeId, string modelId);
    }
}
