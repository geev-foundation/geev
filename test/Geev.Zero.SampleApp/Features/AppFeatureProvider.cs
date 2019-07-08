using Geev.Application.Features;
using Geev.Dependency;
using Geev.UI.Inputs;

namespace Geev.Zero.SampleApp.Features
{
    public class AppFeatureProvider : FeatureProvider
    {
        public const string MyBoolFeature = "MyBoolFeature";
        public const string MyNumericFeature = "MyNumericFeature";

        private readonly IIocResolver _iocResolver; //Just for injection testing

        public AppFeatureProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var boolFeature = context.Create(MyBoolFeature, "false", inputType: new CheckboxInputType());
            var numericFrature = boolFeature.CreateChildFeature(MyNumericFeature, "42");
        }
    }
}
