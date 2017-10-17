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
    /* Internal type: a9x */
    public partial class LandscapeManager
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private a9x _internal;
        
        #region Properties
        
        public a9x LandscapeManager_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public float GetTerrainHeight(Point2 A_0, out RaycastHit A_1) => _internal.d((aky)A_0, out A_1);
        
        #endregion
        
        #region Constructor
        
        public LandscapeManager(a9x instance)
        {
            _internal = instance;
        }
        
        static LandscapeManager()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator a9x(LandscapeManager instance)
        {
            return instance._internal;
        }
        
        public static implicit operator LandscapeManager(a9x instance)
        {
            return new LandscapeManager(instance);
        }
        
        public static implicit operator bool(LandscapeManager instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
