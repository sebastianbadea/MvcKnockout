var State = {
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

salesOrderItemViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
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
                    [{key: "__RequestVerificationToken",
                      value: encodeURIComponent(securityToken)}],
                toBeRemoved: ["save", "setParameters", "addItem", "flafAsEdited", "__ko_mapping__"]
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
            }
        });
    };
    self.addItem = function () {
        var salesOrderItem =
            new salesOrderItemViewModel
            (
                {
                    SalesOrderId: self.SalesOrderId(),
                    SalesOrderItemId: 0,
                    ProductCode: "",
                    Quantity: "1",
                    UnitPrice: 0,
                    State: State.Added
                });
        self.SalesOrderItems.push(salesOrderItem);
    }

    //#region private functions
    self.flafAsEdited = function () {
        if (self.State() != State.Added) {
            self.State(State.Modified);
        }
        return true;
    },
    self.setParameters = function (opt) {        
        for (var i = 0; i < opt.toBeAdded.length; i++) {
            opt.params[opt.toBeAdded[i].key] = opt.toBeAdded[i].value;
        }

        for (var i = 0; i < opt.toBeRemoved.length; i++) {
            delete opt.params[opt.toBeRemoved[i]];
        }

        return opt.params;
    }
    //#endregion
}