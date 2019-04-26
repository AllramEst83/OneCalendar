
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
            slotLabelFormat: "HH:mm",
            slotEventOverlap: false,
            eventBackgroundColor: 'brown',
            lang: 'sv',
            firstDay: 1,
            locale: 'sv',
            timeFormat: 'H(:mm)',

            eventClick: function (calEvent, jsEvent, view) {
                jQuery(this).css('background-color', '#b3ffb3');
            }
        });
    },
    CheckForUserTokenAndReValidate: function () {

        var userData = LocalStorage.Get(LocalStorage.LocalStorageKey);


        if (userData !== "0") {
            console.log(`%c Found key: ${LocalStorage.LocalStorageKey}, userName is: ${userData.userName}, ${userData.userId}`, "background: #222; color:#bada55");

            var settings = {
                url: "https://localhost:44305/api/auth/revalidatetoken",
                method: "POST",
                mediaType: 'application/json',
                token: userData.token
            };

            $.when(ApiObject.RequestWithOutData(settings))

                .then(function (data, textStatus) {
                    if (textStatus === "success") {
                        if (data.isAuthenticated) {

                            $("#titlePane").empty();
                            $("#titlePane").append(userData.userName);
                            $(".panel-body").slideUp(500);

                            console.log(data);
                            //Display message to user and propt for login again
                            console.log("%c successfully logged in", 'background: #222; color:#bada55');
                        } else {
                            alert("Token invalid please login again");
                        }
                    } else {
                        console.log("The revalidate request is not a success");
                    }
                });

        } else {
            //Show login?
            console.log(`%c No key found`, "background: #222; color:#bada55");
            console.log("%c Show login screen?", "background: #222; color:#bada55");

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

                var test = [{
                    "id": 'a',
                    "title": "test title",
                    "allDay": false,
                    "start": '2019-04-25T09:00:00Z',
                    "end": '2019-04-25T15:00:00Z'
                }, {
                    "id": 'b',
                    "title": "test title",
                    "allDay": false,
                    "start": '2019-04-24T09:00:00Z',
                    "end": '2019-04-24T15:00:00Z'
                }];

                $('#calender').fullCalendar('removeEvents');
                $('#calender').fullCalendar('addEventSource', test);
                $('#calender').fullCalendar('rerenderEvents');
            });
    }
};