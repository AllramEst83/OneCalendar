
var CalenderObject = CalenderObject || {};

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
            },
            statusCode: {
                403: function () {
                    CalenderObject.UserMessages.Show("Felmeddelande", "You have to be an admin to make this action.", "panel-danger");
                    CalenderObject.UserMessages.Hide(6000);
                },
                401: function () {
                    CalenderObject.UserMessages.Show("Felmeddelande", "You are UNAUTHORIZED to make this action.", "panel-danger");
                    CalenderObject.UserMessages.Hide(6000);
                },
                500: function () {
                    CalenderObject.UserMessages.Show("Meddelande", "A server error has occured. Please reload page.", "panel-info");
                    CalenderObject.UserMessages.Hide(6000);
                }
            }
        })
            .done(function (data, textStatus) {

                console.log(`%c Request success: ${data}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR}`, 'background: #222; color:red');
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
            },
            403: function () {
                CalenderObject.UserMessages.Show("Felmeddelande", "You have to be an admin to make this action.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            },
            401: function () {
                CalenderObject.UserMessages.Show("Felmeddelande", "You are UNAUTHORIZED to make this action.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            },
            500: function(){
                CalenderObject.UserMessages.Show("Meddelande", "A server error has occured. Please reload page.", "panel-info");
                CalenderObject.UserMessages.Hide(6000);
            },
            400: function () {
                CalenderObject.UserMessages.Show("Meddelande", "Unable to preform action. Please check input and try again.", "panel-info");
                CalenderObject.UserMessages.Hide(6000);
            }
        })
            .done(function (data, textStatus) {
                console.log(`%c Request success: ${data}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR}`, 'background: #222; color:red');
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
            },
            403: function () {
                CalenderObject.UserMessages.Show("Felmeddelande", "You have to be an admin to make this action.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            },
            401: function () {
                CalenderObject.UserMessages.Show("Felmeddelande", "You are UNAUTHORIZED to make this action.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            },
            500: function () {
                CalenderObject.UserMessages.Show("Meddelande", "A server error has occured. Please reload page.", "panel-info");
                CalenderObject.UserMessages.Hide(6000);
            },
            400: function (error) {
                CalenderObject.UserMessages.Show("Meddelande", "Unable to preform action. Please check input and try again.", "panel-info");
                CalenderObject.UserMessages.Hide(6000);
            }
        })
            .done(function (data, textStatus) {

                //var userData = JSON.parse(data);
                console.log(`%c Request success: ${data.statusCode} & ${data.description}`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {

                console.log(`%c request failed: ${jqXHR.responseJSON.title}`, 'background: #222; color:red');
                CalenderObject.UserMessages.Show("Meddelande", `${jqXHR.responseJSON.title} - ${jqXHR.responseJSON.errors.userMail}` , "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
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
                //var userData = JSON.parse(data);
                console.log(`%c Request success`, 'background: #222; color:#bada55');
            })
            .fail(function (jqXHR, textStatus) {
                console.log(`%c request failed: ${jqXHR}`, 'background: #222; color:red');;
            });
    },
};