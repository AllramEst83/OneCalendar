
var ApiObject = ApiObject || {};

var LocalStorage = LocalStorage || {};

var CalenderObject = {

    InitiateCalender: function () {
        moment().locale("sv");
        console.log("moment set to 'sv'-> " + moment().format("YYYY-MM-DD HH:MM"));

        $("#calender").fullCalendar({
            header: {
                left: 'title',
                center: 'today, month, agendaWeek,listWeek',
                right: ' prev,next'
            },
            defaultView: "agendaWeek",
            slotLabelFormat: "HH:mm"
        });
    },
    CheckForUserTokenAndReValidate: function () {
        console.log(1);
        var userData = LocalStorage.KeyStartsWith(LocalStorage.KeyStart);

        if (userData !== "0") {

            var settings = {
                url: "https://localhost:44305/api/auth/revalidatetoken",
                method: "POST",
                mediaType: 'application/json',
                token: userData.token,
            };

            $.when(ApiObject.RequestWithOutData(settings))

                .then(function (data, textStatus) {
                    if (textStatus === "success") {

                        LocalStorage.Set(LocalStorage.KeyStart + data.userName, data.token);

                        $("#titlePane").empty();
                        $("#titlePane").append(userData.userName);
                        $(".panel-body").slideUp(500);

                        console.log(data);
                        //Display message to user and propt for login again
                        console.log("%c successfully logged in", 'background: #222; color:#bada55');

                    } else {
                        console.log("%c If not successfull revalidation. Show login", 'background: #222; color:#bada55');

                    }
                });

        } else {
            //Show login?
            console.log("Show login screen?");
        }

    },
    GetEvents: function (userToken) {
        var settings = {
            url: "https://localhost:44305/api/calender/gettasksbyuserid",
            method: "GET",
            data: { id: userToken },
            mediaType: 'application/json'
        };

        $.when(
            ApiObject.RequestWithOutAuth(settings))
            .then(function (data) {

                var test = {
                    "id": 'a',
                    "title": "test title",
                    "allDay": false,
                    "start": '2019-04-25T09:00:00Z',
                    "end": '2019-04-25T15:00:00Z'
                };

                $("#calender").fullCalendar('renderEvent', test);
            });
    }
};