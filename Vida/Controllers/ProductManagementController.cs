using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vida.Controllers
{
    [UserSession]
    public class ProductManagementController : Controller
    {
        //
        // GET: /ProductManagement/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ProductManagement/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ProductManagement/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductManagement/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductManagement/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ProductManagement/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductManagement/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProductManagement/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
