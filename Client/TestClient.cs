using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;

namespace Client
{
    [TestFixture]
    class TestClient
    {
        

        [TestCase]
        public void SignIn()
        {
            ClientFunc f = new ClientFunc();
            Assert.AreEqual("srp", f.SignIn("upq¬q"));            Assert.AreEqual("swp", f.SignIn("upq¬qq"));
            Assert.AreEqual("srp", f.SignIn("upqws¬q"));
            Assert.AreEqual("sub", f.SignIn("up12¬12"));
        }


    }
}
