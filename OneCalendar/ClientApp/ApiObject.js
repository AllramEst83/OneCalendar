

var ApiObject = {

    GetEventsByUserId: function (url, id) {
        return $.ajax({
            url: url,
            method: 'GET',
            data: { id: id },
            mediaType:'application/json'
        })
            .done(function (data, textStatus) {
                console.log(`Events has been fetched: ${data}`);
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`ajax failed: ${textStatus}`);
            });
    }

};