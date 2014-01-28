namespace CloudShop.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var products = this.Application["Products"] as List<string>;
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            
            // add all products currently not in session
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            //// Add additional filters here

            foreach (var product in filteredProducts)
            {
                this.products.Items.Add(product);
            }
        }

        protected void AddItem_Click(object sender, EventArgs e)
        {
            ListItem selectedItem = this.products.SelectedItem;
            if (selectedItem != null)
            {
                List<string> cart = this.Session["Cart"] as List<string> ?? new List<string>();
                cart.Add(this.products.SelectedItem.ToString());
                this.products.Items.Remove(selectedItem);
                Session["Cart"] = cart;
            }
        }
    }
}
