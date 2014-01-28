namespace CloudShop.Store
{
    using System;
    using System.Collections.Generic;

    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var itemsInSession = this.Session["Cart"] as List<string>;

            if (itemsInSession != null)
            {
                foreach (var item in itemsInSession)
                {
                    this.cart.Items.Add(item);
                }
            }
        }

        protected void RemoveItem_Click(object sender, EventArgs e)
        {
            var selectedItem = this.cart.SelectedItem;
            if (selectedItem != null)
            {
                var itemsInSession = this.Session["Cart"] as List<string>;
                if (itemsInSession != null)
                {
                    itemsInSession.Remove(selectedItem.ToString());
                    this.cart.Items.Remove(selectedItem.ToString());
                }
            }
        }
    }
}
