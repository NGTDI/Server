var watchID = null;
var positionDateTime;
var hasFix;
var m_PositionAverager;
var authToken;
var displayName;
var userName;
var siteTagCount;
var siteUserCount;
var siteHydrantCount;
var Tags;
var m_ItemToDelete;

function menuselect(event, ui) {
    alert("Hello");
}

function save() {

}

function GetType() {
    if ($('#radAllYear').prop('checked')) {
        return "allyear";
    } else if ($('#radBefore').prop('checked')) {
        return "before";
    } else if ($('#radAfter').prop('checked')) {
        return "after";
    } else if ($('#radBetween').prop('checked')) {
        return "between";
    } else if ($('#radOn').prop('checked')) {
        return "on";
    }

    return "";
}

function register() {
    $("#lblStatus").removeClass("bg-success");
    $("#lblStatus").removeClass("bg-danger");
    
    var username = $("#txtUsername").val();
    var email = $("#txtEmailAddress").val();
    var password1 = $("#txtPassword").val();
    var password2 = $("#txtPassword2").val();

    if (password1 == password2) {
        $.ajax({
            type: "POST",
            url: "/register",
            headers: { "Username": username, "Password": password1, "Email": email },
            success: function(response) {
                if (response.Result == "Success") {
                    window.alert("Please check your email to verify your account.");

                    window.location.replace("/login");
                } else if (response.Result == "UsernameNotAvailable") {
                    $("#lblStatus").text("The username is not available.");
                    $("#lblStatus").addClass("bg-danger");
                }
            },
            error: RegisterFailure,
            cache: false,
            contentType: false,
            processData: false
        });
    } else {
        $("#lblStatus").text("Passwords do not match.");
        $("#lblStatus").addClass("bg-danger");
    }
}

function TypeChangeNoDefault(type) {
    $('#lblStartDate').text('Start Date');
    $('#dateStart').prop('disabled', false);
    $('#dateEnd').prop('disabled', false);
    
    if (type == 'allyear') {
        $('#rowStart').show();
        $('#rowEnd').show();

        $('#dateStart').prop('disabled', true);
        $('#dateEnd').prop('disabled', true);
    } else if (type == 'before') {
        $('#rowStart').hide();
        $('#rowEnd').show();
    } else if (type == 'after') {
        $('#rowStart').show();
        $('#rowEnd').hide();
    } else if (type == 'between') {
        $('#rowStart').show();
        $('#rowEnd').show();
    } else if (type == 'on') {
        $('#lblStartDate').text('On');
        $('#rowStart').show();
        $('#rowEnd').hide();
    }
}

function TypeChange(type) {
    $('#lblStartDate').text('Start Date');
    $('#dateStart').prop('disabled', false);
    $('#dateEnd').prop('disabled', false);
    
    if (type == 'allyear') {
        $('#rowStart').show();
        $('#rowEnd').show();

        var dStart;
        var dEnd;
        var dNow = new Date();
        var iMonth = dNow.getMonth();
        var iYear = dNow.getFullYear();
        
        if (iMonth >= 11) {
            dStart = new Date(iYear + 1, 0, 1);
            dEnd = new Date(iYear + 1, 11, 31);
        } else {
            dStart = new Date(iYear, 0, 1);
            dEnd = new Date(iYear, 11, 31);
        }

        $('#dateStart').val(dStart.toLocaleDateString());
        $('#dateStart').prop('disabled', true);

        $('#dateEnd').val(dEnd.toLocaleDateString());
        $('#dateEnd').prop('disabled', true);
    } else if (type == 'before') {
        $('#rowStart').hide();
        $('#dateStart').val(null);

        $('#rowEnd').show();
        $('#dateEnd').val(null);
    } else if (type == 'after') {
        $('#rowStart').show();
        $('#dateStart').val(null);

        $('#rowEnd').hide();
        $('#dateEnd').val(null);
    } else if (type == 'between') {
        $('#rowStart').show();
        $('#dateStart').val(null);

        $('#rowEnd').show();
        $('#dateEnd').val(null);
    } else if (type == 'on') {
        $('#lblStartDate').text('On');

        $('#rowStart').show();
        $('#dateStart').val(null);

        $('#rowEnd').hide();
        $('#dateEnd').val(null);
    }
}

