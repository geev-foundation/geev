namespace Geev.FluentValidation.Configuration
{
    public interface IGeevFluentValidationConfiguration
    {
        /// <summary>
        /// Name of the source name to get translations from.
        /// </summary>
        string LocalizationSourceName { get; set; }
    }
}
