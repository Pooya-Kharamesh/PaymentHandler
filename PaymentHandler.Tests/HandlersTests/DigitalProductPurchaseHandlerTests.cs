namespace PaymentHandler.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Business.PaymentHandlers;
    using PaymentHandler.Entities;

    [TestClass]
    public class DigitalProductPurchaseHandlerTests
    {
        [TestMethod]
        public async Task WhenOneApplicableOneInapplicable_OnlyApplicableRuleApplied()
        {
            Mock<ISpecialRuleRepository> specialRuleRepositoryMock = new Mock<ISpecialRuleRepository>();

            Mock<ICartRule> applicableRule = new Mock<ICartRule>();
            applicableRule.SetupGet(x => x.IsApplicable).Returns((Order) => true);
            applicableRule.SetupGet(x => x.Apply).Returns((o) => { });

            Mock<ICartRule> inapplicableRule = new Mock<ICartRule>();
            inapplicableRule.SetupGet(x => x.IsApplicable).Returns((Order) => false);
            inapplicableRule.SetupGet(x => x.Apply).Returns((o) => { });

            specialRuleRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(new[] { applicableRule.Object, inapplicableRule.Object });

            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 1" }, 4);
            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 2" }, 2);

            var sut = new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object);

            await sut.ProcessAsync(order);
            applicableRule.VerifyGet(x => x.IsApplicable, Times.Once);
            applicableRule.VerifyGet(x => x.Apply, Times.Once);

            inapplicableRule.VerifyGet(x => x.IsApplicable, Times.Once);
            inapplicableRule.VerifyGet<Action<Order>>(x => x.Apply, Times.Never);
        }

        [TestMethod]
        public void WhenRuleSetRepoIsNull_NoRuleExecuted()
        {
            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 1" }, 4);

            Assert.ThrowsException<ArgumentNullException>(() => new DigitalProductPurchaseHandler(null));
        }

        [TestMethod]
        public async Task WhenMultipleApplicable_AllApplicableRuleApplied()
        {
            Mock<ISpecialRuleRepository> specialRuleRepositoryMock = new Mock<ISpecialRuleRepository>();
            Mock<ICartRule> rule1 = new Mock<ICartRule>();

            rule1.SetupGet(x => x.IsApplicable).Returns((Order) => true);
            rule1.SetupGet(x => x.Apply).Returns((Order) => { });

            Mock<ICartRule> rule2 = new Mock<ICartRule>();

            rule2.SetupGet(x => x.IsApplicable).Returns((Order) => true);
            rule2.SetupGet(x => x.Apply).Returns((Order) => { });

            specialRuleRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(new[] { rule1.Object, rule2.Object });

            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 1" }, 4);
            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 2" }, 2);

            var sut = new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object);

            await sut.ProcessAsync(order);
            rule1.VerifyGet(x => x.IsApplicable, Times.Once);
            rule1.VerifyGet(x => x.Apply, Times.Once);

            rule2.VerifyGet(x => x.IsApplicable, Times.Once);
            rule2.VerifyGet(x => x.Apply, Times.Once);
        }

        [TestMethod]
        public async Task WhenRuleSetIsNull_NoRuleIsApplied()
        {
            Mock<ISpecialRuleRepository> specialRuleRepositoryMock = new Mock<ISpecialRuleRepository>();

            specialRuleRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(() => null);

            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 1" }, 4);
            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 2" }, 2);

            var sut = new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object);

            await sut.ProcessAsync(order);
            specialRuleRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            // check if exercised again wont call to GetAll
            await sut.ProcessAsync(order);
            specialRuleRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
