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
    /* Internal type: afd */
    public partial class GameDataFile : DataFile
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private afd _internal;
        
        #region Properties
        
        public afd GameDataFile_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        
        #endregion
        
        #region Constructor
        
        public GameDataFile(afd instance) : base(instance)
        {
            _internal = instance;
        }
        
        static GameDataFile()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator afd(GameDataFile instance)
        {
            return instance._internal;
        }
        
        public static implicit operator GameDataFile(afd instance)
        {
            return new GameDataFile(instance);
        }
        
        public static implicit operator bool(GameDataFile instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
