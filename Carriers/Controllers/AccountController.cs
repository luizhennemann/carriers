using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carriers.Models;
using System.Web.Security;

namespace Carriers.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Users U)
        {
            using (CARRIERSEntities CE = new CARRIERSEntities())
            {
                var sql = from r 
                          in CE.Users
                          where (r.Login == U.Login && r.Password == U.Password)
                          select r;

                if (sql.Count() == 0)
                {
                    ViewBag.msg = "Invalid user.";
                    return View();
                }
                else
                {
                    Users user = sql.FirstOrDefault();
                    FormsAuthentication.SetAuthCookie(U.Login, false);
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}