using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vida.Controllers
{
    [UserSession]
    public class MembershipActivationController : Controller
    {
        //
        // GET: /MembershipActivation/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MembershipActivation/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MembershipActivation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MembershipActivation/Create

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
        // GET: /MembershipActivation/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MembershipActivation/Edit/5

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
        // GET: /MembershipActivation/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MembershipActivation/Delete/5

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
