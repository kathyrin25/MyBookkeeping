function OnSuccess(response) {

    if (typeof (response.success) == 'undefined' || (response.success) == null)
        alert("新增完成!");
    else if (!response.success)
        alert(response.message);

}
function OnFailure(response) {
    alert("Error occured." + response);
}

$.validator.unobtrusive.adapters.addSingleVal("checkdate", "input");
$.validator.addMethod("checkdate", function (value, element, param) {
    //value: 使用者輸入資料 , param: 預期的資料
    //預期的資料
    if (value == false) {
        return true;
    }
    if ((Date.parse(value)).valueOf() > (Date.parse(param)).valueOf()) {
        return false;
    }
    else {
        return true;
    }
});

$.validator.unobtrusive.adapters.addSingleVal("positiveinteger", "input");
$.validator.addMethod("positiveinteger", function (value, element, param) {
    if (value == false) {
        return true;
    }
    console.log(value);
    console.log(param);
    if (Number(value) <= Number(param)) {
        return false;
    }
    else {
        return true;
    }
});
