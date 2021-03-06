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
    public int Id { get; set; }
    public Item (string description, int price, string manufacturer, bool isPacked, int id)
    {
      Description = description;
      Price = price;
      Manufacturer = manufacturer;
      IsPacked = isPacked;
      Id = id;
    }

    public Item (string description, int price, string manufacturer)
    {
      Description = description;
      Price = price;
      Manufacturer = manufacturer;
      IsPacked = false;
      Id = 0;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM item;";
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
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM item;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `item` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = searchId;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      string desc = "";
      int price = 0;
      string manufacturer = "";
      bool isPacked = false;
      int id = 0;
      while (rdr.Read())
      {
        desc = rdr.GetString(0);
        price = rdr.GetInt32(1);
        manufacturer = rdr.GetString(2);
        isPacked = rdr.GetBoolean(3);
        id = rdr.GetInt32(4);
      }

      Item foundItem = new Item(desc, price, manufacturer, isPacked, id);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundItem;
    }

    public static List<Item> GetUnpacked()
    {
      List<Item> outputList = new List<Item>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `item` WHERE ispacked = 0;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        string desc = rdr.GetString(0);
        int price = rdr.GetInt32(1);
        string manufacturer = rdr.GetString(2);
        bool isPacked = rdr.GetBoolean(3);
        int id = rdr.GetInt32(4);
        outputList.Add(new Item(desc, price, manufacturer, isPacked, id));
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return outputList;
    }

    public static void SetIsPackedTrueById(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE item SET ispacked=1 WHERE id = @thisId";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO item (description, price, manufacturer, ispacked) VALUES (@ItemDescription, @ItemPrice, @ItemManufacturer, @ItemIsPacked);";
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@ItemDescription";
      description.Value = this.Description;
      cmd.Parameters.Add(description);
      MySqlParameter price = new MySqlParameter();
      price.ParameterName = "@ItemPrice";
      price.Value = this.Price;
      cmd.Parameters.Add(price);
      MySqlParameter manufacturer = new MySqlParameter();
      manufacturer.ParameterName = "@ItemManufacturer";
      manufacturer.Value = this.Manufacturer;
      cmd.Parameters.Add(manufacturer);
      MySqlParameter ispacked = new MySqlParameter();
      ispacked.ParameterName = "@ItemIsPacked";
      ispacked.Value = this.IsPacked;
      cmd.Parameters.Add(ispacked);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}