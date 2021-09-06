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
        private List<IdentityUser> _userScoreDb = new List<IdentityUser> {
            new IdentityUser()
            {
                Id= "606416f0-8a0a-40fa-929c-e4a188649307",
                Email = "hello@gmail.com",
                EmailConfirmed = true,
                PasswordHash = "ACrsTvOWqwwCsivFAPDMlIgBClASgkCUl49yq154ksq/4xSrOt+rq37JZwDHa5fOAQ==",
                SecurityStamp = "591b8c97-43ec-46b2-88fe-f07e858a1bbf",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = "11111111",
            }
          };
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

        [TestMethod]
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
            mockContext.Setup(ctx => ctx.Set<IdentityUser>()).Returns(mset.Object);//когда доб нов запись в БД
            //var mockDbSet = mockContext.CreateDbSetMock<IdentityUser>(c =>
            //(System.Data.Entity.DbSet<IdentityUser>)c.Users, (i, _) => i.Id , _userScoreDb);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(mockContext.Object));
            var emailMock = new Mock<IIdentityMessageService>();
            emailMock.Setup(e => e.SendAsync(It.IsAny<IdentityMessage>())).
                Returns(() => Task.CompletedTask);

            // Act
            UserService userService = new UserService(mockContext.Object, userManager,
                emailMock.Object);
            var result = userService.AddUser(registerModel);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            //Assert.IsTrue(_userScoreDb.Count == 1);
        }
        [TestMethod]
        public void ResetPasswordTestSuccess()
        {
            //Arrange
            RegisterModel registerModel = new RegisterModel()
            {
                UserName = "Oleg",
                Email = "oleg@gmail.com",
                Password = "11PPpp22"
            };
            var mockContext = new DbContextMock<GameContext>("111");
            var mset = new DbSetMock<IdentityUser>(_userScoreDb, (u, _) => u.Id);

            mockContext.Setup(ctx => ctx.Users).Returns(mset.Object);

            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(mockContext.Object));
            Mock<IdentityUser> mockUserManager = new Mock<IdentityUser>(mockContext.Object);
            string email = "oleg@gmail.com";
            mockUserManager.Setup(s => s.FindByEmail(email)).Returns(new ApplicationUser
            {
                Id = "testId",
                Email = "test@email.com"
            });
            var emailMock = new Mock<IIdentityMessageService>();
            emailMock.Setup(e => e.SendAsync(It.IsAny<IdentityMessage>())).
                Returns(() => Task.CompletedTask);


            UserService userService = new UserService(mockContext.Object,
                            userManager, emailMock.Object);
            Mock<IUserService> mockUserService = new Mock<IUserService>();

            //Act


            var result = userService.ResetPassword(email);
            //Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void ResetPasswordTest()
        {
            //Arrange

            var mockContext = new DbContextMock<GameContext>("111");
            var mset = new DbSetMock<IdentityUser>(_userScoreDb, (u, _) => u.Id);
            mockContext.Setup(ctx => ctx.Users).Returns(mset.Object);
            mockContext.Setup(ctx => ctx.Set<IdentityUser>()).Returns(mset.Object);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(mockContext.Object));
            var emailService = new Mock<IIdentityMessageService>();
            emailService.Setup(obj => obj.Send(It.IsAny<IdentityMessage>()));// мок если вызван метод Send()с любым арг типа  IdentityMessage, то
            var userService = new UserService(mockContext.Object, userManager, emailService.Object);
            var userToken = userManager.GeneratePasswordResetToken("606416f0-8a0a-40fa-929c-e4a188649307");

            //Act
            var result = userService.ResetPassword("606416f0-8a0a-40fa-929c-e4a188649307",
                userToken, "11PPoo!!2");

            //Assert

            Assert.IsTrue(result.IsSuccess);
        }
    }
}
