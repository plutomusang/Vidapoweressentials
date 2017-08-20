using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vida.Controllers
{
    public class GenealogyController : Controller
    {
        //
        // GET: /Genealogy/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Genealogy/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Genealogy/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Genealogy/Create

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
        // GET: /Genealogy/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Genealogy/Edit/5

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
        // GET: /Genealogy/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Genealogy/Delete/5

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
