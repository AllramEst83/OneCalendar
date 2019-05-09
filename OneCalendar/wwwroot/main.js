
var CalenderObject = CalenderObject || {};
var FormObject = FormObject || {};
var LocalStorage = LocalStorage || {};
var Accordion = Accordion || {};

$(document).ready(function () {


    console.log("ready!");

    CalenderObject.CheckForUserTokenAndReValidate();
    CalenderObject.InitiateCalender();
    CalenderObject.GetEvents();
    FormObject.LogInBinding();
    CalenderObject.LinkUserAndGroup();
    FormObject.Logout();
    CalenderObject.GetAllUsersAndGroups();
    CalenderObject.RemoveUserFromGroup();   
    FormObject.AddUser();
    Accordion.BidnAccordion();
    CalenderObject.AddOrUpdateCalanderEvent();
    CalenderObject.DeleteEvent();
    //LocalStorage.DeleteStorageOnBrowserClose();

});