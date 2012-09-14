using System.Web.Mvc;
using lejr_dk.Controllers;
using NUnit.Framework;

namespace lejr_dk.Tests.Controllers
{
    [TestFixture]
    [Category("Unit")]
    public class HomeControllerTest
    {
        [Test]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}