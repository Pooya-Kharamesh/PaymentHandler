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
    public class PhysicalProductPurchaseHandlerTests
    {
        private Mock<IPackageSlipPrinter> packageSlipPrinterMock;
        private Mock<ICommissionHandler> commissionHandlerMock;

        [TestInitialize]
        public void Initialize()
        {
            this.packageSlipPrinterMock = new Mock<IPackageSlipPrinter>();
            this.commissionHandlerMock = new Mock<ICommissionHandler>();
        }

        [TestMethod]
        public async Task WhenOrderIsPhysicalProducts_PackagingSlipIsSentAsync()
        {
            this.packageSlipPrinterMock.Setup(x => x.Print(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            this.commissionHandlerMock.Setup(x => x.Send(It.IsAny<Guid>(), It.IsAny<Item>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var order = new Order()
            {
                Customer = new Customer() { CustomerInfo = "Customer name and address", Address = "Test Street 1" },
                OrderType = OrderType.PhysicalProductPurchase,
            };

            var expectedAgentId = Guid.NewGuid();
            order.Add(new PhysicalItem() { Category = ItemCategory.GenericItem, Name = "Product 1" }, 1);
            order.Add(new PhysicalItem() { Category = ItemCategory.GenericItem, Name = "Product 2", AgentId = expectedAgentId }, 1);
            order.Add(new Item() { Category = ItemCategory.Video, Name = "Video 2" }, 1);

            var sut = new PhysicalProductPurchaseHandler(this.packageSlipPrinterMock.Object, this.commissionHandlerMock.Object);

            await sut.ProcessAsync(order);

            this.packageSlipPrinterMock.Verify(x => x.Print(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.packageSlipPrinterMock.Verify(x => x.Print(order.Customer.Address, It.IsAny<string>()), Times.Once);

            this.commissionHandlerMock.Verify(x => x.Send(It.IsAny<Guid>(), It.IsAny<Item>(), It.IsAny<int>()), Times.Once);
            this.commissionHandlerMock.Verify(x => x.Send(expectedAgentId, It.Is<PhysicalItem>(b => b.Name == "Product 2"), 1));
        }

        [TestMethod]
        public async Task WhenOrderContainsBook_DuplicatePackagingSlipIsSentAsync()
        {
            this.packageSlipPrinterMock.Setup(x => x.Print(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            this.commissionHandlerMock.Setup(x => x.Send(It.IsAny<Guid>(), It.IsAny<Item>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var order = new Order()
            {
                Customer = new Customer() { CustomerInfo = "Customer name and address", Address = "Test Street 1" },
                OrderType = OrderType.PhysicalProductPurchase,
            };

            var expectedAgentId = Guid.NewGuid();
            order.Add(new PhysicalItem() { Category = ItemCategory.Books, Name = "Book 1" }, 1);
            order.Add(new PhysicalItem() { Category = ItemCategory.Books, Name = "Book 2", AgentId = expectedAgentId }, 1);

            var sut = new PhysicalProductPurchaseHandler(this.packageSlipPrinterMock.Object, this.commissionHandlerMock.Object);

            await sut.ProcessAsync(order);


            this.packageSlipPrinterMock.Verify(x => x.Print(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));

            this.packageSlipPrinterMock.Verify(x => x.Print(Constants.RoyaltyDepartmentAddress, It.IsAny<string>()), Times.Once);
            this.packageSlipPrinterMock.Verify(x => x.Print(order.Customer.Address, It.IsAny<string>()), Times.Once);

            this.commissionHandlerMock.Verify(x => x.Send(It.IsAny<Guid>(), It.IsAny<Item>(), It.IsAny<int>()), Times.Once);
            this.commissionHandlerMock.Verify(x => x.Send(expectedAgentId, It.Is<PhysicalItem>(b => b.Name == "Book 2"), 1));
        }
    }
}
