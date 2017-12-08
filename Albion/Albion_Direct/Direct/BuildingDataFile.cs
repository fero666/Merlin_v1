////////////////////////////////////////////////////////////////////////////////////
// Merlin API for Albion Online v1.0.336.100246-prod
////////////////////////////////////////////////////////////////////////////////////
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;

namespace Albion_Direct
{
    /* Internal type: aft */
    public partial class BuildingDataFile : DataFile
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private aft _internal;
        
        #region Properties
        
        public aft BuildingDataFile_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        
        #endregion
        
        #region Constructor
        
        public BuildingDataFile(aft instance) : base(instance)
        {
            _internal = instance;
        }
        
        static BuildingDataFile()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator aft(BuildingDataFile instance)
        {
            return instance._internal;
        }
        
        public static implicit operator BuildingDataFile(aft instance)
        {
            return new BuildingDataFile(instance);
        }
        
        public static implicit operator bool(BuildingDataFile instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
