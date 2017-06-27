<Query Kind="Statements">
  <Connection>
    <ID>441db28a-482d-4e56-b34b-df5e58c8a78f</ID>
    <Persist>true</Persist>
    <Server>PAULKOLOZSV78C6\MSSQLSERVER2008</Server>
    <Database>StockTaker</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

OrderItems.DeleteAllOnSubmit(OrderItems);
Orders.DeleteAllOnSubmit(Orders);
Products.DeleteAllOnSubmit(Products);

Order o1 = new Order()
{
	OrderId = Guid.NewGuid(),
	OrderNumber = "123",
	DateCreated = DateTime.Now
};
Orders.InsertOnSubmit(o1);
Product p1 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0001", Name = "Smarties", Description = "Disc sweets", InStock = true, DateCreated = DateTime.Now};
Product p2 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0002", Name = "Astros", Description = "Round choocolate sweets", InStock = false,DateCreated = DateTime.Now};
Product p3 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0003", Name = "Lunch Bar", Description = "Chocolate bar", InStock = false, DateCreated = DateTime.Now};
Product p4 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0004", Name = "Bar One", Description = "for a 24 hour day", InStock = false, DateCreated = DateTime.Now};
Product p5 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0005", Name = "Ghost Pops", Description = "awesome chips", InStock = true, DateCreated = DateTime.Now};
Product p6 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0006", Name = "Doritos", Description = "Weat chips", InStock = false, DateCreated = DateTime.Now};
Product p7 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0007", Name = "Fritos", Description = "Smaller weat chips", InStock = false, DateCreated = DateTime.Now};
Product p8 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0008", Name = "Magnum", Description = "Expensive Ice Cream", InStock = true, DateCreated = DateTime.Now};
Product p9 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0009", Name = "Corneto", Description = "Cone Ice Cream", InStock = false, DateCreated = DateTime.Now};
Product p10 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0010", Name = "Coca Cola", Description = "Caffeine drink", InStock = false, DateCreated = DateTime.Now};
Product p11 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0011", Name = "Fanta Orange", Description = "Orange drink", InStock = false, DateCreated = DateTime.Now};
Product p12 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0012", Name = "Fanta Grape", Description = "Black Grape drink", InStock = false, DateCreated = DateTime.Now};
Product p13 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0013", Name = "Sprite", Description = "Sweet Lemonade Drink", InStock = false, DateCreated = DateTime.Now};
Product p14 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0014", Name = "Sweppes Dry Lemon", Description = "Sour Lemonade Drink", InStock = false, DateCreated = DateTime.Now};
Product p15 = new Product() { ProductId = Guid.NewGuid(), ProductCode = "S0014", Name = "Sweppes Granadilla", Description = "Granafilla Drink", InStock = false, DateCreated = DateTime.Now};
Products.InsertOnSubmit(p1);
Products.InsertOnSubmit(p2);
Products.InsertOnSubmit(p3);
Products.InsertOnSubmit(p4);
Products.InsertOnSubmit(p5);
Products.InsertOnSubmit(p6);
Products.InsertOnSubmit(p7);
Products.InsertOnSubmit(p8);
Products.InsertOnSubmit(p9);
Products.InsertOnSubmit(p10);
Products.InsertOnSubmit(p11);
Products.InsertOnSubmit(p12);
Products.InsertOnSubmit(p13);
Products.InsertOnSubmit(p14);
Products.InsertOnSubmit(p15);
OrderItems.InsertOnSubmit(new OrderItem()
{
	OrderItemId = Guid.NewGuid(),
	ItemNumber = 123456,
	Quantity = 10,
	OrderId = o1.OrderId,
	ProductId = p1.ProductId,
	DateCreated = DateTime.Now
});

SubmitChanges();

OrderItems.Dump();
Orders.Dump();
Products.Dump();