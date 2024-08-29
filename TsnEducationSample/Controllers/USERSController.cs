using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TsnEducation2024.Models;

namespace TsnEducation2024.Controllers
{
    public class UsersController : Controller
    {
        private TsnSampleEducationContext db = new TsnSampleEducationContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        /// <summary>
        /// ユーザの詳細(プロフィール画面)を表示する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            //パラメータなかったらBadRequest画面表示
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //データベースない人用
            if(!db.Database.Exists()){
                ViewData["NotExistDB"] = "接続先データベースが存在しないため、画面を表示できません";
                return View(new Users());
            }
            //IDをもとに、画面に表示するユーザ情報を取得
            Users targetUser = db.Users.Find(id);
            //IDに紐づくユーザがいない場合はNotFound画面表示
            if (targetUser == null)
            {
                return HttpNotFound();
            }
            //ユーザ情報をもとに画面表示する
            return View(targetUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_ID,User_NAME,LOGIN_PASSWORD,EMAIL,BIRTHDAY,REMARKS,IS_DELETE,INSERT_ID,INSERT_DATE,UPDATE_ID,UPDATE_DATE")] Users uSERS)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(uSERS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uSERS);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users uSERS = db.Users.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // POST: Users/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_ID,User_NAME,LOGIN_PASSWORD,EMAIL,BIRTHDAY,REMARKS,IS_DELETE,INSERT_ID,INSERT_DATE,UPDATE_ID,UPDATE_DATE")] Users uSERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSERS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uSERS);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users uSERS = db.Users.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users uSERS = db.Users.Find(id);
            db.Users.Remove(uSERS);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
