using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UniversityAPP.Utilities
{
    public static class ExtensionMethod
    {
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.Values.Select(x => x.Errors));
        }
    }
}
