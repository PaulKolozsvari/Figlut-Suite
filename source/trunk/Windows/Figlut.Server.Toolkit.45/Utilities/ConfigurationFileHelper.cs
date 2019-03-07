namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel.Configuration;
    using System.ServiceModel.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ConfigurationFileHelper
    {
        #region Methods

        public static void SetApplicationServiceModelPerformanceCounters(PerformanceCounterScope performanceCounterScope)
        {
            //Performance Counters: http://blogs.microsoft.co.il/idof/2011/08/11/wcf-scaling-check-your-counters/
            //Performance Counters: https://docs.microsoft.com/en-us/dotnet/framework/wcf/diagnostics/performance-counters/
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServiceModelSectionGroup sg = ServiceModelSectionGroup.GetSectionGroup(config);
            sg.Diagnostic.PerformanceCounters = performanceCounterScope;
            config.Save();
        }

        #endregion //Methods
    }
}
