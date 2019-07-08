using Geev.Application.Features;
using Geev.UI.Inputs;

using static Geev.ZeroCore.SampleApp.Application.AppLocalizationHelper;

namespace Geev.ZeroCore.SampleApp.Application
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(
                AppFeatures.SimpleBooleanFeature,
                defaultValue: "false",
                displayName: L("SimpleBooleanFeature"),
                inputType: new CheckboxInputType()
            );

            context.Create(
                AppFeatures.SimpleIntFeature,
                defaultValue: "0",
                displayName: L("SimpleIntFeature")
            );
        }
    }
}
