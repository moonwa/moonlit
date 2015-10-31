using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Arithmetic
{
    [TestClass]
    public class QuickSortTests : SortTest<Moonlit.Arithmetic.Sort.QuickSort>
    {

        protected override Moonlit.Arithmetic.Sort.QuickSort CreateT()
        {
            return new Moonlit.Arithmetic.Sort.QuickSort();
        }
    }
}