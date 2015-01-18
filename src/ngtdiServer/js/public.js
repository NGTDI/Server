
function GetPublicAntiResolutions() {
    $.ajax({
        type: "GET",
        url: "/rest/recentfeed",
        success: GetPublicAntiResolutionsReceived,
        error: GetPublicAntiResolutionsFailure,
        cache: false,
        contentType: false,
        processData: false
    });
}

function GetPublicAntiResolutionsFailure() {

}

function GetPublicAntiResolutionsReceived(response) {
    $('#ars_table').dataTable({
        data: response.Data,
        columns: [{ "data": "Period", "width": "15%" },
                  { "data": "Text", "width": "85%"}]
    });
}