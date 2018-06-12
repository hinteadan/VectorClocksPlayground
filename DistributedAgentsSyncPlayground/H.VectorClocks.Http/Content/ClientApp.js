(function ($) {

    var $inputMessage = $('#inputMessage');
    var $buttonSendMessage = $('#buttonSendMessage');
    var $log = $('#log');

    $buttonSendMessage.click(function () {
        $.ajax({
            type: "POST",
            url: '/say',
            contentType: "application/json",
            data: { Message: $inputMessage.val() },
        });
    });

    $inputMessage.keypress(function (e) {
        if (e.which !== 13) return;
        $buttonSendMessage.click();
    });



    function refreshStatus() {
        $.ajax({
            type: "GET",
            url: '/status',
            contentType: "application/json",
            success: function (data, textStatus, $xhr) {
                if (data.isSuccessfull) {
                    var revision = '';
                    for (var i = 0; i < data.solution.revision.length; i++) {
                        revision += ` (${data.solution.revision[i].nodeID}:${data.solution.revision[i].version}) `
                    }
                    $log.html(`${data.solution.payload} [${revision}]`);
                }
                else {
                    $log.html(`Conflict between: ${data.conflict.nodeA.payload}  |  ${data.conflict.nodeB.payload}`);
                }
            },
        });
    }

    function refreshStatusAndQueueAnother() {
        refreshStatus();
        setTimeout(refreshStatusAndQueueAnother, 150);
    }

    refreshStatusAndQueueAnother();

})(this.jQuery);