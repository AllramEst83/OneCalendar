﻿
var ApiObject = ApiObject || {};

var LocalStorage = LocalStorage || {};

var FullCalenderVariables = {
    DeselectEventColor: null
};

var CalenderObject = {
    InitiateCalender: function () {
        moment().locale("sv");

        $("#calender").fullCalendar({
            header: {
                left: 'title',
                center: 'today, month, agendaWeek,listWeek',
                right: ' prev,next'
            },
            defaultView: "agendaWeek",
            slotLabelFormat: "HH:mm",
            slotEventOverlap: false,
            eventBackgroundColor: '#5B8005',
            lang: 'sv',
            firstDay: 1,
            locale: 'sv',
            timeFormat: 'H(:mm)',
            dayClick: function (date, jsEvent, view) {
                var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData),
                    selectHtml = "";
                //alert('Clicked on: ' + moment(date).format("LLLL"));

                var startEvent = date.format();
                var endEvent = moment(date).add(30, 'minutes').format();

                var title = `<input type='text' placeholder='Skriv en titel' value='' class='form-control titleInput'/>`;
                var start = `<input type='datetime-local' value="${startEvent}" class='form-control startInput'/>`;
                var end = `<input type='datetime-local' value="${endEvent}" class='form-control endInput'/>`;

                $.map(groups, function (val, index) {
                    selectHtml += `<option value='${val.id}'>${val.name}</option>`;
                });

                $(".modal-title").empty();
                $("#calederEvent .title").empty();
                $("#calederEvent .start").empty();
                $("#calederEvent .end").empty();
                $("#calederEvent #groupSelectionModal").empty();

                $(".modal-title").append("Lägg till event");
                $("#calederEvent .title").append(title);
                $("#calederEvent .start").append(start);
                $("#calederEvent .end").append(end);
                $("#calederEvent #groupSelectionModal").append(selectHtml);


                $("#calederEvent").modal();
            },
            eventClick: function (calEvent, jsEvent, view) {
                var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData),
                    selectHtml = "";

                var eventIdAndGroupName = CalenderObject.SplitEventId(calEvent.id);

                console.log(calEvent);

                //alert('Event: ' + calEvent.title);

                if (FullCalenderVariables.DeselectEventColor !== null) {
                    FullCalenderVariables.DeselectEventColor.css('background-color', '#5B8005');
                }

                jQuery(this).css('background-color', '#3498DB');

                FullCalenderVariables.DeselectEventColor = jQuery(this);

                $(".modal-title").empty();
                $("#calederEvent .title").empty();
                $("#calederEvent .start").empty();
                $("#calederEvent .end").empty();
                $("#calederEvent #groupSelectionModal").empty();

                var startEvent = calEvent.start.format();
                var endEvent = calEvent.end.format();



                if (groups !== "0") {
                    var userGroupName = "";
                    $.map(groups, function (val, index) {
                        if (val.name === eventIdAndGroupName[1]) {
                            userGroupName = `<option value='${val.id}'>${val.name}</option>`;
                        } else {
                            selectHtml += `<option value='${val.id}'>${val.name}</option>`;
                        }

                    });
                    var outputHTML = userGroupName + selectHtml;
                }

                var title = `<input type='text' value='${calEvent.title}' class='form-control titleInput'/>`;
                var start = `<input type='datetime-local' value="${startEvent}" class='form-control startInput'/>`;
                var end = `<input type='datetime-local' value="${endEvent}" class='form-control endInput'/>`;

                $(".modal-title").append("Ändra event");

                $("#calederEvent .title").append(title);
                $("#calederEvent .start").append(start);
                $("#calederEvent .end").append(end);

                $("#calederEvent #groupSelectionModal").append(outputHTML);

                $("#calederEvent").modal();

            },
            editable: true,
            selectable: true,
            validRange: {
                start: moment()
            },
            columnFormat: 'ddd D MMM'
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
    GetEvents: function () {

        var userData = LocalStorage.Get(LocalStorage.LocalStorageKey);

        if (userData !== "0") {
            var settings = {
                url: "https://localhost:44305/api/calender/gettasksbyuserid",
                method: "GET",
                data: { id: userData.userId },
                mediaType: 'application/json',
                token: userData.token
            };

            $.when(
                ApiObject.Request(settings))
                .then(function (data) {

                    var taskData = JSON.parse(data);

                    var allEvents = [];

                    $.map(taskData, function (val) {
                        $.map(val.events, function (eventVal) {
                            allEvents.push({
                                "id": `${eventVal.id};${val.groupName}`,
                                "title": eventVal.title,
                                "start": eventVal.start,
                                "end": eventVal.end,
                                "allDay": eventVal.allDay
                            });
                        });
                    });

                    LocalStorage.Set(LocalStorage.KeyToTaskData, taskData);

                    console.log("%c Events linked to logged in user has been fetched and rendered", 'background: #222; color:#bada55');

                    $('#calender').fullCalendar('removeEvents');
                    $('#calender').fullCalendar('addEventSource', allEvents);
                    //$('#calender').fullCalendar('rerenderEvents');
                });
        }

    },
    GetAllGroups: function () {

        //-->Save groups in localStorage<---
        var settings = {
            url: "https://localhost:44305/api/calender/getallgroups",
            method: "GET",
            mediaType: 'application/json'
        };

        $.when(ApiObject.SimpleRequest(settings)).then(function (data) {

            var groups = JSON.parse(data);

            if (groups.length > 0) {

                LocalStorage.Set(LocalStorage.KeyToGroupsData, groups);

                CalenderObject.AddGroupInputToForm();
            }

        });
    },
    AddGroupInputToForm: function () {

        var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData);

        var html = "";
        if (groups !== "0") {

            $.map(groups, function (val, index) {
                html += `<option value='${val.id}'>${val.name}</option>`;
            });

            $("#groupSelection").empty().append(html).slideDown();
        }

    },
    SplitEventId: function (eventId) {
        var eventIdArray = eventId.split(";");
        return eventIdArray;
    },
    AddCalanderEvent: function () {

        $("#addEvent").on('click', function () {
            var title = "",
                start = "",
                end = "",
                groupId = "";

            title = $(".titleInput");
            start = $(".startInput");
            end = $(".endInput");
            groupId = $("#groupSelectionModal");

            if (CalenderObject.CheckEmptyInput(title)) {
                var userData = LocalStorage.Get(LocalStorage.LocalStorageKey);
                if (userData !== "0") {

                    var calenderData = {
                        userId: userData.userId,
                        userName: userData.userName,
                        title: title.val(),
                        start: start.val(),
                        end: end.val(),
                        groupId: groupId.val()
                    };

                    var settings = {
                        url: "https://localhost:44305/api/calender/addevent",
                        method: "POST",
                        data: JSON.stringify(calenderData),
                        mediaType: 'application/json',
                        token: userData.token
                    };

                    $.when(ApiObject.Request(settings))
                        .then(function (data) {
                            var calenderData = JSON.parse(data);
                            if (calenderData.statusCode === 200) {

                                CalenderObject.GetEvents();
                                $('#calederEvent').modal('hide');

                            }
                        });
                }
            }
        });
    },
    CheckEmptyInput: function (input) {
        if (input.val() === "") {
            input.focus();
            return false;
        }

        return true;
    }
};