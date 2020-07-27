using Microsoft.AspNetCore.Mvc;
using PackerTracker.Models;
using System.Collections.Generic;

namespace PackerTracker.Controllers
{
  public class ItemsController : Controller
  {
    [HttpGet("/items")]
    public ActionResult Index()
    {
      List<Item> unpackedItems = Item.GetUnpacked();
      return View(unpackedItems);
    }
    [HttpGet("/items/new")]
    public ActionResult New()
    {
      return View();
    }
    [HttpPost("/items")]
    public ActionResult Create(string description, string price, string manufacturer)
    {
      Item myItem = new Item(description, int.Parse(price), manufacturer);
      myItem.Save();
      return RedirectToAction("Index");
    }

    [HttpPost("items/delete")]
    public ActionResult Delete()
    {
      Item.ClearAll();
      return View();
    }

    [HttpGet("/items/{id}")]
    public ActionResult Show(int id)
    {
      Item foundItem = Item.Find(id);
      return View(foundItem);
    }

    [HttpPost("/items/checked")]
    public ActionResult Checked(int[] itemIds)
    {
      foreach (int id in itemIds)
      {
        Item.SetIsPackedTrueById(id);
      }
      return RedirectToAction("Index");
    }
  }
}