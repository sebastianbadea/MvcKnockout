﻿using MvcKo.DataLayer;
using MvcKo.Model;
using MvcKo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
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

            var salesOrderViewModel = Helpers.SetOrderViewModel(salesOrder);
            ViewBag.Title = "Sales order details";

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
                Helpers.SetOrderViewModel
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
            var salesOrderViewModel = Helpers.SetOrderViewModel(salesOrder);
            salesOrderViewModel.State = ObjectState.Deleted;
            
            ViewBag.Title = "Delete sales order";

            return View(salesOrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleModelStateException]
        public JsonResult Save(SalesOrderViewModel salesVM)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState);
            }
            var messageToClient = string.Empty;
            SalesOrder sales = null;
            try
            {
                sales = Helpers.SetOrderModel(salesVM);

                _db.SalesOrders.Attach(sales);

                if (salesVM.State == ObjectState.Deleted)
                {
                    DeleteItemsForDeletedOrder(salesVM.SalesOrderItems);
                }
                else
                {
                    DeleteItemsForSavedOrder(salesVM);
                }

                _db.ApplyStateChanges();
                _db.SaveChanges();

                if (salesVM.State == ObjectState.Deleted)
                {
                    return Json(new { ReturnUrl = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
                }

                messageToClient = Helpers.MessageToClient(salesVM);
                salesVM = Helpers.SetOrderViewModel(sales);
                
            }
            catch (DbUpdateConcurrencyException)
            {
                messageToClient = "Someone changed the item since you retrieve it.";
                salesVM.SalesOrderId = sales.SalesOrderId;
                _db.Dispose();
                _db = new SalesContext();
                var dbSales = _db.SalesOrders.Find(salesVM.SalesOrderId);
                salesVM = Helpers.SetOrderViewModel(dbSales);
            }
            catch (DbEntityValidationException ex)
            {
                var errors = Helpers.ExtractErrors(ex);
                salesVM.MessageToClient = string.Format("the saving failed with the following errors: {0}", errors);
            }
            catch (Exception ex)
            {
                throw new ModelStateException(ex);
            }
            finally
            {
                salesVM.MessageToClient = messageToClient;
                salesVM.State = ObjectState.Unchanged;
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
        /// <summary>
        /// This method set the state of the items of the order as deleted. 
        /// Is called when an order is deleted.
        /// </summary>
        /// <param name="salesVM"></param>
        private void DeleteItemsForDeletedOrder(List<SalesOrderItemViewModel> items)
        {
            foreach (var itemToDelete in items)
            {
                var item = _db.SalesOrderItems.Find(itemToDelete.SalesOrderItemId);
                item.State = ObjectState.Deleted;
            }
        }
        /// <summary>
        /// This method takes the id's set on the client and for each set state to deleted
        /// </summary>
        private void DeleteItemsForSavedOrder(SalesOrderViewModel salesVM)
        {
            foreach (var itemToDelete in salesVM.SalesOrderItemsToDelete)
            {
                var item = _db.SalesOrderItems.Find(itemToDelete);
                item.State = ObjectState.Deleted;
            }
        }
        #endregion
    }
}
