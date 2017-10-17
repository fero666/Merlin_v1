////////////////////////////////////////////////////////////////////////////////////
// Merlin API for Albion Online v1.0.332.98217-prod
////////////////////////////////////////////////////////////////////////////////////
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;

using Albion.Common.Time;

namespace Merlin.API.Direct
{
    /* Internal type: od */
    public partial struct Number
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        
        private od _internal;
        
        #region Properties
        
        public od Number_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public static Number FromDouble(System.Double A_0) => od.e((System.Double)A_0);
        public static Number FromInt64(long A_0) => od.f((long)A_0);
        public static Number FromRaw(long A_0) => od.e((long)A_0);
        public long GetFractionalPart() => _internal.i();
        public System.Double GetFractions() => _internal.j();
        public long GetIntegerPart() => _internal.h();
        public System.Double ToDouble() => _internal.k();
        //public static bool TryParse(string A_0, out Number A_1) => od.f((string)A_0, out A_1);
        //public static bool TryParseRaw(string A_0, out Number A_1) => od.e((string)A_0, out A_1);
        
        #endregion
        
        #region Constructor
        
        public Number(od instance)
        {
            _internal = instance;
        }
        
        static Number()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator od(Number instance)
        {
            return instance._internal;
        }
        
        public static implicit operator Number(od instance)
        {
            return new Number(instance);
        }
        #endregion
    }
}
