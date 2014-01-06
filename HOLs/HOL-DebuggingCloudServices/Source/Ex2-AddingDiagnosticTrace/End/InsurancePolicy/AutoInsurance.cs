namespace InsurancePolicy
{
    using System;

    public class AutoInsurance
    {
        public static readonly int MaximumVehicleAge = 10;

        // (THIS IS NOT A REAL FORMULA)
        public static decimal CalculatePremium(decimal bookValue, int manufacturedYear, decimal bodyStyleFactor, decimal brakeTypeFactor, decimal safetyEquipmentFactor, decimal antiTheftDeviceFactor)
        {
            var ageFactor = (manufacturedYear - DateTime.Today.Year + MaximumVehicleAge) * 2000 / bookValue;
            decimal coefficient = (bodyStyleFactor + brakeTypeFactor + safetyEquipmentFactor + antiTheftDeviceFactor + ageFactor) / 100;
            decimal premium = bookValue * coefficient;

            return premium;
        }
    }
}
