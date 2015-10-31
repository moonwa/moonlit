using Moonlit.PatternDesign;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Moonlit.TestFixtures.PatternDesign
{
    
    
    /// <summary>
    ///This is a test class for Observable_Test and is intended
    ///to contain all Observable_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class ObservableTests
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

    

        internal virtual Observable<TArg> CreateObservable<TArg>()
        {
            // TODO: Instantiate an appropriate concrete class.
            Observable<TArg> target = null;
            return target;
        }

        [TestMethod()]
        public void Notify_Test()
        {
            Observable<bool> observable = new Observable<bool>();
            using(ObserverTestClass observer1 = new ObserverTestClass(observable))
            using(ObserverTestClass observer2 = new ObserverTestClass(observable))
            using (ObserverTestClass observer3 = new ObserverTestClass(observable))
            {
                Assert.AreEqual(false, observer1.Value);
                Assert.AreEqual(false, observer2.Value);
                Assert.AreEqual(false, observer3.Value);

                observable.Notify(false);
                Assert.AreEqual(false, observer1.Value);
                Assert.AreEqual(false, observer2.Value);
                Assert.AreEqual(false, observer3.Value);

                observable.Notify(true);
                Assert.AreEqual(true, observer1.Value);
                Assert.AreEqual(true, observer2.Value);
                Assert.AreEqual(true, observer3.Value);
            }
        }
        #region ObserverTestClass
        class ObserverTestClass : Moonlit.PatternDesign.IObserver<bool>, System.IDisposable
        {
            public bool Value { get; private set; }
            private Observable<bool> _observable;
            public ObserverTestClass(Observable<bool> observable)
            {
                this.Value = false;
                observable.Register(this);
                this._observable = observable;
            }
            #region IObserver<bool> Members

            public void Update(bool arg)
            {
                this.Value = arg;
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                this.Dispose(true);
            }

            private void Dispose(bool disposing)
            {
                if (this._observable != null)
                {
                    this._observable.Unregister(this);
                }
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
            }

            #endregion
            ~ObserverTestClass()
            {
                this.Dispose(false);
            }
        }
        #endregion
    }
}
