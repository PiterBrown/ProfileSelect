using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfileSelect
{
    public class BindExclude : ActionFilterAttribute
    {
        public string Exclude { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Exclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                ToList().ForEach(x => filterContext.Controller.ViewData.ModelState.Remove(x.Trim()));
            base.OnActionExecuting(filterContext);
        }
    }
}
