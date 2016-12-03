using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.Weixin.Tests
{
    public class TestBase
    {
        private MPClient _client;

        public MPClient Client
        {
            get { return _client; }
        }

        [TestInitialize]
        public void Init()
        {
            _client= new MPClient( "wxd8b64943ac261c4c", "6634394c895cce662ca469deda09c30d", "whatisthisidontthinkso");
        }
    }
}