using Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Framework.UI;

namespace Web.Models.Tanımlamalar
{
    public partial class KisilerPagingFilteringModel : TemelSayfalanabilirModel
    {
        #region Ctor
        public KisilerPagingFilteringModel()
        {
            this.AvailableSortOptions = new List<SelectListItem>();
            this.AvailableViewModes = new List<SelectListItem>();
            this.PageSizeOptions = new List<SelectListItem>();
            this.SpecificationFilter = new SpecificationFilterModel();
        }

        #endregion

        #region Properties
        public SpecificationFilterModel SpecificationFilter { get; set; }
        public bool AllowProductSorting { get; set; }
        public IList<SelectListItem> AvailableSortOptions { get; set; }
        public bool AllowProductViewModeChanging { get; set; }
        public IList<SelectListItem> AvailableViewModes { get; set; }
        public bool AllowCustomersToSelectPageSize { get; set; }
        public IList<SelectListItem> PageSizeOptions { get; set; }
        public int? OrderBy { get; set; }
        public string ViewMode { get; set; }

        #endregion

        #region Nested classes
        public partial class SpecificationFilterModel : TemelTSModel
        {
            #region Const

            private const string QUERYSTRINGPARAM = "specs";

            #endregion

            #region Ctor
            public SpecificationFilterModel()
            {
                this.AlreadyFilteredItems = new List<SpecificationFilterItem>();
                this.NotFilteredItems = new List<SpecificationFilterItem>();
            }

            #endregion

            #region Utilities
            protected virtual string ExcludeQueryStringParams(string url, IWebYardımcısı webHelper)
            {
                //comma separated list of parameters to exclude
                const string excludedQueryStringParams = "pagenumber";
                var excludedQueryStringParamsSplitted = excludedQueryStringParams.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string exclude in excludedQueryStringParamsSplitted)
                    url = webHelper.SorguSil(url, exclude);
                return url;
            }
            protected virtual string GenerateFilteredSpecQueryParam(IList<int> optionIds)
            {
                if (optionIds == null)
                    return "";

                string result = string.Join(",", optionIds);
                return result;
            }

            #endregion

            #region Methods
            public virtual List<int> GetAlreadyFilteredSpecOptionIds(IWebYardımcısı webHelper)
            {
                var result = new List<int>();

                var alreadyFilteredSpecsStr = webHelper.Sorgu<string>(QUERYSTRINGPARAM);
                if (String.IsNullOrWhiteSpace(alreadyFilteredSpecsStr))
                    return result;

                foreach (var spec in alreadyFilteredSpecsStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int specId;
                    int.TryParse(spec.Trim(), out specId);
                    if (!result.Contains(specId))
                        result.Add(specId);
                }
                return result;
            }
           

            #endregion

            #region Properties
            public bool Enabled { get; set; }
            public IList<SpecificationFilterItem> AlreadyFilteredItems { get; set; }
            public IList<SpecificationFilterItem> NotFilteredItems { get; set; }
            public string RemoveFilterUrl { get; set; }

            #endregion
        }
        public partial class SpecificationFilterItem : TemelTSModel
        {
            public string SpecificationAttributeName { get; set; }
            public string SpecificationAttributeOptionName { get; set; }
            public string SpecificationAttributeOptionColorRgb { get; set; }
            public string FilterUrl { get; set; }
        }

        #endregion
    }
}