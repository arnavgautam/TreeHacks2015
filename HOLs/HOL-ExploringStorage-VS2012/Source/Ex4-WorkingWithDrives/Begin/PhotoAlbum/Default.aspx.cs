using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotoAlbum
{
    public partial class _Default : Page
    {
        protected string CurrentPath
        {
            get
            {
                string path = Request.Params["path"];
                if (string.IsNullOrEmpty(path))
                {
                    return Global.ImageStorePath;
                }

                return path;
            }
        }

        protected void LinqDataSource1_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new PhotoAlbumDataSource(this.CurrentPath);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.GridView1.Columns[this.GridView1.Columns.Count - 1].Visible = this.CurrentPath != Global.ImageStorePath;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                string fileName = ((GridView)e.CommandSource).DataKeys[index].Value as string;
                File.Delete(fileName);
                this.SelectImageStore(this.CurrentPath);
            }
        }

        private void SelectImageStore(string path)
        {
            Response.Redirect(this.Request.Path + "?path=" + path);
        }
    }
}