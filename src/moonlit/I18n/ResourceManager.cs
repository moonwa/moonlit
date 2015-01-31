namespace Moonlit.I18n
{
    public abstract class ResourceManager
    {
        private static IResourceManagerFactory _managerFactory;
        public static IResourceManagerFactory ManagerFactory
        {
            get
            {
                if (_managerFactory == null)
                {
                    _managerFactory = new DirectoryResourceManagerFactory();
                }
                return _managerFactory;
            }
            set { _managerFactory = value; }
        } 
        public static string Culture { get; set; }
        public static Resource GetResource(string resourceName)
        {
            return ManagerFactory.GetResource(resourceName);
        }
    }
}
