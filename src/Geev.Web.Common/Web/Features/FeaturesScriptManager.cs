using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geev.Application.Features;
using Geev.Dependency;
using Geev.Runtime.Session;
using Geev.Web.Http;

namespace Geev.Web.Features
{
    public class FeaturesScriptManager : IFeaturesScriptManager, ITransientDependency
    {
        public IGeevSession GeevSession { get; set; }

        private readonly IFeatureManager _featureManager;
        private readonly IFeatureChecker _featureChecker;

        public FeaturesScriptManager(IFeatureManager featureManager, IFeatureChecker featureChecker)
        {
            _featureManager = featureManager;
            _featureChecker = featureChecker;

            GeevSession = NullGeevSession.Instance;
        }

        public async Task<string> GetScriptAsync()
        {
            var allFeatures = _featureManager.GetAll().ToList();
            var currentValues = new Dictionary<string, string>();

            if (GeevSession.TenantId.HasValue)
            {
                var currentTenantId = GeevSession.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    currentValues[feature.Name] = await _featureChecker.GetValueAsync(currentTenantId, feature.Name);
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    currentValues[feature.Name] = feature.DefaultValue;
                }
            }

            var script = new StringBuilder();

            script.AppendLine("(function() {");

            script.AppendLine();

            script.AppendLine("    geev.features = geev.features || {};");

            script.AppendLine();

            script.AppendLine("    geev.features.allFeatures = {");

            for (var i = 0; i < allFeatures.Count; i++)
            {
                var feature = allFeatures[i];
                script.AppendLine("        '" + HttpEncode.JavaScriptStringEncode(feature.Name) + "': {");
                script.AppendLine("             value: '" + HttpEncode.JavaScriptStringEncode(currentValues[feature.Name]) + "'");
                script.Append("        }");

                if (i < allFeatures.Count - 1)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }
            }

            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}