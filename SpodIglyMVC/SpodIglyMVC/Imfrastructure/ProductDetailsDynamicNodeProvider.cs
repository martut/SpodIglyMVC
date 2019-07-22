using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;

namespace SpodIglyMVC.Imfrastructure
{
    public class ProductDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext Db = new StoreContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();
            foreach (Album a in Db.Album)
            {
                DynamicNode n = new DynamicNode();
                n.Title = a.AlbumTitle;
                n.Key = "Album_" + a.AlbumId;
                n.ParentKey = "Genre_" + a.GenreId;
                n.RouteValues.Add("id", a.AlbumId);
                returnValue.Add(n);
            }
            return returnValue;
        }

    }
}