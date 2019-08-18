
var ApiObject = ApiObject || {};

var LocalStorage = LocalStorage || {};

var FullCalenderVariables = {
    DeselectEventColor: null
};

var CalenderObject = {
    GetSelectedDaterange: function (rangeSettings) {
        var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData),
            outputHTML = "";

        var title = `<input type='text' placeholder='Skriv en titel' value='' class='form-control titleInput'/>`;
        var description = `<input type='text' placeholder='Skriv en beskrivning' value="" class='form-control descriptionInput'/>`;
        var start = `<input type='datetime-local' value="${rangeSettings.startEvent}" class='form-control startInput'/>`;
        var end = `<input type='datetime-local' value="${rangeSettings.endEvent}" class='form-control endInput'/>`;

        $.map(groups, function (val, index) {
            outputHTML += `<option value='${val.id}'>${val.name}</option>`;
        });

        CalenderObject.EmptyModalInputsAndText();

        $(".modal-title").append("Lägg till event");

        var settings = {
            eventIdAndGroupName: "",
            title: title,
            description: description,
            start: start,
            end: end,
            outputHTML: outputHTML
        };

        CalenderObject.RenderModalInputs(settings);

    },
    ChangeToThreeDayViewIfSmallScreen: function () {

        if (/Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            var _calender = $('#calender');
            var _calenderWrapper = $(".calenderWrapper");
            var _container = $(".container-fluid");
            var _accordion = $(".accordion");

            _container.css({ 'padding-right': '0px', 'padding-left': '0px' });

            _accordion.css({ 'margin': '15px auto auto auto' });

            _calenderWrapper.css({ 'margin': '5px 5px 5px 5px' });
            _calenderWrapper.css({ 'padding': '2px 2px 10px 2px' });

            _calender.fullCalendar('changeView', 'agendaThreeDay');

            var calHeight = $(window).height() * 0.83;
            _calender.fullCalendar('option', 'height', calHeight);
        }
    },
    InitiateCalender: function () {
        moment().locale("sv");

        $("#calender").fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month, agendaWeek,listWeek,agendaThreeDay'

            },
            height: 620,
            defaultView: "agendaWeek",
            slotLabelFormat: "HH:mm",
            slotEventOverlap: false,
            eventBackgroundColor: '#5B8005',
            lang: 'sv',
            firstDay: 1,
            locale: 'sv',
            timeFormat: 'H(:mm)',
            editable: false,
            selectable: true,
            select: function (start, end, jsEvent, view) {
                //https://fullcalendar.io/docs/v3/select-callback
                //alert('selected ' + moment(start).format("LLL") + ' to ' + moment(end).format("LLL"));

                var startEvent = moment(start).format();
                var endEvent = moment(end).format();

                var rangeSettings = {
                    startEvent: startEvent,
                    endEvent: endEvent
                };

                CalenderObject.GetSelectedDaterange(rangeSettings);

            },
            dayClick: function (date, jsEvent, view) {
                //alert('Clicked on: ' + moment(date).format("LLLL"));

                var startEvent = date.format();
                var endEvent = moment(date).add(30, 'minutes').format();

                var rangeSettings = {
                    startEvent: startEvent,
                    endEvent: endEvent
                };

                CalenderObject.GetSelectedDaterange(rangeSettings);

            },
            eventClick: function (calEvent, jsEvent, view) {
                var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData),
                    selectHtml = "";

                var eventIdAndGroupName = CalenderObject.SplitEventId(calEvent.id);

                console.log(calEvent);

                if (FullCalenderVariables.DeselectEventColor !== null) {
                    FullCalenderVariables.DeselectEventColor.css('background-color', '#5B8005');
                }

                jQuery(this).css('background-color', '#3498DB');

                FullCalenderVariables.DeselectEventColor = jQuery(this);

                CalenderObject.EmptyModalInputsAndText();

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

                var title = `<label>Titel</label><input type='text' value='${calEvent.title}' class='form-control titleInput'/>`;
                var description = `<label>Beskrivning</label><input type='text' placeholder='Skriv en beskrivning' value="${calEvent.description}" class='form-control descriptionInput'/>`;
                var start = `<label>Star</label><input type='datetime-local' value="${startEvent}" class='form-control startInput'/>`;
                var end = `<label>Slut</label><input type='datetime-local' value="${endEvent}" class='form-control endInput'/>`;

                $(".modal-title").append("Ändra event");

                var settings = {
                    eventIdAndGroupName: eventIdAndGroupName[0],
                    title: title,
                    description: description,
                    start: start,
                    end: end,
                    outputHTML: outputHTML
                };

                CalenderObject.RenderModalInputs(settings);
            },
            //validRange: {
            //    start: moment()
            //},
            columnFormat: 'ddd D MMM',
            eventRender: function (eventObj, $el) {
                $el.popover({
                    title: eventObj.title,
                    content: eventObj.description,
                    trigger: 'hover',
                    placement: 'top',
                    container: 'body'
                });
            },
            views: {
                agendaThreeDay: {
                    type: 'agenda',
                    duration: { days: 3 },
                    buttonText: '3 day'
                },
                defaultView: 'agendaThreeDay'
            }
        });
    },
    CheckForUserTokenAndReValidate: function () {

        var userData = LocalStorage.Get(LocalStorage.KeyToUserData);

        if (userData !== "0") {
            console.log(`%c Found key: ${LocalStorage.KeyToUserData}, userName is: ${userData.userName}, ${userData.userId}`, "background: #222; color:#bada55");

            var settings = {
                url: "/api/auth/revalidatetoken",
                method: "POST",
                mediaType: 'application/json',
                token: userData.token
            };

            $.when(ApiObject.RequestWithOutData(settings))

                .then(function (data, textStatus) {
                    if (textStatus === "success") {
                        if (data.isAuthenticated) {

                            $("#loginUserTitlePane").empty();
                            $("#loginUserTitlePane").append(userData.userName);
                            $(".loginPanel").removeClass("panel-default");
                            $(".loginPanel").addClass("panel-success");
                            $(".loginUser").slideUp(500);

                            CalenderObject.GetEvents();

                            console.log(data);
                            //Display message to user and propt for login again
                            console.log("%c successfully logged in", 'background: #222; color:#bada55');
                        } else if (!data.userExists) {

                            CalenderObject.UserMessages.Show("Felmeddelande", "&#197;tervalidering av dina uppgifter misslyckades. V&#228;nligen logga in igen.", "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);

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
    EmptyModalInputsAndText: function () {
        $(".modal-title").empty();
        $("#calederEvent .title").empty();
        $("#calederEvent .description").empty();
        $("#calederEvent .start").empty();
        $("#calederEvent .end").empty();
        $("#calederEvent #groupSelectionModal").empty();
        $("#calederEvent #eventId").empty();
    },
    RenderModalInputs: function (settings) {
        $("#calederEvent #eventId").attr('value', settings.eventIdAndGroupName);
        $("#calederEvent .title").append(settings.title);
        $("#calederEvent .description").append(settings.description);
        $("#calederEvent .start").append(settings.start);
        $("#calederEvent .end").append(settings.end);
        $("#calederEvent #groupSelectionModal").append(settings.outputHTML);

        $("#calederEvent").modal();
    },
    GetEvents: function () {

        var userData = LocalStorage.Get(LocalStorage.KeyToUserData);

        if (userData !== "0") {
            var settings = {
                url: "/api/calender/gettasksbyuserid",
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
                                "allDay": eventVal.allDay,
                                "description": eventVal.description,
                                "textColor": eventVal.eventTextColor,
                                "borderColor": "black",
                                "className": eventVal.eventColor
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
    GetAllUsersAndGroups: function () {

        //-->Save groups in localStorage<---
        var settings = {
            url: "/api/calender/getinfo",
            method: "GET",
            mediaType: 'application/json'
        };

        $.when(ApiObject.SimpleRequest(settings)).then(function (data) {

            var groups = data.groups,
                users = data.users,
                roles = data.roles;


            if (groups.length !== 'undefined' && users !== 'undefined' && roles !== 'undefined') {

                LocalStorage.Set(LocalStorage.KeyToGroupsData, groups);
                LocalStorage.Set(LocalStorage.KeyToListOfUsersData, users);
                LocalStorage.Set(LocalStorage.KeyToUserRolesData, roles);

                CalenderObject.AddGroupAndUserInputs();
            }

        });
    },
    AddGroupAndUserInputs: function () {

        var groups = LocalStorage.Get(LocalStorage.KeyToGroupsData);
        var users = LocalStorage.Get(LocalStorage.KeyToListOfUsersData);
        var roles = LocalStorage.Get(LocalStorage.KeyToUserRolesData);

        var groupsHtml = "",
            usersHtml = "",
            rolesHtml = "";

        //Groups
        if (groups !== "0") {
            $.map(groups, function (val, index) {
                groupsHtml += `<option value='${val.id}'>${val.name}</option>`;
            });
        } else {
            groupsHtml = "Inga grupper";
        }

        //Users
        if (users !== "0") {
            $.map(users, function (val, index) {
                usersHtml += `<option value='${val.id}'>${val.userName}</option>`;
            });
        } else {
            usersHtml = "Inga användare";
        }

        //Roles
        if (roles !== "0") {
            $.map(roles, function (val, index) {
                rolesHtml += `<option value='${val.role}'>${val.role}</option>`;
            });
        } else {
            usersHtml = "Inga användare";
        }

        $("#groupSelection").empty().append(groupsHtml).slideDown();
        $("#assignUserInput").empty().append(usersHtml);
        $("#assignGroupInput").empty().append(groupsHtml);
        $("#removeUserInput").empty().append(usersHtml);
        $("#removeGroupInput").empty().append(groupsHtml);
        $("#addNewGroupUsers").empty().append(usersHtml);
        $("#removeGroupSelect").empty().append(groupsHtml);
        $("#userRoles").empty().append(rolesHtml);
        $("#addNewGroupUsers, #removeGroupSelect").select2({
            width: 'resolve'
        });

    },
    SplitEventId: function (eventId) {
        var eventIdArray = eventId.split(";");
        return eventIdArray;
    },
    AddOrUpdateCalanderEvent: function () {

        $("#addEvent").on('click', function (e) {
            e.preventDefault();

            var title = "",
                description = "",
                start = "",
                end = "",
                groupId = "",
                eventId = "",
                eventColor,
                eventTextColor = "";
            settings = {};

            title = $(".titleInput");
            description = $(".descriptionInput");
            start = $(".startInput");
            end = $(".endInput");
            groupId = $("#groupSelectionModal");
            eventId = $("#eventId");
            eventColor = $("#eventColor");
            eventTextColor = $("#eventTetxColor");

            if (CalenderObject.CheckEmptyInput(title) && CalenderObject.CheckEmptyInput(description)) {
                var userData = LocalStorage.Get(LocalStorage.KeyToUserData);
                if (userData !== "0") {

                    var calenderData = {
                        userId: userData.userId,
                        userName: userData.userName,
                        title: title.val(),
                        description: description.val(),
                        start: start.val(),
                        end: end.val(),
                        eventColor: eventColor.val(),
                        eventTextColor: eventTextColor.val(),
                        groupId: groupId.val()
                    };
                    if (!CalenderObject.CheckEmptyInput(eventId)) {
                        settings = {
                            url: "/api/calender/addevent",
                            method: "POST",
                            data: JSON.stringify(calenderData),
                            mediaType: 'application/json',
                            token: userData.token
                        };
                    } else {
                        calenderData.eventId = eventId.val();
                        settings = {
                            url: "/api/calender/updateevent",
                            method: "PUT",
                            data: JSON.stringify(calenderData),
                            mediaType: 'application/json',
                            token: userData.token
                        };
                    }



                    $.when(ApiObject.Request(settings))
                        .then(function (data) {
                            var calenderResponse = JSON.parse(data);
                            if (calenderResponse.statusCode === 200) {

                                CalenderObject.GetEvents();
                                $('#calederEvent').modal('hide');

                            } else if (calenderResponse.statusCode === 422) {
                                $('#calederEvent').modal('hide');
                                CalenderObject.UserMessages.Show("Felmeddelande", calenderResponse.description, "panel-danger");
                                CalenderObject.UserMessages.Hide(6000);
                            }
                        });
                } else {
                    $('#calederEvent').modal('hide');
                    CalenderObject.UserMessages.Show("Meddelande", "V&#228;nligen logga in f&#246;r att skapa ett event.", "panel-info");
                    CalenderObject.UserMessages.Hide(6000);
                }
            }
        });
    },
    AddNewGroup: function () {
        $("#addNewGroupButton").on('click', function () {
            var groupName = "",
                groupUsers = [];

            groupUsers = $("#addNewGroupUsers").val();
            groupName = $("#newGroupName");
            if (CalenderObject.CheckEmptyInput(groupName) && CalenderObject.CheckIfArrayIsEmpty(groupUsers)) {
                var userData = LocalStorage.Get(LocalStorage.KeyToUserData);
                if (userData !== "0") {

                    var createGroupData = {
                        groupName: groupName.val(),
                        groupUsers: groupUsers
                    };
                    var settings = {    
                        url: "/api/calender/addgroup",
                        method: "POST",
                        data: JSON.stringify(createGroupData),
                        mediaType: 'application/json',
                        token: userData.token
                    };

                    $.when(ApiObject.Request(settings)).then(function (calenderResponse) {
                        //var calenderResponse = JSON.parse(data);
                        if (calenderResponse.statusCode === 200) {

                            CalenderObject.UserMessages.Show("Meddelande", calenderResponse.description, "panel-info");
                            CalenderObject.UserMessages.Hide(6000);
                            CalenderObject.GetAllUsersAndGroups();

                        } else if (calenderResponse.statusCode === 422) {

                            CalenderObject.UserMessages.Show("Felmeddelande", calenderResponse.description, "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);

                        } else if (calenderResponse.statusCode === 409) {

                            CalenderObject.UserMessages.Show("Felmeddelande", calenderResponse.description, "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);
                        }
                    });

                } else {
                    CalenderObject.UserMessages.Show("Meddelande", "V&#228;nligen logga in f&#246;r att lägga till en grupp.", "panel-info");
                    CalenderObject.UserMessages.Hide(6000);
                }


            } else {
                CalenderObject.UserMessages.Show("Felmeddelande", "V&#228;nligen ange ett gruppnamn eller välj minst en användare.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            }
        });
    },
    Deletegroup: function () {

        $("#removeGroupButton").on('click', function () {

            var groups = $("#removeGroupSelect").val();

            var deleteGroupData = {
                groups: groups
            };

            var userData = LocalStorage.Get(LocalStorage.KeyToUserData);
            var settings = {
                url: "/api/calender/deletegroup",
                method: "DELETE",
                data: JSON.stringify(deleteGroupData),
                mediaType: 'application/json',
                token: userData.token
            };

            if (CalenderObject.CheckIfArrayIsEmpty(groups)) {
                if (userData !== "0") {

                    $.when(ApiObject.Request(settings)).then(function (calenderResponse) {
                        if (calenderResponse.statusCode === 200) {

                            CalenderObject.UserMessages.Show("Meddelande", calenderResponse.description, "panel-info");
                            CalenderObject.UserMessages.Hide(6000);
                            CalenderObject.GetAllUsersAndGroups();
                            CalenderObject.GetEvents();

                        } else if (calenderResponse.statusCode === 422) {

                            CalenderObject.UserMessages.Show("Felmeddelande", calenderResponse.description, "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);

                        } else if (calenderResponse.statusCode === 404) {

                            CalenderObject.UserMessages.Show("Felmeddelande", calenderResponse.description, "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);

                        }
                    });

                } else {
                    CalenderObject.UserMessages.Show("Meddelande", "V&#228;nligen logga in f&#246;r att lägga till en grupp.", "panel-info");
                    CalenderObject.UserMessages.Hide(6000);
                }
            } else {
                CalenderObject.UserMessages.Show("Felmeddelande", "V&#228;nligen välj minst en användare.", "panel-danger");
                CalenderObject.UserMessages.Hide(6000);
            }


        });

    },
    CheckIfArrayIsEmpty: function (arr) {
        if (!Array.isArray(arr)) {
            return false;
        }
        if (arr.length > 0) {
            return true;
        } else {
            return false;
        }
    },
    CheckEmptyInput: function (input) {
        if (input.val() === "") {
            input.focus();
            return false;
        }

        return true;
    },
    DeleteEvent: function () {

        $("#deleteEvent").on('click', function (e) {
            e.preventDefault();

            var eventId = $("#calederEvent #eventId"),
                userData = LocalStorage.Get(LocalStorage.KeyToUserData),
                groupId = $("#groupSelectionModal");

            if (userData !== "0") {

                var calenderData = {
                    eventId: eventId.val(),
                    groupId: groupId.val()
                };

                var settings = {
                    url: "/api/calender/deleteevent",
                    method: "DELETE",
                    data: JSON.stringify(calenderData),
                    mediaType: 'application/json',
                    token: userData.token
                };

                $.when(ApiObject.Request(settings))
                    .then(function (data) {
                        var calenderResponse = JSON.parse(data);
                        if (calenderResponse.statusCode === 200) {

                            CalenderObject.GetEvents();
                            $('#calederEvent').modal('hide');

                        }
                    });
            }


        });

    },
    UserMessages: {
        Show: function (title, message, addClass) {
            var userMessagePanel = $(".userMessagePanel");
            var messageBody = $(".messageBody");
            var messageBodyPanel = $(".messageBodyPanel");
            var userMessageTitle = $(".userMessageTitle");
            var panelInfo = "panel-info";
            var pandelDanger = "panel-danger";

            if (addClass === panelInfo || !userMessagePanel.hasClass(addClass)) {
                userMessagePanel.removeClass(pandelDanger);
                userMessagePanel.addClass(addClass);
            }

            userMessageTitle.empty();
            messageBody.empty();

            var html = `<b>${message}</b>`;
            messageBody.append(html);
            userMessageTitle.append(title);

            userMessagePanel.slideDown(500);
            //messageBodyPanel.slideDown(500);

        },
        Hide: function (interval) {
            setTimeout(function () {
                var userMessagePanel = $(".userMessagePanel"),
                    messageBodyPanel = $(".messageBodyPanel"),
                    messageBody = $(".messageBody"),
                    userMessageTitle = $(".userMessageTitle");

                //messageBodyPanel.slideUp(500);
                userMessagePanel.slideUp(500, function () {
                    messageBody.empty();
                    userMessageTitle.empty();
                });




            }, interval);

        }
    },
    LinkUserAndGroup: function () {
        $("#assignUserToGroupBtn").on('click', function (e) {
            e.preventDefault();

            var userId = $("#assignUserInput").val(),
                groupId = $("#assignGroupInput").val();
            if (groupId !== '' && userId) {
                var calenderData = { userId: userId, groupId: groupId };
                var userData = LocalStorage.Get(LocalStorage.KeyToUserData);
                var settings = {
                    url: "/api/calender/addusertogroup",
                    method: "POST",
                    data: JSON.stringify(calenderData),
                    mediaType: 'application/json',
                    token: userData.token
                };

                $.when(ApiObject.Request(settings)).then(function (data) {
                    if (data.statusCode === 200) {
                        CalenderObject.UserMessages.Show("Meddelande", data.description, "panel-info");
                        CalenderObject.GetEvents();
                    } else if (data.statusCode === 400) {
                        CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                    } else if (data.statusCode === 404) {
                        CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                    }
                    CalenderObject.UserMessages.Hide(6000);

                })
                    .fail(function (jqXHR) {
                        if (jqXHR.status === 401) {
                            CalenderObject.UserMessages.Show("Felmeddelande", "Ett fel inträffade när när servern skulle anropas.", "panel-danger");
                            CalenderObject.UserMessages.Hide(6000);
                        }
                    });;
            }
        });
    },
    RemoveUserFromGroup: function () {
        $("#removeUserToGroupBtn").on('click', function (e) {
            e.preventDefault();

            var userId = $("#removeUserInput").val(),
                groupId = $("#removeGroupInput").val();

            if (groupId !== '' && userId) {

                var calenderData = { userId: userId, groupId: groupId };

                var userData = LocalStorage.Get(LocalStorage.KeyToUserData);

                var settings = {
                    url: "/api/calender/removeuserfromgroup",
                    method: "DELETE",
                    data: JSON.stringify(calenderData),
                    mediaType: 'application/json',
                    token: userData.token
                };

                $.when(ApiObject.Request(settings)).then(function (data) {

                    if (data.statusCode === 200) {
                        CalenderObject.UserMessages.Show("Meddelande", data.description, "panel-info");
                        CalenderObject.GetEvents();
                    } else if (data.statusCode === 400) {
                        CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                    } else if (data.statusCode === 404) {
                        CalenderObject.UserMessages.Show("Felmeddelande", data.description, "panel-danger");
                    }
                    CalenderObject.UserMessages.Hide(6000);

                }).fail(function (jqXHR) {
                    if (jqXHR.status === 401) {
                        CalenderObject.UserMessages.Show("Felmeddelande", "Ett fel inträffade när när servern skulle anropas.", "panel-danger");
                        CalenderObject.UserMessages.Hide(6000);
                    }
                });
            }
        });
    }
};