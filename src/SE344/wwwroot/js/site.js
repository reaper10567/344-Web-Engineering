// Write your Javascript code.
(function ($) {
    var chatProxy = $.connection.chatHub;

    chatProxy.client.newMessage = function (username, message) {
        $('#messagesPane').append('' +
            '<li class="list-group-item">' +
            '<h5 class="list-group-item-heading">' + username + '</h5>' +
            '<p class="list-group-item-text">' + message + '</p>' +
            '</li>');

        // Then auto scroll
        $('#messagesPane').stop().animate({
            scrollTop: $("#messagesPane")[0].scrollHeight
        }, 300);
    };

    $.connection.hub.start().done(function () {
        console.log('Now connected, connection ID = ' + $.connection.hub.id);

        $('#sendMessage').click(sendMessage);
        $('#newMessage').keypress(function (event) {
            if (event.which === 13) {
                event.preventDefault();
                sendMessage();
            }
        });
    }).fail(function () {
        console.warn('Could not Connect!');
    });

    function sendMessage() {
        var message = $('#newMessage').val();
        if (message && message.length !== 0) {
            chatProxy.server.sendMessage(window.CURRENT_USER, message).done(function () {
                // clear chat input after succesfully sent
                $('#newMessage').val('');
            }).fail(function () {
                console.warn('Failed to send message');
            });
        }
    }

})(jQuery)
