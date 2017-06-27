namespace Figlut.Server.Toolkit.Web.Client.Funnel
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    public class FunnerlServiceResultHandler
    {
        #region Methods

        public static bool HandleServiceResult(FunnelServiceResult result)
        {
            bool abort = false;
            switch (result.Code)
            {
                case FunnelServiceResultCode.Success:
                    break;
                case FunnelServiceResultCode.Information:
                    UIHelper.DisplayInformation(result.Message);
                    break;
                case FunnelServiceResultCode.SpecialInstructions:
                    //using (SpecialInstructionsForm f = new SpecialInstructionsForm(result.Message))
                    //{
                    //    f.ShowDialog();
                    //}
                    throw new NotImplementedException("SpecialInstructions");
                case FunnelServiceResultCode.Warning:
                    UIHelper.DisplayWarning(result.Message);
                    break;
                case FunnelServiceResultCode.OperationError:
                    UIHelper.DisplayError(result.Message);
                    abort = true;
                    break;
                case FunnelServiceResultCode.FatalError:
                    throw new Exception(result.Message);
                    break;
                default:
                    throw new Exception("Invalid service result.");
            }
            return abort;
        }

        #endregion //Methods
    }
}
