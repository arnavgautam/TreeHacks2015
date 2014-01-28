// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.SocialGames.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public class DictionaryHaveSameElementsComparison<T, K>
    {
        private IDictionary<T, K> first;

        private IDictionary<T, K> second;

        public DictionaryHaveSameElementsComparison(IDictionary<T, K> first, IDictionary<T, K> second)
        {
            this.first = first;
            this.second = second;
        }

        public bool DoIt()
        {
            if (this.first.Count == this.second.Count)
            {
                for (int i = 0; i < this.first.Count; i++)
                {
                    if (!this.first.ElementAt(i).Key.Equals(this.second.ElementAt(i).Key) || 
                        !this.first.ElementAt(i).Value.Equals(this.second.ElementAt(i).Value))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}