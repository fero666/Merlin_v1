////////////////////////////////////////////////////////////////////////////////////
// Merlin API for Albion Online v1.0.327.97446-live
////////////////////////////////////////////////////////////////////////////////////
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Merlin.API.Direct
{
    /* Internal type: gz.SpellAssessment */
    public enum SpellAssessment
    {
        Negative = 0,
        Positive = 1
    }
    
    public static class SpellAssessmentExtensions
    {
        public static gz.SpellAssessment ToInternal(this SpellAssessment instance)
        {
            return (gz.SpellAssessment)instance;
        }
        
        public static SpellAssessment ToWrapped(this gz.SpellAssessment instance)
        {
            return (SpellAssessment)instance;
        }
    }
}