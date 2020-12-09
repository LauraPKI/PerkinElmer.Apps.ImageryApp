using System.Linq;
using System.Reflection;
using PerkinElmer.Signals.Analytics.AppCommon;
using PerkinElmer.Signals.Analytics.AppCommon.AppRegistry;
using Spotfire.Dxp.Application.Extension;
using SpotfireAddin = Spotfire.Dxp.Application.Extension.AddIn;

namespace PerkinElmer.Apps.ImageryApp
{
    public class AddIn : SpotfireAddin
    {
        protected override void OnGlobalServicesRegistered(ServiceProvider serviceProvider)
        {
            base.OnGlobalServicesRegistered(serviceProvider);

            string resourcePath = Assembly.GetExecutingAssembly().GetName().Name + ".app.json";
            AppStoreStartAppData appMetaData = ResourceLoaderUtility.LoadAppStoreDataResource(Assembly.GetExecutingAssembly(), resourcePath);

            var appRegistry = serviceProvider.GetService<AppRegistryService>();

            var appMetadata = System.Attribute
                .GetCustomAttributes(typeof(App))
                .SingleOrDefault(a => a is AppMetadata) as AppMetadata;

            appRegistry.Register(appMetadata.Name, typeof(App), appMetaData);
        }
    }
}
