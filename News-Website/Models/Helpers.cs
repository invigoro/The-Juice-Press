using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public static class Helpers
    {
        private static Random random = new Random();
        public static string RandomString(int length = 10, string prefix = "")
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return prefix + new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

        public static string FormattedDaysAgo(DateTime d)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - d.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static async Task<bool> IsInRoleAsync(this UserManager<User> manager, User user, string role)
        {
            var roles = await manager.GetRolesAsync(user);
            return roles.Contains(role);
        }
        public static bool IsInRole(this UserManager<User> manager, User user, string role)
        {
            var roles = manager.GetRolesAsync(user).Result;
            return roles.Contains(role);
        }
        public static async Task<bool> IsInAnyRoleAsync(this UserManager<User> manager, User user, string roles)
        {
            var rolesSplit = roles.Split(",");
            foreach(var r in rolesSplit)
            {
                if (await manager.IsInRoleAsync(user, r)) return true;
            }
            return false;
        }
        public static bool IsInAnyRole(this UserManager<User> manager, User user, string roles)
        {
            var rolesSplit = roles.Split(",");
            foreach (var r in rolesSplit)
            {
                if (manager.IsInRole(user, r)) return true;
            }
            return false;
        }
        public static async Task<bool> IsInAllRolesAsync(this UserManager<User> manager, User user, string roles)
        {
            var rolesSplit = roles.Replace(" ", "").Split(",");
            foreach (var r in rolesSplit)
            {
                if (!(await manager.IsInRoleAsync(user, r))) return false;
            }
            return true;
        }
        public static bool IsInAllRoles(this UserManager<User> manager, User user, string roles)
        {
            var rolesSplit = roles.Replace(" ", "").Split(",");
            foreach (var r in rolesSplit)
            {
                if (!manager.IsInRole(user, r)) return false;
            }
            return true;
        }

        public static async Task<bool> IsInRoleAsync(this UserManager<User> manager, string role)
        {
            var currentUser = await manager.GetUserAsync(System.Security.Claims.ClaimsPrincipal.Current);
            return await manager.IsInRoleAsync(currentUser, role);
        }
        public static bool IsInRole(this UserManager<User> manager, string role)
        {
            var currentUser = manager.GetUserAsync(ClaimsPrincipal.Current).Result;
            return manager.IsInRole(currentUser, role);
        }

        public static bool IsInAllRoles(this ClaimsPrincipal claim, string roles)
        {
            var rolesSplit = roles.Replace(" ", "").Split(",");
            foreach (var r in rolesSplit)
            {
                if (!claim.IsInRole(r)) return false;
            }
            return true;
        }
        public static bool IsInAnyRole(this ClaimsPrincipal claim, string roles)
        {
            var rolesSplit = roles.Replace(" ", "").Split(",");
            foreach (var r in rolesSplit)
            {
                if (claim.IsInRole(r)) return true;
            }
            return false;
        }
    }

    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        //public static List<string> GetDisplayValues()
        //{
        //    var values = Enum.GetValues(typeof(T));
        //    foreach (var t in values) {
        //        EnumHelper<T>.
        //    }
        //}
    }
}
