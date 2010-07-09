using System.Collections.Generic;
using horn.services.core.Value;

namespace Horn.Services.Core.Extensions
{
    public static class ResourceExtensions
    {
        public static string GetResourceUrl(this IResource resource)
        {
            var list = new List<string> {resource.Name};

            var parent = resource.Parent;

            while (parent != null)
            {
                if(parent.IsRoot)
                    break;

                list.Add(parent.Name);

                parent = parent.Parent;
            }

            list.Reverse();

            var url = string.Empty;

            list.ForEach(x => url +=string.Format("{0}/", x));

            return url;
        }
    }
}
