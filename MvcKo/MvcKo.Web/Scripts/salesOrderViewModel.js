﻿var State = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3
};

salesOrderItemMapping = {
    //SalesOrderItems will be the name of the observable and will be used in the html template
    'SalesOrderItems': {
        key: function (SalesOrderItem) {
            return ko.utils.unwrapObservable(SalesOrderItem.SalesOrderItemId);
        },
        create: function (options) {
            return new salesOrderItemViewModel(options.data);
        }
    }
};

var dataConverter = function (key, value) {
    if (key === "RowVersion" && Array.isArray(value)) {
        var str = String.fromCharCode.apply(null, value);
        return btoa(str);
    }
    return value;
}

var salesOrderItemViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.flagAsEdited = function () {
        if (self.State() != State.Added) {
            self.State(State.Modified);
        }
        return true;
    },
    self.TotalPrice = ko.computed(function () {
        return (self.Quantity() * self.UnitPrice()).toFixed(2);
    });
}

SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

    self.save = function () {
        var jsParams = ko.toJS(self);
        var securityToken = $('[name=__RequestVerificationToken]').val();
        //here you can add/remove parameters from the observable
        var params =
            this.setParameters
            ({
                params: jsParams,
                toBeAdded: 
                    [{
                        key: "__RequestVerificationToken",
                        value: encodeURIComponent(securityToken)
                    }],
                toBeRemoved: ["save", "setParameters", "addItem", "flagAsEdited", "__ko_mapping__", "DeleteOrderItem"]
            });
        $.ajax({
            url: $("#urlSavePost").val(),
            type: "POST",
            data: params,
            success: function (data) {
                if (data.ReturnUrl != null) {
                    window.location = data.ReturnUrl;
                }
                if (data.salesVM != null) {
                    ko.mapping.fromJS(data.salesVM, {}, self);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status == 400) {
                    $("#messageToClient").text(XMLHttpRequest.responseText);
                }
                else {
                    $("#messageToClient").text("The server had a problem.");
                }
            }
        });
    },
    self.addItem = function () {
        var salesOrderItem =
            new salesOrderItemViewModel
            (
                {
                    SalesOrderId: self.SalesOrderId(),
                    SalesOrderItemId: 0,
                    ProductCode: "",
                    Quantity: 1,
                    UnitPrice: 0,
                    State: State.Added
                });
        self.SalesOrderItems.push(salesOrderItem);
    },
    self.flagAsEdited = function () {
        if (self.State() != State.Added) {
            self.State(State.Modified);
        }
        return true;
    },
    self.TotalPrice = ko.computed(function () {
        var total = 0;
        ko.utils.arrayForEach(self.SalesOrderItems(), function (saleOrderItem) {
            total += parseFloat(saleOrderItem.TotalPrice());
        });

        return total.toFixed(2);
    }),
    self.DeleteOrderItem = function (item) {
        self.SalesOrderItems.remove(item);
        if (item.SalesOrderItemId() > 0 && self.SalesOrderItemsToDelete().indexOf(item.SalesOrderItemId()) == -1) {
            self.SalesOrderItemsToDelete().push(item.SalesOrderItemId());
        }
    },

    //#region private functions
    self.setParameters = function (opt) {
        //add
        for (var i = 0; i < opt.toBeAdded.length; i++) {
            opt.params[opt.toBeAdded[i].key] = opt.toBeAdded[i].value;
        }
        //remove
        for (var i = 0; i < opt.toBeRemoved.length; i++) {
            delete opt.params[opt.toBeRemoved[i]];
        }
        //convert 
        for (var key in opt.params) {
            if (opt.params.hasOwnProperty(key)) {
                var obj = opt.params[key];
                opt.params[key] = dataConverter(key, opt.params[key]);
            }
        }

        return opt.params;
    }
    //#endregion
}