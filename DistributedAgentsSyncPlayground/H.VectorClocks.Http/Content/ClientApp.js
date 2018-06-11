(function ($) {

    var $inputMessage = $('#inputMessage');
    var $buttonSendMessage = $('#buttonSendMessage');


    $buttonSendMessage.click(function () {

        $.ajax({
            type: "POST",
            url: '/say',
            contentType: "application/json",
            data: { Message: $inputMessage.val() },
        });

    });


})(this.jQuery);