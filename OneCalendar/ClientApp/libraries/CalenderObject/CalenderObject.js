
var ApiObject = ApiObject || {};

var CalenderObject = {

    InitiateCalender: function () {
        console.log(moment().locale("sv"));

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
    GetEvents: function () {
        var url = "https://localhost:44305/api/calender/gettasksbyuserid";
        var id = 2;
        $.when(
            ApiObject.GetEventsByUserId(url, id))
            .then(function (data) {

                var test = {
                    "id": 'a',
                    "title":"test title",
                    "allDay": false,
                    "start": '2019-04-25T09:00:00Z',
                    "end": '2019-04-25T15:00:00Z'
                };

                $("#calender").fullCalendar('renderEvent', test);
            });
    }
};