using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filter;
using BlogAppDAL;
using BlogAppDAL.UoW;
namespace WebApplication1.Controllers
{
    public class PSS_ExWarehouseModeController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        //
        // GET: /PSS_ExWarehouseMode/
        [RoleActionFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadList()
        {
            return Json(uow.PSS_ExWarehouseMode.GetAll());
        }
        [HttpPost]
        public ActionResult LoadForm(string id)
        {
            int dbId = int.Parse(id);
            PSS_ExWarehouseMode exWarehouseMode=null;
            if (!string.IsNullOrEmpty(id))
                exWarehouseMode = uow.PSS_ExWarehouseMode.Get(b => b.ID == dbId);
                return Json(exWarehouseMode);
        }
        [HttpPost]
        public ActionResult AcceptClick(PSS_ExWarehouseMode obj)
        {
            int isOk = 1;
            //判断ID的值，ID为0表示ID为默认值，插入记录。非0的ID代表数据库已存在记录，则修改
            try
            {
                if (obj.ID != 0)
                    uow.PSS_ExWarehouseMode.Update(obj);
                else
                    uow.PSS_ExWarehouseMode.Insert(obj);
                uow.SaveChanges();
            }
            catch (Exception)
            {
                isOk = 0;
            }
            return Content(isOk.ToString());
        }
        [HttpPost]
        public ActionResult Del(string id)
        {
            int isOk = 1;
            int dbId = int.Parse(id);
            var obj = uow.PSS_ExWarehouseMode.Get(b => b.ID == dbId);
            try
            {
                if (!string.IsNullOrEmpty(id))
                    uow.PSS_ExWarehouseMode.Delete(obj);
                uow.SaveChanges();
            }
            catch (Exception ex)
            {
                isOk = 0;
            }
            return Content(isOk.ToString());
        }
    }
}