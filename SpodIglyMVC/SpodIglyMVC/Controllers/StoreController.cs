using SpodIglyMVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SpodIglyMVC.Controllers
{
    public class StoreController : Controller
    {
        StoreContext Db = new StoreContext();
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var album = Db.Album.Find(id);

            return View(album);
        }
        public ActionResult List(string genrename, string searchQuery = null)
        {
            var genre = Db.Genre.Include("Albums").Where(g => g.Name.ToUpper() == genrename.ToUpper()).Single();

            var albums = genre.Albums.Where(a => (searchQuery == null ||
                                            a.AlbumTitle.ToLower().Contains(searchQuery.ToLower()) ||
                                            a.ArtistName.ToLower().Contains(searchQuery.ToLower())) &&
                                            !a.IsHidden);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", albums);
            }

            return View(albums);
        }

        [OutputCache(Duration = 80000)]
        [ChildActionOnly]
        public ActionResult GenresMenu()
        {
            var genres = Db.Genre.ToList();

            return PartialView("_GenresMenu", genres);
        }

        public ActionResult AlbumSuggestions(string term)
        {
            var album = this.Db.Album.Where(a => !a.IsHidden && a.AlbumTitle.ToLower().Contains(term.ToLower())).Take(5).Select(a => new { label = a.AlbumTitle });

            return Json(album, JsonRequestBehavior.AllowGet);
        }

    }
}