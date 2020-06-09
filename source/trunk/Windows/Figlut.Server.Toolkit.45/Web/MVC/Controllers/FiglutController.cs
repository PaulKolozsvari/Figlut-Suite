namespace Figlut.Server.Toolkit.Web.MVC.Controllers
{
    #region Using Directives

    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Web;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Web.MVC.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    #endregion //Using Directives

    public class FiglutController : Controller
    {
        #region Constants

        protected const string CONFIRMATION_DIALOG_PARTIAL_VIEW_NAME = "_ConfirmationDialog";
        protected const string CONFIRMATION_DIALOG_DIV_ID = "dlgConfirmation"; //Used to manage div making up the dialog.

        protected const string WAIT_DIALOG_PARTIAL_VIEW_NAME = "_WaitDialog";
        protected const string WAIT_DIALOG_DIV_ID = "dlgWait";

        protected const string INFORMATION_DIALOG_PARTIAL_VIEW_NAME = "_InformationDialog";
        protected const string INFORMATION_DIALOG_DIV_ID = "dlgInformation";

        protected const string WEB_REQUEST_ACTIVITY_SOURCE_APPLICATION = "WebSite";

        #endregion //Constants

        #region Actions

        public virtual ActionResult WaitDialog(string message)
        {
            try
            {
                WaitModel model = new WaitModel();
                model.PostBackControllerAction = GetCurrentActionName();
                model.PostBackControllerName = GetCurrentControllerName();
                model.DialogDivId = WAIT_DIALOG_DIV_ID;
                model.WaitMessage = message == null ? string.Empty : message;
                PartialViewResult result = PartialView(WAIT_DIALOG_PARTIAL_VIEW_NAME, model);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return GetJsonResult(false, ex.Message);
            }
        }

        public virtual ActionResult InformationDialog(string message)
        {
            try
            {
                InformationModel model = new InformationModel();
                model.PostBackControllerAction = GetCurrentActionName();
                model.PostBackControllerName = GetCurrentControllerName();
                model.DialogDivId = INFORMATION_DIALOG_DIV_ID;
                model.InformationMessage = message == null ? string.Empty : message;
                PartialViewResult result = PartialView(INFORMATION_DIALOG_PARTIAL_VIEW_NAME, model);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return GetJsonResult(false, ex.Message);
            }
        }

        #endregion //Actions

        #region Methods

        protected virtual string GetCurrentActionName()
        {
            string result = this.ControllerContext.RouteData.Values["action"].ToString();
            return result;
        }

        protected virtual string GetCurrentControllerName()
        {
            string result = this.ControllerContext.RouteData.Values["controller"].ToString();
            return result;
        }

        protected virtual string GetWebRequestVerb()
        {
            return Request.RequestType;
        }

        protected virtual string GetFullRequestUrl()
        {
            return Request.Url != null ? Request.Url.AbsoluteUri : null;
        }

        protected virtual string GetFullRequestReferrerUrl()
        {
            return Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : null;
        }

        protected virtual string GetUserAgent()
        {
            return Request.UserAgent;
        }

        protected virtual string GetUserHostAddress()
        {
            return Request.UserHostAddress;
        }

        protected virtual string GetUserHostName()
        {
            return Request.UserHostName;
        }

        protected virtual bool IsCrawler()
        {
            return Request.Browser == null ? false : Request.Browser.Crawler;
        }

        protected virtual bool IsMobileDevice()
        {
            return Request.Browser == null ? false : Request.Browser.IsMobileDevice;
        }

        protected virtual string GetMobileDeviceManufacturer()
        {
            return IsMobileDevice() ? Request.Browser.MobileDeviceManufacturer : null;
        }

        protected virtual string GetMobileDeviceModel()
        {
            return IsMobileDevice() ? Request.Browser.MobileDeviceModel : null;
        }

        protected virtual string GetPlatform()
        {
            return Request.Browser == null ? null : Request.Browser.Platform;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            if (actionName.Contains("Dialog") || actionName.ToLower().Contains("dialog") || controllerName == "WebRequestActivity")
            {
                return;
            }
            LogHeaders();
            base.OnActionExecuting(filterContext);
        }

        protected virtual bool ExcludeWebRequestActivityByUserAgent(string currentUserAgent, List<string> userAgentsToExclude)
        {
            string currentUserAgentLower = currentUserAgent.Trim().ToLower();
            foreach (string userAgentText in userAgentsToExclude)
            {
                string s = userAgentText.Trim().ToLower();
                if (currentUserAgentLower.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual string GetClientType(bool isCrawler, bool isMobileDevice)
        {
            string result = null;
            if (isMobileDevice)
            {
                result = "Mobile";
            }
            else if (isCrawler)
            {
                result = "Crawler";
            }
            else
            {
                result = "PC";
            }
            return result;
        }

        protected virtual void LogHeaders()
        {
            string allHeadersFullString = GetAllHeadersFullString();
            string allHeadersFormatted = GetAllHeadersFormatted();
            GOC.Instance.Logger.LogMessage(new LogMessage(allHeadersFullString, LogMessageType.Information, LoggingLevel.Maximum));
        }

        protected virtual void SetViewBagErrorMessage(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
        }

        protected virtual JsonResult GetJsonResult(bool success)
        {
            return Json(new { Success = success, ErrorMsg = string.Empty });
        }

        protected virtual JsonResult GetJsonResult(bool success, string errorMessage)
        {
            return Json(new { Success = success, ErrorMsg = errorMessage });
        }

        protected virtual JsonResult GetJsonFileResult(bool success, string fileName)
        {
            return Json(new { Success = success, FileName = fileName });
        }

        protected virtual RedirectToRouteResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        protected virtual RedirectToRouteResult RedirectToError(string message)
        {
            return RedirectToAction("Error", "Information", new { errorMessage = message });
        }

        protected virtual RedirectToRouteResult RedirectToInformation(string message)
        {
            return RedirectToAction("Information", "Information", new { informationMessage = message });
        }

        protected virtual RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }

        #region Header Methods

        protected virtual string GetAllHeadersFullString()
        {
            return Request.Headers.ToString();
        }

        protected virtual string GetAllHeadersFormatted()
        {
            StringBuilder result = new StringBuilder();
            foreach (var key in Request.Headers.AllKeys)
            {
                result.AppendLine(string.Format("{0}={1}", key, Request.Headers[key]));
            }
            return result.ToString();
        }

        protected virtual string GetHeader(string key, bool throwExceptionOnNotFound)
        {
            string result = string.Empty;
            if (Request != null && Request.Headers.HasKeys())
            {
                result = Request.Headers.Get(key);
            }
            if (result == null && throwExceptionOnNotFound)
            {
                throw new NullReferenceException(string.Format("Could not find HTTP Header with key {0}.", key));
            }
            return result;
        }

        protected virtual FileContentResult GetCsvFileResult<E>(EntityCache<Guid, E> cache) where E : class
        {
            string filePath = Path.GetTempFileName();
            cache.ExportToCsv(filePath, null, false, false);
            string downloadFileName = string.Format("{0}-{1}.csv", typeof(E).Name, DateTime.Now);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, "text/plain", downloadFileName);
        }

        protected virtual void GetConfirmationModelFromSearchParametersString(
            string searchParametersString,
            out string[] searchParameters,
            out string searchText)
        {
            searchText = string.Empty;
            searchParameters = searchParametersString.Split('|');
            if (!string.IsNullOrEmpty(searchParametersString) && searchParameters.Length > 0)
            {
                searchText = searchParameters[0];
            }
        }

        protected virtual void GetConfirmationModelFromSearchParametersString(
            string searchParametersString,
            out string[] searchParameters,
            out string searchText,
            out Nullable<DateTime> startDate,
            out Nullable<DateTime> endDate)
        {
            searchText = string.Empty;
            startDate = null;
            endDate = null;
            searchParameters = searchParametersString.Split('|');
            if (!string.IsNullOrEmpty(searchParametersString) && searchParameters.Length >= 3)
            {
                searchText = searchParameters[0];
                DateTime startDateParsed;
                DateTime endDateParsed;
                if (DateTime.TryParse(searchParameters[1], out startDateParsed))
                {
                    startDate = startDateParsed;
                }
                if (DateTime.TryParse(searchParameters[2], out endDateParsed))
                {
                    endDate = endDateParsed;
                }
            }
        }

        protected virtual void GetConfirmationModelFromSearchParametersString(
            string searchParametersString,
            out string[] searchParameters,
            out string searchText,
            out Nullable<DateTime> startDate,
            out Nullable<DateTime> endDate,
            out Nullable<Guid> parentId)
        {
            searchText = string.Empty;
            startDate = null;
            endDate = null;
            searchParameters = searchParametersString.Split('|');
            parentId = null;
            if (!string.IsNullOrEmpty(searchParametersString) && searchParameters.Length >= 4)
            {
                searchText = searchParameters[0];
                DateTime startDateParsed;
                DateTime endDateParsed;
                if (DateTime.TryParse(searchParameters[1], out startDateParsed))
                {
                    startDate = startDateParsed;
                }
                if (DateTime.TryParse(searchParameters[2], out endDateParsed))
                {
                    endDate = endDateParsed;
                }
                Guid entityIdGuid;
                if (Guid.TryParse(searchParameters[3], out entityIdGuid))
                {
                    parentId = entityIdGuid;
                }
            }
        }

        protected virtual void GetConfirmationModelFromSearchParametersString(
            string searchParametersString,
            out string[] searchParameters,
            out string searchText,
            out Nullable<DateTime> startDate,
            out Nullable<DateTime> endDate,
            out string parentName,
            out Nullable<Guid> parentId)
        {
            searchText = string.Empty;
            startDate = null;
            endDate = null;
            searchParameters = searchParametersString.Split('|');
            parentName = null;
            parentId = null;
            if (!string.IsNullOrEmpty(searchParametersString) && searchParameters.Length >= 5)
            {
                searchText = searchParameters[0];
                DateTime startDateParsed;
                DateTime endDateParsed;
                if (DateTime.TryParse(searchParameters[1], out startDateParsed))
                {
                    startDate = startDateParsed;
                }
                if (DateTime.TryParse(searchParameters[2], out endDateParsed))
                {
                    endDate = endDateParsed;
                }
                parentName = searchParameters[3];
                Guid entityIdGuid;
                if (Guid.TryParse(searchParameters[4], out entityIdGuid))
                {
                    parentId = entityIdGuid;
                }
            }
        }

        protected virtual void GetConfirmationModelFromSearchParametersString(
            string searchParametersString,
            out string[] searchParameters,
            out string searchText,
            out Nullable<DateTime> startDate,
            out Nullable<DateTime> endDate,
            out Nullable<Guid> parentId,
            out Nullable<Guid> secondParentId)
        {
            searchText = string.Empty;
            startDate = null;
            endDate = null;
            searchParameters = searchParametersString.Split('|');
            parentId = null;
            secondParentId = null;
            if (!string.IsNullOrEmpty(searchParametersString) && searchParameters.Length >= 5)
            {
                searchText = searchParameters[0];
                DateTime startDateParsed;
                DateTime endDateParsed;
                if (DateTime.TryParse(searchParameters[1], out startDateParsed))
                {
                    startDate = startDateParsed;
                }
                if (DateTime.TryParse(searchParameters[2], out endDateParsed))
                {
                    endDate = endDateParsed;
                }
                Guid parentIdGuid;
                if (Guid.TryParse(searchParameters[3], out parentIdGuid))
                {
                    parentId = parentIdGuid;
                }
                Guid secondParentIdGuid;
                if (Guid.TryParse(searchParameters[4], out secondParentIdGuid))
                {
                    secondParentId = secondParentIdGuid;
                }
            }
        }

        #endregion //Header Methods

        #endregion //Methods
    }
}