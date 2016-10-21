using System;
using System.Linq;
using DALLib.EF;
using DALLib.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace DALLib.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public void TestIfUserAddWorks()
        {
            var mockContext = new Mock<MsSqlMonitorEntities>();
            var mockSet = new Mock<DbSet<User>>();
            mockContext.Setup(g => g.Users).Returns(mockSet.Object);

            using (var unitOfWork = new UnitOfWork(mockContext.Object))
            {
                User user = new User
                {
                    Login = "Login12",
                    Password = "Pass",
                    Role = UserRole.User
                };

                unitOfWork.Users.Create(user);
                unitOfWork.Save();
            }
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void TestIfUserGetWorks()
        {
            var users = new List<User>
            {
                new User { Id = 1, Login = "Login1", Password = "Password1" },
                new User { Id = 2, Login = "Login2", Password = "Password2" },
                new User { Id = 3, Login = "Login3", Password = "Password3" }
            }.AsQueryable();

            var mockContext = new Mock<MsSqlMonitorEntities>();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var unitOfWork = new UnitOfWork(mockContext.Object);

            var getUser = unitOfWork.Users.Get(1);
            Assert.AreEqual(getUser.Login, "Login1");
        }
    }
}
