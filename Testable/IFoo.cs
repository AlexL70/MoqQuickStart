using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
