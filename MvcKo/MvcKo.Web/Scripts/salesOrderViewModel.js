var State = {
    Unchanged: 0,
    Added: 1, 
    Modified: 2, 
    Deleted: 3
};
SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);
    self.save = function () {
        var jsParams = ko.toJS(self);
        var securityToken = $('[name=__RequestVerificationToken]').val();
        var params =
            this.setParameters
            ({
                params: jsParams,
                toBeAdded: 
                    [{key: "__RequestVerificationToken",
                      value: encodeURIComponent(securityToken)}],
                toBeRemoved: ["save", "setParameters"]
            });
        $.ajax({
            url: $("#urlSavePost").val(),
            type: "POST",
            data: params,
            success: function (data) {
                if (data.salesVM != null) {
                    ko.mapping.fromJS(data.salesVM, {}, self);
                }
            }
        });
    };
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
}