//in order to validate, you must have have a form
//usually you can use the submit button, but there is a workaround(http://stackoverflow.com/questions/2107279/does-the-jquery-validation-plugin-require-a-form-tag)
$("form").validate({
    submitHandler: function () {
        //the variable defined in the Operations view
        salesOrderViewModel.save();
    },
    rules: {
        CustomerName: {
            required: true,
            maxlength: 30
        },
        PoNumber: {
            maxlength: 10
        },
        ProductCode: {
            required: true,
            maxlength: 30,
            alphaonly: true
        },
        Quantity: {
            required: true,
            number: true,
            range: [1, 100]
        },
        UnitPrice: {
            required: true,
            number: true,
            range: [1, 100000]
        }
    },
    //if you don't specify the messages, it will display the default one
    messages: {
        CustomerName: {
            required: "The order must have a customer name.",
            maxlength: "The customer name must have between 1-30 characters."
        },
        //you can specify messages only for certain rules and the others will have the default message
        ProductCode: {
            alphaonly: "The product code can have only letters."
        },
        Quantity: {
            required: "The quantity is required",
            range: "[1, 100]"
        },
        UnitPrice: {
            required: "The quantity is required",
            range: "[1, 100000]"
        }
    },
    showErrors: function (errorMap, errorList) {
        $.each(errorList, function (index, value) {
            $(value.element).parent().addClass("has-error");
        });
        this.defaultShowErrors();
    }
});
//you can add custom validations
$.validator.addMethod("alphaonly", function (value) {
    return /^[A-Za-z]+$/.test(value);
});