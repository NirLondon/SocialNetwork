using Social.Common;
using Social.Common.DAL;
using Social.DAL;

namespace Social.BL
{
    public class BLDependenciesResolver : DependenciesResolver
    {
        public BLDependenciesResolver()
        : base ((typeof(ISocialRepository), typeof(SocialRepository)))
        { }
    }
}
