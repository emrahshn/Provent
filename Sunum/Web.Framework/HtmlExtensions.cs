﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using Core;
using Core.Altyapı;
using Services.Hint;
using Web.Framework.Mvc;

namespace Web.Framework
{
    public static class HtmlExtensions
    {
        #region Admin area extensions
        
        
        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector) where T : TemelTSEntityModel
        {
            return DeleteConfirmation(helper, "", buttonsSelector);
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
            string buttonsSelector) where T : TemelTSEntityModel
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Sil";

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-sil-onayı")
                .ToHtmlString();

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Sil", deleteConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        public static MvcHtmlString ActionConfirmation(this HtmlHelper helper, string buttonId, string actionName = "")
        {
            if (string.IsNullOrEmpty(actionName))
                actionName = helper.ViewContext.RouteData.GetRequiredString("action");

            var modalId = MvcHtmlString.Create(buttonId + "-action-confirmation").ToHtmlString();

            var actionConfirmationModel = new ActionConfirmationModel()
            {
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Onay", actionConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\");", buttonId, modalId));
            window.AppendLine(string.Format("$('#{0}-submit-button').attr(\"name\", $(\"#{1}\").attr(\"name\"));", modalId, buttonId));
            window.AppendLine(string.Format("$(\"#{0}\").attr(\"name\", \"\")", buttonId));
            window.AppendLine(string.Format("if($(\"#{0}\").attr(\"type\") == \"submit\")$(\"#{0}\").attr(\"type\", \"button\")", buttonId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue>> forInputExpression,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string>();
            dataInputIds.Add(helper.FieldIdFor(forInputExpression));
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null, dataInputIds.ToArray());
        }
        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue1, TValue2>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue1>> forInputExpression1,
            Expression<Func<TModel, TValue2>> forInputExpression2,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string>();
            dataInputIds.Add(helper.FieldIdFor(forInputExpression1));
            dataInputIds.Add(helper.FieldIdFor(forInputExpression2));
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null, dataInputIds.ToArray());
        }
        public static MvcHtmlString OverrideStoreCheckboxFor<TModel, TValue1, TValue2, TValue3>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            Expression<Func<TModel, TValue1>> forInputExpression1,
            Expression<Func<TModel, TValue2>> forInputExpression2,
            Expression<Func<TModel, TValue3>> forInputExpression3,
            int activeStoreScopeConfiguration)
        {
            var dataInputIds = new List<string>();
            dataInputIds.Add(helper.FieldIdFor(forInputExpression1));
            dataInputIds.Add(helper.FieldIdFor(forInputExpression2));
            dataInputIds.Add(helper.FieldIdFor(forInputExpression3));
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, null, dataInputIds.ToArray());
        }
        public static MvcHtmlString OverrideStoreCheckboxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string parentContainer,
            int activeStoreScopeConfiguration)
        {
            return OverrideStoreCheckboxFor(helper, expression, activeStoreScopeConfiguration, parentContainer);
        }
        private static MvcHtmlString OverrideStoreCheckboxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            int activeStoreScopeConfiguration,
            string parentContainer = null,
            params string[] datainputIds)
        {
            if (String.IsNullOrEmpty(parentContainer) && datainputIds == null)
                throw new ArgumentException("Specify at least one selector");

            var result = new StringBuilder();
            if (activeStoreScopeConfiguration > 0)
            {
                //render only when a certain store is chosen
                const string cssClass = "multi-store-override-option";
                string dataInputSelector = "";
                if (!String.IsNullOrEmpty(parentContainer))
                {
                    dataInputSelector = "#" + parentContainer + " input, #" + parentContainer + " textarea, #" + parentContainer + " select";
                }
                if (datainputIds != null && datainputIds.Length > 0)
                {
                    dataInputSelector = "#" + String.Join(", #", datainputIds);
                }
                var onClick = string.Format("checkOverriddenStoreValue(this, '{0}')", dataInputSelector);
                result.Append(helper.CheckBoxFor(expression, new Dictionary<string, object>
                {
                    { "class", cssClass },
                    { "onclick", onClick },
                    { "data-for-input-selector", dataInputSelector },
                }));
            }
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString RenderBootstrapTabContent(this HtmlHelper helper, string currentTabName,
            HelperResult content, bool isDefaultTab = false, string tabNameToSelect = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;

            var tag = new TagBuilder("div")
            {
                InnerHtml = content.ToHtmlString(),
                Attributes =
                {
                    new KeyValuePair<string, string>("class", string.Format("tab-pane{0}", tabNameToSelect == currentTabName ? " active" : "")),
                    new KeyValuePair<string, string>("id", string.Format("{0}", currentTabName))
                }
            };

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString RenderBootstrapTabHeader(this HtmlHelper helper, string currentTabName,
            string title, bool isDefaultTab = false, string tabNameToSelect = "", string customCssClass = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;

            var a = new TagBuilder("a")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-tab-name", currentTabName),
                    new KeyValuePair<string, string>("href", string.Format("#{0}", currentTabName)),
                    new KeyValuePair<string, string>("data-toggle", "tab"),
                },
                InnerHtml = title
            };
            var liClassValue = "";
            if (tabNameToSelect == currentTabName)
            {
                liClassValue = "active";
            }
            if (!String.IsNullOrEmpty(customCssClass))
            {
                if (!String.IsNullOrEmpty(liClassValue))
                    liClassValue += " ";
                liClassValue += customCssClass;
            }

            var li = new TagBuilder("li")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("class", liClassValue),
                },
                InnerHtml = a.ToString(TagRenderMode.Normal)
            };

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }
        public static string GetSelectedTabName(this HtmlHelper helper)
        {
            //keep this method synchornized with
            //"SaveSelectedTab" method of \Administration\Controllers\BaseAdminController.cs
            var tabName = string.Empty;
            const string dataKey = "cms.selected-tab";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.Controller.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.Controller.TempData[dataKey].ToString();

            return tabName;
        }

        public static string GetSelectedPage(this HtmlHelper helper)
        {
            //keep this method synchornized with
            //"SaveSelectedTab" method of \Administration\Controllers\BaseAdminController.cs
            var tabName = string.Empty;
            const string dataKey = "selected-page";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.Controller.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.Controller.TempData[dataKey].ToString();

            return tabName;
        }

        #region Form fields

        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            //create tag builder
            var builder = new TagBuilder("div");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "ico-help");
            var icon = new StringBuilder();
            icon.Append("<i class='fa fa-question-circle'></i>");
            builder.InnerHtml = icon.ToString();
            //render tag
            return MvcHtmlString.Create(builder.ToString());
        }
        
        public static MvcHtmlString LabelFor_<TModel, TValue>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;

            result.Append(helper.LabelFor(expression, new { title = hintResource, @class = "control-label" }));

            if (!String.IsNullOrEmpty(metadata.DisplayName))
            {
                //var value = metadata.DisplayName;
                var resourceDisplayName = metadata.DisplayName;
                if (resourceDisplayName != null && displayHint)
                {
                    hintResource = EngineContext.Current.Resolve<IHintServisi>()
                        .GetResource(metadata.ContainerType.Name +"."+ metadata.DisplayName);
                    if (!String.IsNullOrEmpty(hintResource))
                    {
                        result.Append(helper.Hint(hintResource).ToHtmlString());
                    }
                }

            }

            var laberWrapper = new TagBuilder("div");
            laberWrapper.Attributes.Add("class", "label-wrapper");
            laberWrapper.InnerHtml = result.ToString();

            return MvcHtmlString.Create(laberWrapper.ToString());
        }

        public static MvcHtmlString EditorFor_<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string postfix = "",
            bool? renderFormControlClass = null, bool required = false)
        {
            var result = new StringBuilder();

            object htmlAttributes = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
                (renderFormControlClass.HasValue && renderFormControlClass.Value))
                htmlAttributes = new { @class = "form-control" };

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.EditorFor(expression, new { htmlAttributes, postfix }));
            else
                result.Append(helper.EditorFor(expression, new { htmlAttributes, postfix }));

            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString EditorFor__<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string postfix = "",
            bool? renderFormControlClass = null, bool required = false,string defaultValue="")
        {
            var result = new StringBuilder();

            object htmlAttributes = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
                (renderFormControlClass.HasValue && renderFormControlClass.Value))
                htmlAttributes = new { @class = "form-control" ,@Value = defaultValue };

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.EditorFor(expression, new { htmlAttributes, postfix }));
            else
                result.Append(helper.EditorFor(expression, new { htmlAttributes, postfix }));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString DropDownList_<TModel>(this HtmlHelper<TModel> helper, string name,
            IEnumerable<SelectListItem> itemList, object htmlAttributes = null,
            bool renderFormControlClass = true, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.DropDownList(name, itemList, attrs));
            else
                result.Append(helper.DropDownList(name, itemList, attrs));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString DropDownListFor_<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> itemList,
            object htmlAttributes = null, bool renderFormControlClass = true, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.DropDownListFor(expression, itemList, attrs));
            else
                result.Append(helper.DropDownListFor(expression, itemList, attrs));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString TextAreaFor_<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null,
            bool renderFormControlClass = true, int rows = 4, int columns = 20, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.TextAreaFor(expression, rows, columns, attrs));
            else
                result.Append(helper.TextAreaFor(expression, rows, columns, attrs));

            return MvcHtmlString.Create(result.ToString());
        }


        public static MvcHtmlString DisplayFor_<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var result = new TagBuilder("div");
            result.Attributes.Add("class", "form-text-row");
            result.InnerHtml = helper.DisplayFor(expression).ToString();

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString Display<TModel>(this HtmlHelper<TModel> helper, string expression)
        {
            var result = new TagBuilder("div");
            result.Attributes.Add("class", "form-text-row");
            result.InnerHtml = expression;

            return MvcHtmlString.Create(result.ToString());
        }

        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control";
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control";

            return htmlAttributes as RouteValueDictionary;
        }
        #endregion

        #endregion

        #region Common extensions

        public static MvcHtmlString RequiredHint(this HtmlHelper helper, string additionalText = null)
        {
            // Create tag builder
            var builder = new TagBuilder("span");
            builder.AddCssClass("required");
            var innerText = "*";
            //add additional text if specified
            if (!String.IsNullOrEmpty(additionalText))
                innerText += " " + additionalText;
            builder.SetInnerText(innerText);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            string exp = ExpressionHelper.GetExpressionText(expression);
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(exp);
            return id.Replace('[', '_').Replace(']', '_');
        }
        public static MvcHtmlString DatePickerDropDowns(this HtmlHelper html,
            string dayName, string monthName, string yearName,
            int? beginYear = null, int? endYear = null,
            int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null,
            bool localizeLabels = true, object htmlAttributes = null, bool wrapTags = false)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            daysList.MergeAttributes(htmlAttributesDictionary, true);
            monthsList.MergeAttributes(htmlAttributesDictionary, true);
            yearsList.MergeAttributes(htmlAttributesDictionary, true);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            string dayLocale, monthLocale, yearLocale;
            dayLocale = "Day";
            monthLocale = "Month";
            yearLocale = "Year";
            

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", dayLocale);
            for (int i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", monthLocale);
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                                    i,
                                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", yearLocale);

            if (beginYear == null)
                beginYear = DateTime.UtcNow.Year - 100;
            if (endYear == null)
                endYear = DateTime.UtcNow.Year;

            if (endYear > beginYear)
            {
                for (int i = beginYear.Value; i <= endYear.Value; i++)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }
            else
            {
                for (int i = beginYear.Value; i >= endYear.Value; i--)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            if (wrapTags)
            {
                string wrapDaysList = "<span class=\"days-list select-wrapper\">" + daysList + "</span>";
                string wrapMonthsList = "<span class=\"months-list select-wrapper\">" + monthsList + "</span>";
                string wrapYearsList = "<span class=\"years-list select-wrapper\">" + yearsList + "</span>";

                return MvcHtmlString.Create(string.Concat(wrapDaysList, wrapMonthsList, wrapYearsList));
            }
            else
            {
                return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
            }

        }

        public static MvcHtmlString Widget(this HtmlHelper helper, string widgetZone, object ekVeri = null, string area = null)
        {
            return helper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone, ekVeri = ekVeri, area = area });
        }
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string suffix)
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new[] { '.' }).Last());
            if (string.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }
            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName)));
            if (!String.IsNullOrEmpty(suffix))
            {
                resolvedLabelText = String.Concat(resolvedLabelText, suffix);
            }
            tag.SetInnerText(resolvedLabelText);

            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            tag.MergeAttributes(dictionary, true);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #endregion
    }
}