using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossNull.Logic.Services;
using EntityFrameworkMock;
using CrossNull.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Linq;
using Moq;
using System.Threading.Tasks;

namespace ServiceTest
{
    /// <summary>
    /// Summary description for UserServiceTest
    /// </summary>
    [TestClass]
    public class UserServiceTest
    {
        public UserServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private List<IdentityUser> _userScoreDb = new List<IdentityUser> { };
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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

       // [TestMethod]
        public void TestUserServiceAddCorrect()
        {
            // Arrange
            RegisterModel registerModel = new RegisterModel()
            {
                UserName = "Oleg",
                Email = "oleg@gmail.com",
                Password = "11PPpp22"
            };
            var mockContext = new DbContextMock<GameContext>("111");
            var mset = new DbSetMock<IdentityUser>(_userScoreDb, (u, _) => u.Id);
            mockContext.Setup(ctx => ctx.Users).Returns(mset.Object);
            mockContext.Setup(ctx => ctx.Set<IdentityUser>()).Returns(mset.Object);
            //var mockDbSet = mockContext.CreateDbSetMock<IdentityUser>(c =>
            //(System.Data.Entity.DbSet<IdentityUser>)c.Users, (i, _) => i.Id , _userScoreDb);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(mockContext.Object));
            var emailMock = new Mock<IIdentityMessageService>();
            emailMock.Setup(e => e.SendAsync(It.IsAny<IdentityMessage>())).
                Returns(()=> Task.CompletedTask);

            // Act
            UserService userService = new UserService(mockContext.Object, userManager,
                emailMock.Object);
            var result = userService.AddUser(registerModel);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(_userScoreDb.Count == 1);
        }
        [TestMethod]
        public void ResetPasswordTestSuccess()
        {
            //Arrange
            var mockContext = new DbContextMock<GameContext>("111");
            var mset = new DbSetMock<IdentityUser>(_userScoreDb, (u, _) => u.Id);
            mockContext.Setup(ctx => ctx.Users).Returns(mset.Object);
            mockContext.Setup(ctx => ctx.Set<IdentityUser>()).Returns(mset.Object);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(mockContext.Object));
            var emailMock = new Mock<IIdentityMessageService>();

            emailMock.Setup(e => e.SendAsync(It.IsAny<IdentityMessage>())).
                Returns(() => Task.CompletedTask);
            RegisterModel registerModel = new RegisterModel()
            {
                UserName = "Oleg",
                Email = "oleg@gmail.com",
                Password = "11PPpp22"
            };

            string email = "oleg@gmail.com";

            //Act

            UserService userService = new UserService(mockContext.Object,
                userManager, emailMock.Object);
            var result = userService.ResetPassword(email);
            //Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
