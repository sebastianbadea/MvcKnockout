﻿using MvcKo.DataLayer;
using MvcKo.Model;
using MvcKo.Web.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MvcKo.Web.Controllers
{
    public class SalesController : Controller
    {
        #region fields
        private SalesContext _db;
        #endregion

        #region constructor
        public SalesController()
        {
            _db = new SalesContext();
        }
        #endregion

        #region action methods
        public ActionResult Index()
        {
            return View(_db.SalesOrders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _db.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            var salesOrderViewModel = SetViewModel(salesOrder);

            return View(salesOrderViewModel);
        }

        public ActionResult Create()
        {
            var salesOrderViewModel = new SalesOrderViewModel { State = ObjectState.Added };
            ViewBag.Title = "Create a new order";
            return View("Operations",salesOrderViewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _db.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }
            var salesOrderViewModel = 
                SetViewModel
                (
                    salesOrder,
                    string.Format("The original value of the customer name is {0}.", salesOrder.CustomerName)
                );
            ViewBag.Title = "Edit the order";
            return View("Operations", salesOrderViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _db.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }
            var salesOrderViewModel = SetViewModel(salesOrder);
            salesOrderViewModel.State = ObjectState.Deleted;
            
            ViewBag.Title = "Delete sales order";

            return View(salesOrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(SalesOrderViewModel salesVM)
        {
            try
            {
                var sales = SetModel(salesVM);

                _db.SalesOrders.Attach(sales);
                _db.ApplyStateChanges();
                _db.SaveChanges();

                if (salesVM.State == ObjectState.Deleted)
                {
                    return Json(new { ReturnUrl = Url.Action("Index")}, JsonRequestBehavior.AllowGet);
                }

                salesVM.SalesOrderId = sales.SalesOrderId;
                salesVM.MessageToClient = MessageToClient(salesVM);
                salesVM.State = ObjectState.Unchanged;
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ExtractErrors(ex);
                salesVM.MessageToClient = string.Format("the saving failed with the following errors: {0}", errors);
            }
            return Json(new { salesVM }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region overriden methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region private methods
        private static SalesOrder SetModel(SalesOrderViewModel salesVM)
        {
            return
                new SalesOrder
                {
                    CustomerName = salesVM.CustomerName,
                    PoNumber = salesVM.PoNumber,
                    State = salesVM.State,
                    SalesOrderId = salesVM.SalesOrderId
                };
        }

        private SalesOrderViewModel SetViewModel(SalesOrder salesOrder, string messageToClient = "create view for the viewmodel")
        {
            return 
                new SalesOrderViewModel
                {
                    SalesOrderId = salesOrder.SalesOrderId,
                    CustomerName = salesOrder.CustomerName,
                    PoNumber = salesOrder.PoNumber,
                    MessageToClient = messageToClient
                };
        }

        private string ExtractErrors(DbEntityValidationException ex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        private string MessageToClient(SalesOrderViewModel salesVM)
        {
            var message = string.Empty;
            switch (salesVM.State)
            {
                case ObjectState.Added:
                    message = string.Format("{0}'s orders have been added.", salesVM.CustomerName);
                    break;
                case ObjectState.Modified:
                    message = string.Format("{0}'s orders have been modified.", salesVM.CustomerName);
                    break;
            }

            return message;
        }
        #endregion
    }
}
