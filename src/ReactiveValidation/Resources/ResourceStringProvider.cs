using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace ReactiveValidation
{
    /// <summary>
    /// String provider which get strings from <see cref="ResourceManager" />.
    /// </summary>
    public class ResourceStringProvider : IStringProvider
    {
        private readonly ResourceManager _mainResourceManager;
        private readonly IDictionary<string, ResourceManager> _secondaryResourceManagers;

        /// <summary>
        /// Create new instance of resource string provider with one main resource manager.
        /// </summary>
        /// <param name="mainResourceManager">Main resource manager.</param>
        public ResourceStringProvider(ResourceManager mainResourceManager)
        {
            _mainResourceManager = mainResourceManager;
        }

        /// <summary>
        /// Create new instance of resource string provider with one main resource manager and many secondaries managers.
        /// </summary>
        /// <param name="mainResourceManager">Main resource manager.</param>
        /// <param name="secondaryResourceManagers">
        /// Secondary resource managers.
        /// Key is name of resource manager.
        /// Value is resource manager.
        /// </param>
        public ResourceStringProvider(ResourceManager mainResourceManager, IDictionary<string, ResourceManager> secondaryResourceManagers)
        {
            _mainResourceManager = mainResourceManager;
            _secondaryResourceManagers = secondaryResourceManagers;
        }


        /// <inheritdoc />
        public string GetString(string key, CultureInfo culture)
        {
            return _mainResourceManager.GetString(key, culture);
        }

        /// <inheritdoc />
        public string GetString(string resource, string key, CultureInfo culture)
        {
            if (!_secondaryResourceManagers.TryGetValue(resource, out var resourceManager))
                return null;

            return resourceManager.GetString(key, culture);
        }
    }
}
