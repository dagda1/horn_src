
public static class IoC
{

    private static IDependencyResolver dependencyResolver;


    public static void InitializeWith(IDependencyResolver resolver)
    {
        dependencyResolver = resolver;
    }

    public static T Resolve<T>()
    {
        return dependencyResolver.Resolve<T>();
    }

    public static T Resolve<T>(string key)
    {
        return dependencyResolver.Resolve<T>(key);
    }



}

public interface IDependencyResolver
{
    T Resolve<T>();

    T Resolve<T>(string key);
}