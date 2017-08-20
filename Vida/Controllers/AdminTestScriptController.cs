using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vida.Controllers
{
    public class AdminTestScriptController : Controller
    {
        //
        // GET: /AdminTestScript/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AdminTestScript/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdminTestScript/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminTestScript/Create

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
        // GET: /AdminTestScript/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AdminTestScript/Edit/5

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
        // GET: /AdminTestScript/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AdminTestScript/Delete/5

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
