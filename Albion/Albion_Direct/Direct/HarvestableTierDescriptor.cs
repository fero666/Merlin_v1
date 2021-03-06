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
using System.Linq;

namespace Albion_Direct
{
    /* Internal type: h9.b */
    public partial class HarvestableTierDescriptor
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private h9.b _internal;
        
        #region Properties
        
        public h9.b HarvestableTierDescriptor_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public HarvestableChargeDescriptor[] GetCharges() => _internal.a0().Select(x =>(HarvestableChargeDescriptor)x).ToArray();
        public HarvestableRareStateDescriptor[] GetRareStates() => _internal.av().Select(x =>(HarvestableRareStateDescriptor)x).ToArray();
        public bool RequiresTool() => _internal.aj();
        
        #endregion
        
        #region Constructor
        
        public HarvestableTierDescriptor(h9.b instance)
        {
            _internal = instance;
        }
        
        static HarvestableTierDescriptor()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator h9.b(HarvestableTierDescriptor instance)
        {
            return instance._internal;
        }
        
        public static implicit operator HarvestableTierDescriptor(h9.b instance)
        {
            return new HarvestableTierDescriptor(instance);
        }
        
        public static implicit operator bool(HarvestableTierDescriptor instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
