SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
    self.save = function () {
        $.ajax({
            url: $("#urlSavePost").val(),
            type: "POST",
            data: ko.toJSON(self),
            contentType: "application/json",
            success: function (data) {
                if (data.salesVM != null) {
                    ko.mapping.fromJS(data.salesVM, {}, self);
                }
            }
        });
    }
}