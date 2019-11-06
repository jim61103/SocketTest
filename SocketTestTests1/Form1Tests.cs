using NUnit.Framework;
using SocketTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketTest.Tests
{
    [TestFixture()]
    public class Form1Tests
    {
        [Test()]
        [TestCase("123")]
        public void ClientSendMsgTest(string test)
        {
           
            Form1 ex = new Form1();

            ex.ClientSendMsg(test);

            Assert.Fail();
        }
    }
}