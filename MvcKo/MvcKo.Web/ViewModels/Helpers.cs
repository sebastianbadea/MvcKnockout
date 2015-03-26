using MvcKo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcKo.Web.ViewModels
{
    static class Helpers
    {
        public static SalesOrder SetModel(SalesOrderViewModel salesVM)
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

        public static SalesOrderViewModel SetViewModel(SalesOrder salesOrder, string messageToClient = "create view for the viewmodel")
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