using MvcKo.DataLayer;
using MvcKo.Model;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View(salesOrder);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalesOrderId,CustomerName,PoNumber")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _db.SalesOrders.Add(salesOrder);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(salesOrder);
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
            return View(salesOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalesOrderId,CustomerName,PoNumber")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(salesOrder).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesOrder);
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
            return View(salesOrder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesOrder salesOrder = _db.SalesOrders.Find(id);
            _db.SalesOrders.Remove(salesOrder);
            _db.SaveChanges();
            return RedirectToAction("Index");
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
    }
}
