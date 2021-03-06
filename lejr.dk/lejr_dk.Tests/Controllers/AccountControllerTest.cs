﻿using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using lejr_dk.Controllers;
using lejr_dk.Models;
using NUnit.Framework;

namespace lejr_dk.Tests.Controllers
{
    [TestFixture]
    [Category("Unit")]
    public class AccountControllerTest
    {
        private static AccountController GetAccountController()
        {
            var controller = new AccountController
                                 {
                                     FormsService = new MockFormsAuthenticationService(),
                                     MembershipService = new MockMembershipService()
                                 };
            controller.ControllerContext = new ControllerContext
                                               {
                                                   Controller = controller,
                                                   RequestContext =
                                                       new RequestContext(new MockHttpContext(), new RouteData())
                                               };
            return controller;
        }

        private class MockFormsAuthenticationService : IFormsAuthenticationService
        {
            public bool SignIn_WasCalled;
            public bool SignOut_WasCalled;

            #region IFormsAuthenticationService Members

            public void SignIn(string userName, bool createPersistentCookie)
            {
                // verify that the arguments are what we expected
                Assert.AreEqual("someUser", userName);
                Assert.IsFalse(createPersistentCookie);

                SignIn_WasCalled = true;
            }

            public void SignOut()
            {
                SignOut_WasCalled = true;
            }

            #endregion
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get { return _user; }
                set { base.User = value; }
            }
        }

        private class MockMembershipService : IMembershipService
        {
            #region IMembershipService Members

            public int MinPasswordLength
            {
                get { return 10; }
            }

            public bool ValidateUser(string userName, string password)
            {
                return (userName == "someUser" && password == "goodPassword");
            }

            public MembershipCreateStatus CreateUser(string userName, string password, string email)
            {
                if (userName == "duplicateUser")
                {
                    return MembershipCreateStatus.DuplicateUserName;
                }

                // verify that values are what we expected
                Assert.AreEqual("goodPassword", password);
                Assert.AreEqual("goodEmail", email);

                return MembershipCreateStatus.Success;
            }

            public bool ChangePassword(string userName, string oldPassword, string newPassword)
            {
                return (userName == "someUser" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword");
            }

            #endregion
        }

        [Test]
        public void ChangePassword_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePassword();

            // Assert
            Assert.AreEqual(10, ((ViewResult) result).ViewData["PasswordLength"]);
        }

        [Test]
        public void ChangePassword_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new ChangePasswordModel
                            {
                                OldPassword = "goodOldPassword",
                                NewPassword = "goodNewPassword",
                                ConfirmPassword = "goodNewPassword"
                            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            var redirectResult = (RedirectToRouteResult) result;
            Assert.AreEqual("ChangePasswordSuccess", redirectResult.RouteValues["action"]);
        }

        [Test]
        public void ChangePassword_Post_ReturnsViewIfChangePasswordFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new ChangePasswordModel
                            {
                                OldPassword = "goodOldPassword",
                                NewPassword = "badNewPassword",
                                ConfirmPassword = "badNewPassword"
                            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("The current password is incorrect or the new password is invalid.",
                            controller.ModelState[""].Errors[0].ErrorMessage);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [Test]
        public void ChangePassword_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new ChangePasswordModel
                            {
                                OldPassword = "goodOldPassword",
                                NewPassword = "goodNewPassword",
                                ConfirmPassword = "goodNewPassword"
                            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [Test]
        public void LogOff_LogsOutAndRedirects()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOff();

            // Assert
            var redirectResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService) controller.FormsService).SignOut_WasCalled);
        }

        [Test]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new LogOnModel
                            {
                                UserName = "someUser",
                                Password = "goodPassword",
                                RememberMe = false
                            };

            // Act
            ActionResult result = controller.LogOn(model, "/someUrl");

            // Assert
            var redirectResult = (RedirectResult) result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            Assert.IsTrue(((MockFormsAuthenticationService) controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithoutReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new LogOnModel
                            {
                                UserName = "someUser",
                                Password = "goodPassword",
                                RememberMe = false
                            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            var redirectResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService) controller.FormsService).SignIn_WasCalled);
        }

        [Test]
        public void LogOn_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new LogOnModel
                            {
                                UserName = "someUser",
                                Password = "goodPassword",
                                RememberMe = false
                            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
        }

        [Test]
        public void LogOn_Post_ReturnsViewIfValidateUserFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new LogOnModel
                            {
                                UserName = "someUser",
                                Password = "badPassword",
                                RememberMe = false
                            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("The user name or password provided is incorrect.",
                            controller.ModelState[""].Errors[0].ErrorMessage);
        }

        [Test]
        public void Register_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Register();

            // Assert
            Assert.AreEqual(10, ((ViewResult) result).ViewData["PasswordLength"]);
        }

        [Test]
        public void Register_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new RegisterModel
                            {
                                UserName = "someUser",
                                Email = "goodEmail",
                                Password = "goodPassword",
                                ConfirmPassword = "goodPassword"
                            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            var redirectResult = (RedirectToRouteResult) result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }

        [Test]
        public void Register_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new RegisterModel
                            {
                                UserName = "someUser",
                                Email = "goodEmail",
                                Password = "goodPassword",
                                ConfirmPassword = "goodPassword"
                            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [Test]
        public void Register_Post_ReturnsViewIfRegistrationFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            var model = new RegisterModel
                            {
                                UserName = "duplicateUser",
                                Email = "goodEmail",
                                Password = "goodPassword",
                                ConfirmPassword = "goodPassword"
                            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            var viewResult = (ViewResult) result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("Username already exists. Please enter a different user name.",
                            controller.ModelState[""].Errors[0].ErrorMessage);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }
    }
}