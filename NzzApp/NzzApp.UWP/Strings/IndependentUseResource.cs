using Windows.ApplicationModel.Resources;

namespace NzzApp.UWP.Strings
{
    public class IndependentUseResource
    {
        private static readonly ResourceLoader IndependentResourceLoader = ResourceLoader.GetForViewIndependentUse();

        public static string LoadString(string resource)
        {
            return IndependentResourceLoader.GetString(resource);
        }
    }
}
