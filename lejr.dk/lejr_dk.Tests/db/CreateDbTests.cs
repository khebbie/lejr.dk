using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using lejr_dk.Models;
using NUnit.Framework;

namespace lejr_dk.Tests.db
{
    [TestFixture]
    [Category("Integration")]
    public class CreateDbTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEach()
        {
            var source = new XmlConfigurationSource("lejr_dk.cfg.xml");

            ActiveRecordStarter.Initialize(source, typeof (Camp));
        }

        #endregion

        [Test]
        [Ignore("only used to create the schema")]
        public void Create()
        {
            ActiveRecordStarter.CreateSchema();
        }

        [Test]
        public void CreateSimpleCamp()
        {
            var beginTime = DateTime.Now.AddDays(-20);
            var endTime = DateTime.Now.AddDays(-18);
            var displayBegin = DateTime.Now.AddDays(-40);
            var displayEnd = DateTime.Now.AddDays(-5);
            var c = new Camp(CampType.Junior, beginTime, endTime,
                             displayBegin, displayEnd,
                             "Description", "http://www.lejr.dk");
            
            Assert.DoesNotThrow(c.Create);
        }
    }
}