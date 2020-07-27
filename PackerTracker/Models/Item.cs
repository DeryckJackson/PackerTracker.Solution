using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PackerTracker.Models
{
  public class Item
  {
    public string Description { get; set; }
    public int Price { get; set; }
    public string Manufacturer { get; set; }
    public bool IsPacked { get; set; }
    public int Id { get; }
    public Item (string description, int price, string manufacturer, bool isPacked, int id)
    {
      Description = description;
      Price = price;
      Manufacturer = manufacturer;
      IsPacked = isPacked;
      Id = id;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        string desc = rdr.GetString(0);
        int price = rdr.GetInt32(1);
        string manufacturer = rdr.GetString(2);
        bool isPacked = rdr.GetBoolean(3);
        int id = rdr.GetInt32(4);
        allItems.Add(new Item(desc, price, manufacturer, isPacked, id));
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }
    public static void ClearAll()
    {
      _instances.Clear();
    }

    public static Item Find(int searchId)
    {
      return _instances[searchId];
    }

    public static List<Item> GetUnpacked()
    {
      List<Item> outputList = new List<Item>();
      foreach (Item item in _instances)
      {
        if (!item.IsPacked)
        {
          outputList.Add(item);
        }
      }
      return outputList;
    }

    public static void SetIsPackedTrueById(int id)
    {
      _instances[id].IsPacked = true;
    }
  }
}