// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function DateTimeToString(datetimefull, format) {
    var Year = datetimefull.substring(0, 4);
    var Month = datetimefull.substring(5, 7);
    var Day = datetimefull.substring(8, 10);
    var HH = datetimefull.substring(11, 13);
    var mm = datetimefull.substring(14, 16);
    var ss = datetimefull.substring(17, 19);
    var result = "";
    var zFormat = format.split(" ");
    var FormatDate = zFormat[0].trim();
    var FormatTime = zFormat[1].trim();

    switch (FormatDate) {
        case "dd/MM/yyyy":
            result = Day + "/" + Month + "/" + Year;
            break;
        case "dd/MM/yy":
            result = Day + "/" + Month + "/" + Year.substring(2,4);
            break;
        case "dd-MM-yyyy":
            result = Day + "-" + Month + "-" + Year;
            break;
        case "dd-MM-yy":
            result = Day + "-" + Month + "-" + Year.substring(2, 4);
            break;
    }

    switch (FormatTime) {
        case "HH:mm":
            result += " " + HH + ":" + mm;
            break;
        case "HH:mm:ss":
            result += " " + HH + ":" + mm + ":" + ss;
            break;
    }
    return result;
}