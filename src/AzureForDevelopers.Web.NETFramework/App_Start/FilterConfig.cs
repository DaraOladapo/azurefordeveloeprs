﻿using System.Web;
using System.Web.Mvc;

namespace AzureForDevelopers.Web.NETFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
