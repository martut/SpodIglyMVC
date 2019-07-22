using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Imfrastructure
{
    public interface ICacheProvider
    {
        object Get(string Key);

        void Set(string Key, object data, int cacheTime);
        bool IsSet(string Key);
        void Invalidate(string Key);

    }
}