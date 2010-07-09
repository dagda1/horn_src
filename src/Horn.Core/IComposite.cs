namespace Horn.Core
{
    public interface IComposite<T>
    {
        void Add(T item);

        void Remove(T item);

        T Parent { get; set; }

        T[] Children { get;}
    }
}