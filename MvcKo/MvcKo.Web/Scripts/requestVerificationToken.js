$(document).ready(function () {
    securityToken = $('[name=__RequestVerificationToken]').val();
    $(document).ajaxSend(function (elm, xhr, s) {
        if (s.type == 'POST' && typeof securityToken != 'undefined') {
            if (s.data.length > 0) {
                s.data += "&__RequestVerificationToken=" + encodeURIComponent(securityToken);
            }
            else {
                s.data = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
            }
        }
    });
    //$('body').bind('ajaxSend', function (elm, xhr, s) {
    //    alert('from ajax send');
    //    if (s.type == 'POST' && typeof securityToken != 'undefined') {
    //        if (s.data.length > 0) {
    //            s.data += "&__RequestVerificationToken=" + encodeURIComponent(securityToken);
    //        }
    //        else {
    //            s.data = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
    //        }
    //    }
    //});
});