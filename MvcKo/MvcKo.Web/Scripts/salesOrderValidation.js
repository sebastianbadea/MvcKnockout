$("form").validate({
    submitHandler: function () {
        //the variable defined in the Operations view
        salesOrderViewModel.save();
    },
    rules: {
        CustomerName: {
            required: true
        }
    }
});