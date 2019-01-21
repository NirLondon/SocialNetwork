using Social.Common;
using Social.Common.BL;
using Social.Common.DAL;
using Social.DAL;

namespace Social.BL
{
    public class BLDependenciesResolver : DependenciesResolver
    {
        public BLDependenciesResolver()
        : base((typeof(ISocialRepository), typeof(Neo4jSocialRepository)),
              (typeof(ISocialManager), typeof(SocialManager)),
              (typeof(IPhotosStorage), typeof(PhotoStorage)))
        { }
    }
}
