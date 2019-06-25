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
        public void SignIn_uYes()
        {
            ClientFunc f = new ClientFunc();
            Assert.AreEqual("srp", f.SignIn_uYes("upq¬q"));
            Assert.AreEqual("swp", f.SignIn_uYes("upq¬qq"));
        }

        [TestCase]
        public void SignIn_uNo()
        {
            ClientFunc f = new ClientFunc();
            Assert.AreEqual("srp", f.SignIn_uNo("upqqewwers¬q"));
        }

        [TestCase]
        public void SignIn_ban()
        {
            ClientFunc f = new ClientFunc();
            Assert.AreEqual("sub", f.SignIn_ban("up12¬12"));
        }

        [TestCase]
        public void OneMes()
        {
            ClientFunc f = new ClientFunc();
            Assert.AreEqual("qqq", f.OneMes("q"));
        }
    }
}