function RegisterFailure() {
    $("#lblStatus").text("An error has occurred.");
    $("#lblStatus").addClass("bg-danger");
}

function ShowEula(response) {
    $("#EulaText").text(response.EulaText);
    $("#hidEulaGuid").val(response.EulaGuid);

    $('#modalEula').modal({ show: true });
}

function EulaAcknowledged() {
    var guid = $("#hidEulaGuid").val();
    var username = localStorage.userName;
    var authToken = localStorage.authToken;
    
    $.ajax({
        type: "POST",
        url: "/signeula/" + guid,
        headers: { "Username": username, "AuthorizationToken": authToken },
        success: SignEulaSuccess,
        error: SignEulaFailure,
        cache: false,
        contentType: false,
        processData: false
    });

    $('#modalEula').modal({ show: false });
}

function SignEulaSuccess(response) {
    window.location.replace("/home");
}

function SignEulaFailure(response) {

}

function EulaDeclined() {
    $('#modalEula').modal('hide');
    LogoutAndClear();
}

function login() {
    var username = $("#txtUsername").val();
    var password = $("#txtPassword").val();
    
    $("#lblStatus").removeClass("bg-success");
    $("#lblStatus").removeClass("bg-danger");

    $.ajax({
        type: "POST",
        url: "/login",
        headers: { "Username": username, "Password": password },
        success: function (response) {
            if (response.Result == "Success") {
                //Cache user info
                authToken = response.AuthorizationToken;
                userName = response.UserName;
                displayName = response.DisplayName;
                WriteToLocalStorage();

                if (response.NeedsEula == "True") {
                    ShowEula(response);
                } else {
                    window.location.replace("/home");
                }
            }
            else if (response.Result == "NotActive") {
                $("#lblStatus").text("User has been deactivated.");
                $("#lblStatus").addClass("bg-danger");
            }
            else if (response.Result == "NotVerified") {
                $("#lblStatus").text("Please verify the email address of your account.");
                $("#lblStatus").addClass("bg-danger");
            }
            else {
                $("#lblStatus").text("Bad user or password combination.");
                $("#lblStatus").addClass("bg-danger");
            }
        },
        error: LoginFailure,
        cache: false,
        contentType: false,
        processData: false
    });
}

function ChangePassword() {
    $("#lblStatus").removeClass("bg-success");
    $("#lblStatus").removeClass("bg-danger");

    var username = localStorage.userName;
    var authToken = localStorage.authToken;
    var current = $("#txtCurrentPassword").val();
    var new1 = $("#txtNewPassword").val();
    var new2 = $("#txtNewPasswordRepeat").val();

    if (new1 == new2) {
        var formData = new FormData();
        formData.append('currentpassword', current);
        formData.append('newpassword', new1);

        $.ajax({
            type: "POST",
            url: "/changepassword",
            headers: { "Username": username, "AuthorizationToken": authToken },
            data: formData,
            success: function(response) {
                if (response.Result == "Success") {
                    $("#lblStatus").addClass("bg-success");
                    $("#lblStatus").text("Your password has been changed.");
                } else {
                    $("#lblStatus").addClass("bg-danger");
                    $("#lblStatus").text("Your current password wasn't correct.");
                }
            },
            error: ResetFailure,
            cache: false,
            contentType: false,
            processData: false
        });
    } else {
        $("#lblStatus").text("The new passwords do not match.");
        $("#lblStatus").addClass("bg-danger");
    }
}

function ResetPassword() {
    var email = $("#txtEmailAddress").val();

    $.ajax({
        type: "POST",
        url: "/reset",
        headers: { "EmailAddress": email },
        success: function (response) {
            if (response.Result == "Success") {
                alert("If the email is in our system, a password reset email has been sent.");

                window.location.replace("/home");
            }
        },
        error: ResetFailure,
        cache: false,
        contentType: false,
        processData: false
    });
}

