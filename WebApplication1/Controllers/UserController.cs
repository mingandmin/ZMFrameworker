using BlogAppDAL;
using BlogAppDAL.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filter;//过滤器引用
namespace WebApplication1.Controllers
{
 
    public class UserController : Controller
    {
        
        //
        // GET: /User/
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["user"];
            if (cookie != null) {
                return RedirectToAction("Index","Home");
            }
            else
            return View();
        }

        [HttpPost]
        public ActionResult CheckUserLogin(string username, string password)
        {
            string msg = string.Empty;
            COM_User user;
            username = HttpUtility.UrlEncode(username.Trim());
            password = password.Trim();
            HttpCookie cookie = new HttpCookie("user");
            using (var uow = new UnitOfWork())
            {
                user = uow.COM_UserRepository.Get(b => b.Username == username);
            }

            if (user == null)
                msg = "1";
            else if (user.Password == EncryptUtils.MD5Encrypt(password))
            {
                msg = "2";
                cookie.Values.Add("Username", HttpUtility.UrlEncode(user.Username));
                cookie.Values.Add("Code", user.Code);
                cookie.Values.Add("Name", HttpUtility.UrlEncode(user.Name));
                cookie.Values.Add("Tel", user.Tel);
                cookie.Values.Add("Role", HttpUtility.UrlEncode(user.Role));
                cookie.Values.Add("Address", HttpUtility.UrlEncode(user.Address));
                cookie.Values.Add("Department", HttpUtility.UrlEncode(user.Department));
                cookie.Values.Add("IP", user.IP);
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.SetCookie(cookie);
            }
            else
                msg = "3";
            return Content(msg);
        }

        public ActionResult Logout()
        { 
            HttpCookie cookie= Request.Cookies["user"];
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login","User");
        }

        public ActionResult ModifyPassword(string Username, string Password, string NewPassword)
        { 
            int isOk=default(int);
            using (var uow = new UnitOfWork())
            {
                COM_User user = uow.COM_UserRepository.Get(b => b.Username == Username);
                Password=EncryptUtils.MD5Encrypt(Password.Trim());
                if (user.Password == Password)
                {
                    user.Password = EncryptUtils.MD5Encrypt(NewPassword.Trim());
                    uow.COM_UserRepository.Update(user);
                    isOk = 1;
                }
                else
                    isOk = -1;
                return Content(isOk.ToString());
            }  
        }

        UnitOfWork uow = new UnitOfWork();//IOC容器

        /// <summary>
        /// 用户信息维护视图
        /// </summary>
        /// <returns></returns>
        [RoleActionFilter]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 加载所有数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadList()
        {
            return Json(uow.COM_UserRepository.GetAll());
        }
        /// <summary>
        /// 加载弹窗修改或新增表单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadForm(string userName)
        {
            COM_User user = null;
            if (!string.IsNullOrEmpty(userName))
                user = uow.COM_UserRepository.Get(b => b.Name == userName);
            return Json(user);
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AcceptClick(COM_User obj)
        {
            int isOk = 0;
            //判断userName的值 ，默认是 空，插入记录。非空代表数据库已经存在，则修改
            isOk = uow.COM_UserRepository.GetAll(b=>b.Name==obj.Name).ToList().Count;
            try
            {
                if (isOk>0)
                    uow.COM_UserRepository.Update(obj);
                else
                    uow.COM_UserRepository.Insert(obj);
                uow.SaveChanges();
                isOk = 1;//操作成功
            }
            catch (Exception)
            {
                isOk = 0;//操作失败
            }
            return Content(isOk.ToString());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(string userName)
        {
            int isOk = 1;
            var obj = uow.COM_UserRepository.Get(b => b.Name == userName);
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    uow.COM_UserRepository.Delete(obj);
                    uow.SaveChanges();
                }
            }
            catch (Exception)
            {
                isOk = 0;
            }
            return Content(isOk.ToString());
        }
    }
}