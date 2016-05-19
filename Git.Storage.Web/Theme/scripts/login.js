var Login = function () {
    return {
        init: function () {

            jQuery('#forget-password').click(function () {
                jQuery('#loginform').hide();
                jQuery('#forgotform').show(200);
            });

            jQuery('#forget-btn').click(function () {
                jQuery('#loginform').slideDown(200);
                jQuery('#forgotform').slideUp(200);
            });

            //µÇÂ¼
            $("#login-btn").click(function () {
                var userName = $("#input-username").val();
                var passWord = $("#input-password").val();
                var remPass = $("#rdRember").attr("checked");

            });
        }
    };
}();