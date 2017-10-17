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
    /* Internal type: a3p */
    public partial class TerritoryObject : StaticObject
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private a3p _internal;
        
        #region Properties
        
        public a3p TerritoryObject_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public TerritoryDescriptor GetDescriptor() => _internal.s4();
        
        #endregion
        
        #region Constructor
        
        public TerritoryObject(a3p instance) : base(instance)
        {
            _internal = instance;
        }
        
        static TerritoryObject()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator a3p(TerritoryObject instance)
        {
            return instance._internal;
        }
        
        public static implicit operator TerritoryObject(a3p instance)
        {
            return new TerritoryObject(instance);
        }
        
        public static implicit operator bool(TerritoryObject instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
