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
    /* Internal type: at5 */
    public partial class WorldObject : SimulationObject
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private at5 _internal;
        
        #region Properties
        
        public at5 WorldObject_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public Point2 GetPosition() => _internal.h1();
        public float GetRotation() => _internal.h3();
        
        #endregion
        
        #region Constructor
        
        public WorldObject(at5 instance) : base(instance)
        {
            _internal = instance;
        }
        
        static WorldObject()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator at5(WorldObject instance)
        {
            return instance._internal;
        }
        
        public static implicit operator WorldObject(at5 instance)
        {
            return new WorldObject(instance);
        }
        
        public static implicit operator bool(WorldObject instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
