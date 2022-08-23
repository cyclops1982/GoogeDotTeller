using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogerDotTeller
{
    internal static class EmbeddedResourceHelper
    {
        public static MemoryStream GetEmbeddedResource(string embeddedFilename)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFilename, StringComparison.CurrentCultureIgnoreCase));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                MemoryStream result = new MemoryStream();
                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);
                return result;
            }

        }

    }
}
