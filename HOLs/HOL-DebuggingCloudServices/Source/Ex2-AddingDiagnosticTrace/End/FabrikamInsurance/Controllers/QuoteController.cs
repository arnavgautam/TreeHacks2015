namespace FabrikamInsurance.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using FabrikamInsurance.Models;
    using InsurancePolicy;

    [HandleError]
    public class QuoteController : Controller
    {
        private IAutomobileDataRepository repository;

        public QuoteController()
            : this(new AutomobileDataRepository())
        {
        }

        public QuoteController(IAutomobileDataRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult About()
        {
            System.Diagnostics.Trace.TraceInformation("About called...");
            return this.View();
        }

        public ActionResult Calculator()
        {
            System.Diagnostics.Trace.TraceInformation("Calculator called...");
            QuoteViewModel model = new QuoteViewModel();
            this.PopulateViewModel(model, null);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Calculator(QuoteViewModel model)
        {
            this.PopulateViewModel(model, model.MakeId);

            if (ModelState.IsValid)
            {
                decimal bookValue = this.repository.GetBookValue(model.MakeId, model.ModelId);
                decimal bodyStyleFactor = model.BodyStyles.Where(item => item.Id == model.BodyStyleId).FirstOrDefault().Value;
                decimal brakeTypeFactor = model.BrakeTypes.Where(item => item.Id == model.BrakeTypeId).FirstOrDefault().Value;
                decimal safetyEquipmentFactor = model.SafetyEquipment.Where(item => item.Id == model.SafetyEquipmentId).FirstOrDefault().Value;
                decimal antiTheftDeviceFactor = model.AntiTheftDevices.Where(item => item.Id == model.AntiTheftDeviceId).FirstOrDefault().Value;
                decimal premium = AutoInsurance.CalculatePremium(bookValue, model.ManufacturedYear, bodyStyleFactor, brakeTypeFactor, safetyEquipmentFactor, antiTheftDeviceFactor);
                model.MonthlyPremium = premium / 12;
                model.YearlyPremium = premium;
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult GetModels(string id)
        {
            return this.Json(this.repository.GetModels(id));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            System.Diagnostics.Trace.TraceError(filterContext.Exception.Message);
        }

        private void PopulateViewModel(QuoteViewModel model, string makeId)
        {
            model.Makes = this.repository.GetMakes();
            model.Models = this.repository.GetModels(makeId);
            model.BodyStyles = this.repository.GetBodyStyles();
            model.BrakeTypes = this.repository.GetBrakeTypes();
            model.SafetyEquipment = this.repository.GetSafetyEquipment();
            model.AntiTheftDevices = this.repository.GetAntiTheftDevices();
            model.YearList = Enumerable.Range(DateTime.Today.Year - AutoInsurance.MaximumVehicleAge + 1, AutoInsurance.MaximumVehicleAge);
        }
    }
}