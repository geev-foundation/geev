using Geev.Application.Features;
using Geev.Runtime.Validation;
using Geev.UI.Inputs;

namespace Geev.TestBase.SampleApplication.ContacLists
{
    public class SampleFeatureProvider : FeatureProvider
    {
        public static class Names
        {
            public const string Contacts = "ContactManager.Contacts";
            public const string MaxContactCount = "ContactManager.MaxContactCount";
            public const string ChildFeatureToOverride = "ContactManager.ChildFeatureToOverride";
            public const string ChildFeatureToDelete = "ContactManager.ChildFeatureToDelete";
        }

        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var contacts = context.Create(Names.Contacts, "false");
            contacts.CreateChildFeature(Names.MaxContactCount, "100", inputType: new SingleLineStringInputType(new NumericValueValidator(1, 10000)));

            contacts.CreateChildFeature(Names.ChildFeatureToOverride, "ChildFeature");
            contacts.RemoveChildFeature(Names.ChildFeatureToOverride);
            contacts.CreateChildFeature(Names.ChildFeatureToOverride, "ChildFeatureToOverride");

            contacts.CreateChildFeature(Names.ChildFeatureToDelete, "ChildFeatureToDelete");
            contacts.RemoveChildFeature(Names.ChildFeatureToDelete);
        }
    }
}