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
    public class MembershipPurchaseHandlerTests
    {
        private Mock<IMembershipRepository> membershipRepositoryMock;
        private Mock<IEmailClient> emailClientMock;

        [TestInitialize]
        public void Initialize()
        {
            this.membershipRepositoryMock = new Mock<IMembershipRepository>();
            this.emailClientMock = new Mock<IEmailClient>();

            this.membershipRepositoryMock.Setup(x => x.ActivateMembership(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            this.emailClientMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
        }

        [TestMethod]
        public async Task WhenOrderIsActivateOneUpgradeOne_OneMemberShipIsActivatedOneIsUpgraded()
        {
            var order = new Order()
            {
                OrderType = OrderType.MembershipPurchase
            };

            var activateMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipActivation, DefaultUnitPrice = 5, MembershipEmail = "X@d.com", MembershipId = Guid.NewGuid() };
            var upgradeMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipUpgrade, DefaultUnitPrice = 5, MembershipEmail = "X@d.com", MembershipId = Guid.NewGuid(), MembershipClass = MembershipClass.Golden };
            order.Add(activateMembershipItem, 1);
            order.Add(upgradeMembershipItem, 1);

            var sut = new MembershipPurchaseHandler(membershipRepositoryMock.Object, this.emailClientMock.Object);

            await sut.ProcessAsync(order);
            this.membershipRepositoryMock.Verify(x => x.ActivateMembership(activateMembershipItem.MembershipId), Times.Once);
            this.membershipRepositoryMock.Verify(x => x.UpgradeMembership(upgradeMembershipItem.MembershipId, upgradeMembershipItem.MembershipClass), Times.Once);
        }

        [TestMethod]
        public async Task WhenOrderMembershipIdIsMissing_Throws()
        {
            var order = new Order()
            {
                OrderType = OrderType.MembershipPurchase
            };

            var activateMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipActivation, DefaultUnitPrice = 5, MembershipEmail = "X@d.com", MembershipId = Guid.Empty };
            order.Add(activateMembershipItem, 1);

            var sut = new MembershipPurchaseHandler(membershipRepositoryMock.Object, this.emailClientMock.Object);

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await sut.ProcessAsync(order));
        }

        [TestMethod]
        public async Task WhenOrderMembershipEmailIsMissing_Throws()
        {
            var order = new Order()
            {
                OrderType = OrderType.MembershipPurchase
            };

            var activateMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipActivation, DefaultUnitPrice = 5, MembershipEmail = string.Empty, MembershipId = Guid.NewGuid() };
            order.Add(activateMembershipItem, 1);

            var sut = new MembershipPurchaseHandler(membershipRepositoryMock.Object, this.emailClientMock.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut.ProcessAsync(order));
        }

        [TestMethod]
        public async Task WhenOrderIsActivateMembership_MemberShipIsActivated()
        {
            var order = new Order()
            {
                OrderType = OrderType.MembershipPurchase
            };

            var activateMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipActivation, DefaultUnitPrice = 5, MembershipEmail = "X@d.com", MembershipId = Guid.NewGuid() };
            var upgradeMembershipItem = new MembershipItem() { Category = ItemCategory.Membership, Name = Constants.ItemNames.MembershipUpgrade, DefaultUnitPrice = 5, MembershipEmail = "X@d.com", MembershipId = Guid.NewGuid(), MembershipClass = MembershipClass.Golden };
            order.Add(activateMembershipItem, 1);
            order.Add(upgradeMembershipItem, 1);

            var sut = new MembershipPurchaseHandler(membershipRepositoryMock.Object, this.emailClientMock.Object);

            await sut.ProcessAsync(order);
            this.membershipRepositoryMock.Verify(x => x.ActivateMembership(activateMembershipItem.MembershipId), Times.Once);
            this.membershipRepositoryMock.Verify(x => x.UpgradeMembership(upgradeMembershipItem.MembershipId, upgradeMembershipItem.MembershipClass), Times.Once);
        }

        // TODO : more tests like 
        // e.g Membership is not downgraded

    }
}
