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
    /* Internal type: d0 */
    public partial class CharactersDataFile : DataFile
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private d0 _internal;
        
        #region Properties
        
        public d0 CharactersDataFile_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        
        #endregion
        
        #region Constructor
        
        public CharactersDataFile(d0 instance) : base(instance)
        {
            _internal = instance;
        }
        
        static CharactersDataFile()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator d0(CharactersDataFile instance)
        {
            return instance._internal;
        }
        
        public static implicit operator CharactersDataFile(d0 instance)
        {
            return new CharactersDataFile(instance);
        }
        
        public static implicit operator bool(CharactersDataFile instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
