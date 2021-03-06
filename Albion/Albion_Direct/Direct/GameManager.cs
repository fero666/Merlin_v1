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
    /* Internal type: bb3 */
    public partial class GameManager
    {
        private static List<MethodInfo> _methodReflectionPool = new List<MethodInfo>();
        private static List<PropertyInfo> _propertyReflectionPool = new List<PropertyInfo>();
        private static List<FieldInfo> _fieldReflectionPool = new List<FieldInfo>();
        
        private bb3 _internal;
        
        #region Properties
        
        public bb3 GameManager_Internal => _internal;
        
        #endregion
        
        #region Fields
        
        
        #endregion
        
        #region Methods
        
        public static GameManager GetInstance() => bb3.t();
        public LandscapeManager GetLandscapeManager() => _internal.aa();
        public LocalPlayerCharacterView GetLocalPlayerCharacterView() => _internal.w();
        public GameState GetState() => _internal.x().ToWrapped();
        public SimulationObjectView GetView(SimulationObject A_0) => _internal.a((au1)A_0);
        public SimulationObjectView GetView(long A_0) => _internal.a((long)A_0);
        
        #endregion
        
        #region Constructor
        
        public GameManager(bb3 instance)
        {
            _internal = instance;
        }
        
        static GameManager()
        {
            
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator bb3(GameManager instance)
        {
            return instance._internal;
        }
        
        public static implicit operator GameManager(bb3 instance)
        {
            return new GameManager(instance);
        }
        
        public static implicit operator bool(GameManager instance)
        {
            return instance._internal != null;
        }
        #endregion
    }
}