function ResetFailure() {
    alert("An error occurred.");
}

function GetAntiResolutions() {
    var username = localStorage.userName;
    var authToken = localStorage.authToken;

    $.ajax({
        type: "GET",
        url: "/getantiresolutions",
        headers: { "Username": username, "AuthorizationToken": authToken },
        success: GetAntiResolutionsReceived,
        error: GetAntiResolutionsFailure,
        cache: false,
        contentType: false,
        processData: false
    });
}

function GetAntiResolutionsFailure() {
    
}

function GetAntiResolutionsReceived(response) {
    $('#ars_table').dataTable({
        data: response.Data,
        columns: [{ "data": "EditButton", "width": "10%" },
                  { "data": "DeleteButton", "width": "10%" },
                  { "data": "Period", "width": "10%" },
                  { "data": "StartDate", "width": "15%" },
                  { "data": "EndDate", "width": "15%" },
                  { "data": "Text", "width": "40%"}]
    });
}

function DeleteAntiResolution(guid) {
    m_ItemToDelete = guid;

    $('#modalValidateDelete').modal({ show: true });
}

function DeleteAcknowledged() {
    $('#modalValidateDelete').modal('hide');

    var username = localStorage.userName;
    var authToken = localStorage.authToken;
    
    $.ajax({
        type: "GET",
        url: "/deleteantiresolution/" + m_ItemToDelete,
        headers: { "Username": username,
            "AuthorizationToken": authToken},
        success: DeleteAntiResolutionsReceived,
        error: DeleteAntiResolutionsFailure,
        cache: false,
        contentType: false,
        processData: false
    });
}

function DeleteAntiResolutionsReceived() {
    window.location.replace("/home");
}

function DeleteAntiResolutionsFailure() {
    
}

function DeleteCancelled() {
    $('#modalValidateDelete').modal('hide');

}

function LoginFailure(jqxhr, status, error) {
    alert(error);
}

//Load the security info from local storage
function LoadFromLocalStorage() {
    authToken = localStorage.authToken;
    displayName = localStorage.displayName;
    userName = localStorage.userName;
}

//Write security info to local storage
function WriteToLocalStorage() {
    localStorage.authToken = authToken;
    localStorage.displayName = displayName;
    localStorage.userName = userName;
}

//Clear security info from storage to log the user out
function LogoutAndClear() {
    authToken = null;
    displayName = null;
    userName = null;

    localStorage.removeItem("authToken");
    localStorage.removeItem("displayName");
    localStorage.removeItem("userName");
}

function SaveAR() {
    $("#AntiResolutionForm").submit();
}

function SaveAntiResolutions() {
    var type = GetType();
    var datStart = $("#dateStart").val();
    var datEnd = $("#dateEnd").val();
    var antiResolution = $("#txtAntiResolution").val();
    var userName = localStorage.userName;
    var authToken = localStorage.authToken;
    var guid = $("#hidGuid").val();
    var isPublic = "False";
    if ($("#chkMakePublic").prop("checked", true)) {
        isPublic = "True";
    }
    
    $('#btnSaveAR').button('disable');

    try {
        var formData = new FormData();
        formData.append('Guid', guid);
        formData.append('Type', type);
        formData.append('Start', datStart);
        formData.append('End', datEnd);
        formData.append('AntiResolution', antiResolution);
        formData.append('IsPublic', isPublic);

        $.ajax({
            type: "POST",
            url: "/addantiresolution",
            headers: { "UserName": userName, "AuthorizationToken": authToken },
            data: formData,
            success: SaveReturn,
            error: SaveError,
            cache: false,
            contentType: false,
            processData: false
        });
    }
    catch (err) {
        alert(err);
        $('#btnSaveAR').button('enable');
    }
}

function SaveReturn() {
    $('#modalSaved').modal({ show: true });
}

function SaveError() {

}

function SaveAcknowledged() {
    window.location.replace("/home");
}