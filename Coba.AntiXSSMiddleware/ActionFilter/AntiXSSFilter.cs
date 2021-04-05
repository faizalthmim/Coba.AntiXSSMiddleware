using Coba.AntiXSSMiddleware.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coba.AntiXSSMiddleware.ActionFilter
{
    public class AntiXSSFilter : IActionFilter
    {
        private static readonly char[] StartingChars = { '<', '&' };

        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.HttpContext.Request.HasFormContentType)
            {
                var a = context.HttpContext.Request.Form;
                if (a.Any(m => m.Value.Any(y => IsDangerousString(y))))
                {
                    bool isAjaxCall = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
                    if (isAjaxCall)
                    {
                        context.Result = new RedirectToActionResult("ErrorXssAjax", "Home", null);
                    }
                    else
                    {
                        context.Result = new RedirectToActionResult("Error", "Home", null);
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }
        public static bool IsDangerousString(string s)
        {
            //bool inComment = false;

            for (var i = 0; ;)
            {

                // Look for the start of one of our patterns 
                var n = s.IndexOfAny(StartingChars, i);

                // If not found, the string is safe
                if (n < 0) return false;

                // If it's the last char, it's safe 
                if (n == s.Length - 1) return false;


                switch (s[n])
                {
                    case '<':
                        // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                        if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                        break;
                    case '&':
                        // If the & is followed by a #, it's unsafe (e.g. S) 
                        if (s[n + 1] == '#') return true;
                        break;

                }

                // Continue searching
                i = n + 1;
            }
        }

        private static bool IsAtoZ(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }
}
