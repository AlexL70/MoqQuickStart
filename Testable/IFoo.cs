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
    }
}
