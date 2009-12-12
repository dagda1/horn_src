using System;

public abstract class TestBase : IDisposable
{

    protected abstract void Because();



    public void Dispose()
    {
        After_each_spec();
    }



    protected TestBase()
    {
        Before_each_spec();
        Because();
    }

    protected virtual void Before_each_spec() { }

    protected virtual void After_each_spec() { }



}