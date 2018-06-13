(function ($) {

    var $messageEditor = $('#messageEditor');
    var $inputMessage = $('#inputMessage');
    var $buttonSendMessage = $('#buttonSendMessage');
    var $log = $('#log');
    var $conflictEditor = $('#conflictEditor');
    var $inputConflictResolveWith = $('#inputConflictResolveWith');
    var $buttonConflictResolve = $('#buttonConflictResolve');

    $buttonSendMessage.click(function () {
        $.ajax({
            type: "POST",
            url: '/say',
            contentType: "application/json",
            data: { Message: $inputMessage.val() },
            success: function (data, textStatus, $xhr) {
                $inputMessage.val('');
            },
        });
    });

    $buttonConflictResolve.click(function () {
        $.ajax({
            type: "POST",
            url: '/resolveConflict',
            contentType: "application/json",
            data: { Message: $inputConflictResolveWith.val() },
            success: function (data, textStatus, $xhr) {
                $inputConflictResolveWith.val('');
                $conflictEditor.hide();
                $messageEditor.show();
            },
        });
    });

    $inputMessage.keypress(function (e) {
        if (e.which !== 13) return;
        $buttonSendMessage.click();
    });

    $inputConflictResolveWith.keypress(function (e) {
        if (e.which !== 13) return;
        $buttonConflictResolve.click();
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
                    $log.html(`Conflict between: ${data.conflict.nodeA.payload}${printRevision(data.conflict.nodeA)}  |  ${data.conflict.nodeB.payload}${printRevision(data.conflict.nodeB)}`);
                    $messageEditor.hide();
                    $conflictEditor.show();
                }
            },
        });
    }

    function printRevision(node) {
        return `(${node.revision.map(x => x.version).join(',')})`;
    }

    function refreshStatusAndQueueAnother() {
        refreshStatus();
        setTimeout(refreshStatusAndQueueAnother, 150);
    }

    refreshStatusAndQueueAnother();

})(this.jQuery);