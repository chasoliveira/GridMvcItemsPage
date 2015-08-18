using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridViewNumericList.Models;

namespace GridViewNumericList.Filters
{
    public class GridMvcAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = (Controller)filterContext.Controller;
            var rd = filterContext.RouteData;
            var request = filterContext.HttpContext.Request;
            var model = controller.ViewData.Model as IQueryable<Object>;

            var count = model != null ? model.Count() : 0;

            int pagesize, page;

            if (!int.TryParse(request.Params.Get("grid-page-size"), out pagesize))
                pagesize = GridMvcFilter.DefaultPagesize;

            if (!int.TryParse(request.Params.Get("grid-page"), out page))
                page = 1;

            var tbFilter = new GridMvcFilter
            {
                Id = rd.Values["id"] as string,
                Action = rd.GetRequiredString("action"),
                Controller = rd.GetRequiredString("controller"),
                GridPage = page,
                GridPageSize = pagesize,
                GridFilter = request.Params.Get("grid-filter"),
                GridOrd = request.Params.Get("grid-ord"),
            };

            controller.ViewBag.ItemsPerPage = pagesize;
            controller.ViewBag.ListDropDown = GetListsDropDown(tbFilter, count);  

        }
        private static IEnumerable<SelectListItem> GetListsDropDown(GridMvcFilter tbFilter, int max)
        {

            var itemSelected = tbFilter.GridPageSize ?? GridMvcFilter.DefaultPagesize;
            var list = new List<SelectListItem>();
            const int defaultPageSize = GridMvcFilter.DefaultPagesize;

            if (max <= defaultPageSize)
                list.Add(new SelectListItem
                {
                    Value = SetLink(tbFilter, defaultPageSize.ToString(CultureInfo.CurrentCulture)),
                    Text = defaultPageSize.ToString(CultureInfo.CurrentCulture),
                    Selected = true
                });
            else
            {
                for (var i = defaultPageSize; i < max + defaultPageSize; i += defaultPageSize)
                {
                    list.Add(new SelectListItem
                    {
                        Value = SetLink(tbFilter, i.ToString(CultureInfo.CurrentCulture)),
                        Text = i.ToString(CultureInfo.CurrentCulture),
                        Selected = itemSelected == i
                    });
                }
            }
            return list;
        }

        private static string SetLink(GridMvcFilter tblParams, string value)
        {
            var filter = tblParams.GridFilter != null ? tblParams.GridFilter.Split(',') : new string[] { };

            var gridfilter = "";
            if (filter.Length <= 0)
                return String.Format("/{0}{1}?grid-page={2}&grid-page-size={3}{4}{5}",
                    tblParams.Controller,
                    (!string.IsNullOrEmpty(tblParams.Action) ? "/" + tblParams.Action : "") +
                    (!string.IsNullOrEmpty(tblParams.Id) ? ("/" + tblParams.Id) : ""),
                    tblParams.GridPage,
                    value,
                    tblParams.GridOrd != null ? ("&grid-ord=" + tblParams.GridOrd) : "",
                    gridfilter);

            gridfilter = filter.Where(item => item.Length > 0).Aggregate(gridfilter, (current, item) => current + ("&grid-filter=" + item));

            return String.Format("/{0}{1}?grid-page={2}&grid-page-size={3}{4}{5}",
               tblParams.Controller,
               (!string.IsNullOrEmpty(tblParams.Action) ? "/" + tblParams.Action : "") + (!string.IsNullOrEmpty(tblParams.Id) ? ("/" + tblParams.Id) : ""),
               tblParams.GridPage,
               value,
               tblParams.GridOrd != null ? ("&grid-ord=" + tblParams.GridOrd) : "",
               gridfilter);
        } 
    }
}