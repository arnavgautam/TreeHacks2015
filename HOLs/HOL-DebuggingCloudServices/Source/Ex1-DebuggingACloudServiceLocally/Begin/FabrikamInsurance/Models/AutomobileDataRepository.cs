namespace FabrikamInsurance.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;

    public class AutomobileDataRepository 
        : IAutomobileDataRepository
    {
        private static XElement automobileData;

        static AutomobileDataRepository()
        {
            AutomobileDataRepository.automobileData = 
                XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/AutomobileData.xml"));
        }

        public IEnumerable<KeyValuePair<string, string>> GetMakes()
        {
            return from make in AutomobileDataRepository.automobileData.Element("automobiles").Elements("make")
                   orderby make.Attribute("name").Value
                   select new KeyValuePair<string, string>(make.Attribute("id").Value, make.Attribute("name").Value);
        }

        public IEnumerable<KeyValuePair<string, string>> GetModels(string makeId)
        {
            return from make in AutomobileDataRepository.automobileData.Element("automobiles").Elements("make")
                   from model in make.Elements("model")
                   where make.Attribute("id").Value == makeId
                   orderby model.Value
                   select new KeyValuePair<string, string>(model.Attribute("id").Value, model.Value);
        }

        public IEnumerable<Factor> GetBodyStyles()
        {
            return this.GetOptionList("bodystyles");
        }

        public IEnumerable<Factor> GetBrakeTypes()
        {
            return this.GetOptionList("braketypes");
        }

        public IEnumerable<Factor> GetSafetyEquipment()
        {
            return this.GetOptionList("safetyequipment");
        }

        public IEnumerable<Factor> GetAntiTheftDevices()
        {
            return this.GetOptionList("antitheftdevices");
        }
        
        public decimal GetBookValue(string makeId, string modelId)
        {
            var bookValue = (from make in AutomobileDataRepository.automobileData.Element("automobiles").Elements("make")
                            from model in make.Elements("model")
                            where make.Attribute("id").Value == makeId && model.Attribute("id").Value == modelId
                            select model.Attribute("bookValue").Value).FirstOrDefault();

            return bookValue != null ? Convert.ToDecimal(bookValue) : -1;
        }

        private IEnumerable<Factor> GetOptionList(string name)
        {
            return from item in AutomobileDataRepository.automobileData.Element(name).Elements("option")
                   orderby item.Attribute("id").Value
                   select new Factor 
                   {
                       Id = item.Attribute("id").Value,
                       Name = item.Value,
                       Value = Convert.ToDecimal(item.Attribute("factor").Value)
                   };
        }
    }
}