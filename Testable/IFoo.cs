namespace Testable
{
    public interface IFoo
    {
        bool DoSomething(string value);
        string DoSomethingStringy(string value);
        string DoSomethingDoubleStringy(string val1, string val2);
        bool TryParse(string value, out string outputValue);
        bool Submit(ref Bar bar);
        int GetCount();
        int GetCountThing();
        bool Add(int value);
        bool Subtract(int v);

        string Name { get; set; }
        string NickName { get; set; }
        IBar Bar { get; set; }
    }

    public interface IBar
    {
        IBaz Baz { get; set; }
    }

    public interface IBaz
    {
        string Name { get; set; }
        int Age { get; set; }
    }
}
