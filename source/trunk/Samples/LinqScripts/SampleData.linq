<Query Kind="Statements">
  <Connection>
    <ID>75bff1c5-bfa4-4e93-904b-24f949788874</ID>
    <Persist>true</Persist>
    <Server>PAULKOLOZSV4DD3\SQL2008STANDARD</Server>
    <Database>StockTaker</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

Stocks.DeleteAllOnSubmit(Stocks);
Stocks.InsertOnSubmit(new Stock()
{
	StockTakeId = Guid.NewGuid(),
	ProductCode = "KMI123",
	Location = "B01",
	Quantity = 100,
	Description = "This is the first stock item.",
	DateCreated = DateTime.Now
});
Stocks.InsertOnSubmit(new Stock()
{
	StockTakeId = Guid.NewGuid(),
	ProductCode = "KMI456",
	Location = "B02",
	Quantity = 127,
	Description = "This is the second stock item.",
	DateCreated = DateTime.Now
});
SubmitChanges();
Stocks.Dump();