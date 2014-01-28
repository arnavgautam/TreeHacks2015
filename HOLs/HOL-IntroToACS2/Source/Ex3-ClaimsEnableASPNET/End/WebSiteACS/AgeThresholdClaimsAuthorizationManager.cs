namespace WebSiteACS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Xml;

    public class AgeThresholdClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        private static Dictionary<string, int> _policies = new Dictionary<string, int>();

        public override void LoadCustomConfiguration(XmlNodeList nodelist)
        {
            foreach (XmlNode node in nodelist)
            {
                XmlTextReader rdr = new XmlTextReader(new StringReader(node.OuterXml));
                rdr.MoveToContent();

                string resource = rdr.GetAttribute("resource");

                rdr.Read();

                string claimType = rdr.GetAttribute("claimType");

                if (claimType.CompareTo(ClaimTypes.DateOfBirth) != 0)
                    throw new NotSupportedException("Only birthdate claims are supported");

                string minAge = rdr.GetAttribute("minAge");

                _policies[resource] = int.Parse(minAge);
            }
        }

        public override bool CheckAccess(AuthorizationContext context)
        {
            Uri webPage = new Uri(context.Resource[0].Value);

            if (_policies.ContainsKey(webPage.PathAndQuery))
            {
                int minAge = _policies[webPage.PathAndQuery];
                string userBirthdate = context.Principal.FindFirst(ClaimTypes.DateOfBirth).Value;

                int userAge = DateTime.Now.Subtract(DateTime.Parse(userBirthdate)).Days / 365;

                if (userAge < minAge)
                {
                    return false;
                }
            }

            return true;
        }
    }
}