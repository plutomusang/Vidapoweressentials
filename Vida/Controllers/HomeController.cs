using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Vida.Controllers
{
    public class HomeController : Controller
    {
         public ActionResult Signin()
        {
            string appkey = Request.QueryString["appkey"];
            string token =  Request.QueryString["tok"];
            string redirectUrl = Request.QueryString["ReturnUrl"] ==""?"Home": Request.QueryString["ReturnUrl"];

            //-- lets check if cookie is available then lets overwrite
            HttpCookie appCookie = Request.Cookies["appKey"];
            if (appCookie != null)
            {
                appkey = appCookie.Value;
            }

            //-- lets compare if its legit
            string code = ConfigurationManager.AppSettings["SaltKey"].ToString() + token;
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(code);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            if(sb.ToString() == appkey)
            {
                //-- lets authenticate the user
                FormsAuthentication.SetAuthCookie(null, true);
                Session["token"] = token;
                if (redirectUrl =="Home")
                {
                    redirectUrl = "UserAccount";
                }
                else
                {
                    //--- lets remove the first slash if present
                    if(redirectUrl.Substring(0,1) == "/")
                    {
                        redirectUrl = redirectUrl.Substring(1, redirectUrl.Length-1);
                    }
                }
            }


            return RedirectToAction("Index",redirectUrl);
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

            //
            // GET: /Home/

            public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

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
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

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
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

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
