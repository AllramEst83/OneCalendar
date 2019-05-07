

var ApiObject = {

    Request: function (settings) {

        return $.ajax({
            url: settings.url + `?Authorization=Bearer ${settings.token}`,
            method: settings.method,
            data: settings.data,
            mediaType: settings.mediaType,
            headers: {
                'Authorization': `Bearer  ${settings.token}`,
                //"Access-Control-Allow-Origin": "*",
                "Content-Type": "application/json"
            }
        })
            .done(function (data, textStatus) {

                console.log(`%c Request success: ${data}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR.responseJSON.errors.Description}, ${settings.url}`, 'background: #222; color:red');
                ApiObject.ShowErrorMessage(jqXHR.responseJSON.errors.Description);
       
            });
    },
    RequestWithOutData: function (settings) {

        return $.ajax({
            url: settings.url + `?Authorization=Bearer ${settings.token}`,
            method: settings.method,
            mediaType: settings.mediaType,
            headers: {
                'Authorization': `Bearer ${settings.token}`,
                "Content-Type": "application/json"
            }
        })
            .done(function (data, textStatus) {
                console.log(`%c Request success: ${data}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR.responseJSON.errors.Description}`, 'background: #222; color:red');
                alert(`Det verkar vara ett problem med kontakten till servern. URL: ${settings.url}`);
            });
    },
    RequestWithOutAuth: function (settings) {
        return $.ajax({
            url: settings.url,
            method: settings.method,
            data: settings.data,
            mediaType: settings.mediaType,
            headers: {
                "Content-Type": "application/json"
            }
        })
            .done(function (data, textStatus) {
                var userData = JSON.parse(data);
                console.log(`%c Request success: ${userData.statusCode} & ${userData.description}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR.responseJSON.errors.Description}`, 'background: #222; color:red');
                alert(`Det verkar vara ett problem med kontakten till servern. URL: ${settings.url}`);
            });
    },
    SimpleRequest: function (settings) {
        return $.ajax({
            url: settings.url,
            method: settings.method,
            mediaType: settings.mediaType,
            headers: {
                "Content-Type": "application/json"
            }
        })
            .done(function (data, textStatus) {
                var userData = JSON.parse(data);
                console.log(`%c Request success`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR.responseJSON.errors.Description}`, 'background: #222; color:red');
                alert(`Det verkar vara ett problem med kontakten till servern. URL: ${settings.url}`);
            });
    },
    ShowErrorMessage: function (message) {
        var errorMessage = $(".errorMessage");
        var html = `<b>${message}</b>`;
        errorMessage.empty();
        errorMessage.append(html);
        $('#calederEvent').modal('hide');
        $(".errorPanel").slideDown(500);
    }

};