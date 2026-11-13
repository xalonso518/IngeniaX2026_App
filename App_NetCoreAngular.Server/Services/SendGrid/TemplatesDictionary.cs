using System;
using System.Collections.Generic;
using System.Text;
using static App_NetCoreAngular.Server.Enums.EnumSystem;

namespace App_NetCoreAngular.SendGrid
{
    public static class TemplatesDictionary
    {
        public static readonly Dictionary<TypeTemplate, string> TEMPLATES = new Dictionary<TypeTemplate, string>()
        {
            { TypeTemplate.Bienvenida , "d-af39727d85534b4b96f393068faf2611" },
            { TypeTemplate.ResetPassword , "d-43da04d10f77498b9e553b838269e148"},
            { TypeTemplate.CorreoPrueba , "d-af39727d85534b4b96f393068faf26a1"},
        };
    };
}
