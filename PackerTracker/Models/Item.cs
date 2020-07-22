using System.Collections.Generic;

namespace PackerTracker.Models
{
  public class Item
  {
    public string Description { get; set; }
    public int Price { get; set; }
    public string Manufacturer { get; set; }
    private static List<Item> _instances = new List<Item> {};
    public bool IsPacked { get; set; }
    public int Id { get; }
    public Item (string description, int price, string manufacturer)
    {
      Description = description;
      Price = price;
      Manufacturer = manufacturer;
      IsPacked = false;
      _instances.Add(this);
      Id = _instances.Count - 1;
    }

    public static List<Item> GetAll()
    {
      return _instances;
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