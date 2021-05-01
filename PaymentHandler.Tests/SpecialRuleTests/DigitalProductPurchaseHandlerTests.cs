namespace PaymentHandler.Tests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Business.PaymentHandlers;
    using PaymentHandler.Business.SpecialRules;
    using PaymentHandler.Entities;

    [TestClass]
    public class AutoAddFirstAidRuleTest
    {
        private Mock<ISpecialRuleRepository> specialRuleRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            this.specialRuleRepositoryMock = new Mock<ISpecialRuleRepository>();
            this.specialRuleRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(new[] { new AutoAddFirstAid() });
        }

        [TestMethod]
        public async Task WhenLearnToSkiIsOrdered_FirstAidIsAdded()
        {
            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = Constants.ItemNames.LearnToSkiVideoName }, 4);

            await (new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object)).ProcessAsync(order);

            Assert.AreEqual(2, order.CartItems.Count);
            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.LearnToSkiVideoName, out var learnToSkiVideoItem));
            Assert.AreEqual(4, learnToSkiVideoItem.Quantity);

            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.FirstAidVideoName, out var firstAidVideoItem));
            Assert.AreEqual(1, firstAidVideoItem.Quantity);
            Assert.AreEqual(0, firstAidVideoItem.LineTotal);
        }

        [TestMethod]
        public async Task WhenLearnToSkiIsOrderedNotInCart_FirstAidIsNotAdded()
        {
            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = "Random Video" }, 4);

            await (new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object)).ProcessAsync(order);

            Assert.AreEqual(1, order.CartItems.Count);
            Assert.IsTrue(order.CartItems.TryGetValue("Random Video", out var learnToSkiVideoItem));
            Assert.AreEqual(4, learnToSkiVideoItem.Quantity);
        }

        [TestMethod]
        public async Task WhenFirstAidIsInCartThenLearnSkiIsAdded_FirstAidIsFree()
        {
            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = Constants.ItemNames.FirstAidVideoName, DefaultUnitPrice = 5 }, 4);
            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.FirstAidVideoName, out var firstAidVideoName));
            Assert.AreEqual(20, firstAidVideoName.LineTotal);
            order.Add(new Item() { Category = ItemCategory.Video, Name = Constants.ItemNames.LearnToSkiVideoName }, 4);

            await (new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object)).ProcessAsync(order);

            Assert.AreEqual(2, order.CartItems.Count);
            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.LearnToSkiVideoName, out var learnToSkiVideoItem));
            Assert.AreEqual(4, learnToSkiVideoItem.Quantity);

            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.FirstAidVideoName, out var firstAidVideoItem));
            Assert.AreEqual(1, firstAidVideoItem.Quantity);
            Assert.AreEqual(0, firstAidVideoItem.LineTotal);
        }

        [TestMethod]
        public async Task WhenFirstAidIsAddedWithoutLearnSkiIsAdded_FirstAidIsCharged()
        {
            var order = new Order()
            {
                OrderType = OrderType.DigitalProductPurchase
            };

            order.Add(new Item() { Category = ItemCategory.Video, Name = Constants.ItemNames.FirstAidVideoName, DefaultUnitPrice = 5 }, 1);
            Assert.IsTrue(order.CartItems.TryGetValue(Constants.ItemNames.FirstAidVideoName, out var firstAidVideoName));
            Assert.AreEqual(5, firstAidVideoName.LineTotal);

            await (new DigitalProductPurchaseHandler(specialRuleRepositoryMock.Object)).ProcessAsync(order);

            Assert.AreEqual(5, firstAidVideoName.LineTotal);
        }
    }
}
