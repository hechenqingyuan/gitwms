var FormSamples = function () {


    return {
        //main function to initiate the module
        init: function () {

            // to fix chosen dropdown width in inactive hidden tab content
            $('.advance_form_with_chosen_element').on('shown', function (e) {
                App.initChosenSelect('.chosen_category:visible');
            });

        }

    };

}();