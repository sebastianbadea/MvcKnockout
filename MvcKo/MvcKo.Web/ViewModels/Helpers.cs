﻿using MvcKo.Model;
using System.Data.Entity.Validation;
using System.Text;

namespace MvcKo.Web.ViewModels
{
    static class Helpers
    {
        #region order
        public static SalesOrder SetOrderModel(SalesOrderViewModel orderVM)
        {
            var order =
                new SalesOrder
                {
                    CustomerName = orderVM.CustomerName,
                    PoNumber = orderVM.PoNumber,
                    State = orderVM.State,
                    SalesOrderId = orderVM.SalesOrderId
                };

            foreach (var itemVm in orderVM.SalesOrderItems)
            {
                var item = SetOrderItemModel(itemVm);
                order.SalesOrderItems.Add(item);
            }

            return order;
        }

        public static SalesOrderViewModel SetOrderViewModel(SalesOrder order, string messageToClient = "create view for the viewmodel")
        {
            var orderVm =
                new SalesOrderViewModel
                {
                    SalesOrderId = order.SalesOrderId,
                    CustomerName = order.CustomerName,
                    PoNumber = order.PoNumber,
                    MessageToClient = messageToClient
                };

            foreach (var item in order.SalesOrderItems)
            {
                var itemVm = SetOrderItemViewModel(item);
                orderVm.SalesOrderItems.Add(itemVm);
            }

            return orderVm;
        }
        #endregion

        #region order item
        private static SalesOrderItemViewModel SetOrderItemViewModel(SalesOrderItem item)
        {
            var itemVm =
                new SalesOrderItemViewModel
                {
                    SalesOrderId = item.SalesOrderId,
                    SalesOrderItemId = item.SalesOrderItemId,
                    ProductCode = item.ProductCode,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    State = item.State
                };
            return itemVm;
        }

        private static SalesOrderItem SetOrderItemModel(SalesOrderItemViewModel itemVm)
        {
            var item =
                new SalesOrderItem
                {
                    ProductCode = itemVm.ProductCode,
                    Quantity = itemVm.Quantity,
                    UnitPrice = itemVm.UnitPrice,
                    State = itemVm.State,
                    SalesOrderItemId = itemVm.SalesOrderItemId,
                    SalesOrderId = itemVm.SalesOrderId
                };
            return item;
        }
        #endregion

        public static string ExtractErrors(DbEntityValidationException ex)
        {
            var sb = new StringBuilder();

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

        public static string MessageToClient(SalesOrderViewModel salesVM)
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
    }
}