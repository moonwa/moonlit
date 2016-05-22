namespace Moonlit
{
    public class MoonlitDependencyResolver
    {
        private static MoonlitDependencyResolver _instance = new MoonlitDependencyResolver();
        /// <summary>
        /// Gets the implementation of the dependency resolver.
        /// </summary>
        /// 
        /// <returns>
        /// The implementation of the dependency resolver.
        /// </returns>
        public static IDependencyResolver Current
        {
            get
            {
                return MoonlitDependencyResolver._instance.InnerCurrent;
            }
        }
        /// <summary>
        /// Provides a registration point for dependency resolvers, using the specified dependency resolver interface.
        /// </summary>
        /// <param name="resolver">The dependency resolver.</param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            MoonlitDependencyResolver._instance.InnerSetResolver(resolver);
        }

        private void InnerSetResolver(IDependencyResolver resolver)
        {
            InnerCurrent = resolver;
        }

        public IDependencyResolver InnerCurrent { get; private set; }
    }
}