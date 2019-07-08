namespace Geev.Owin
{
    public class GeevOwinOptions
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool UseEmbeddedFiles { get; set; }

        public GeevOwinOptions()
        {
            UseEmbeddedFiles = true;
        }
    }
}