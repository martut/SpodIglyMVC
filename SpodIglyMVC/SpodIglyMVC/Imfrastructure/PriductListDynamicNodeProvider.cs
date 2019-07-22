using MvcSiteMapProvider;
using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Imfrastructure
{

    public class PriductListDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext Db = new StoreContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();
            foreach (Genre a in Db.Genre)
            {
                DynamicNode n = new DynamicNode();
                n.Title = a.Name;
                n.Key = "Genre_" + a.GenreId;
                n.RouteValues.Add("genrename", a.Name);
                returnValue.Add(n);
            }
            return returnValue;
        }

    }
}