using Geev.Application.Editions;
using Geev.Application.Features;
using Geev.Domain.Repositories;

namespace Geev.Zero.SampleApp.Editions
{
    public class EditionManager : GeevEditionManager
    {
        public EditionManager(
            IRepository<Edition> editionRepository,
            IGeevZeroFeatureValueStore featureValueStore)
            : base(
               editionRepository,
               featureValueStore)
        {
        }
    }
}