$(document).ready(function () {
    GetCustomerList();
    //ExportToPDF();
    //getRootUrl();

});

function GetCustomerList() {
    $.ajax({
        url: '/DemoGrid/GetCustomerList',
        type: 'Get',
        datatype: 'json',
        success: OnSuccess
    })
}

function OnSuccess(response) {
    debugger
    $('#dataTableData').DataTable({
        bProcessing: true,
        bLengthChange: true,
        lengthManu: [[5, 10, 25, -1], [5, 10, 25, "All"]],
        bfilter: true,
        bSort: true,
        bPaginate: true,
        data: response,
        columns: [
            {
                data: 'CustomerID',
                render: function (data, type, row, meta) {
                    return row.customerID
                }
            },
            {
                data: 'FirstName',
                render: function (data, type, row, meta) {
                    return row.firstName
                }
            },
            {
                data: 'LastName',
                render: function (data, type, row, meta) {
                    return row.lastName
                }
            },
            {
                data: 'Address',
                render: function (data, type, row, meta) {
                    return row.address
                }
            },
            {
                data: 'Country',
                render: function (data, type, row, meta) {
                    return row.country
                }
            },
            {
                data: 'City',
                render: function (data, type, row, meta) {
                    return row.city
                }
            },
            {
                data: 'state',
                render: function (data, type, row, meta) {
                    return row.state
                }
            },
            {
                data: 'Phoneno',
                render: function (data, type, row, meta) {
                    return row.phoneno
                }
            },
            {
                data: 'Email',
                render: function (data, type, row, meta) {
                    return row.email
                }
            },
            {
                data: 'Pincode',
                render: function (data, type, row, meta) {
                    return row.pincode
                }
            },
            {
                render: function (data, type, row) {
                    return "<a href='#' onclick=Edit('" + row.customerID + "'); style='display: flex; align-items: center;'>" +
                        "<i class='fa-solid fa-pen-to-square' style='color:info;'></i>" +
                        "</a>";
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return "<a href='#' onclick=Delete('" + row.customerID + "'); style='display: flex; align-items: center;'>" +
                        "<i class='fa-sharp fa-solid fa-trash' style='color: red;'></i>" +
                        "</a>";
                }
            },
        ]
    });
}

//function ExportToPDF() {
//    var exportURL = getRootUrl() + "DemoGrid/ExportPDF";
//    window.location.href = exportURL;
//}

//function getRootUrl() {
//    return window.location.origin ? window.location.origin + "/" : window.location.protocol + "//" + window.location.host + "/";
//}
function AddEmployee() {
    debugger
    var objdata = {
        FirstName: $('#fname').val(),
        LastName: $('#lname').val(),
        Address: $('#address').val(),
        Country: $('#country').val(),
        State: $('#state').val(),
        City: $('#city').val(),
        Phoneno: $('#pnumber').val(),
        Email: $('#email').val(),
        Pincode: $('#pincode').val()

    }
    $.ajax({
        url: '/DemoGrid/AddEmployee',
        type: 'post',
        data: objdata,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        datatype: 'json',
        success: function () {
            alert("data saved");
            GetCustomerList();
            HideModelPopUp();
            ClearTextBox();
        },
        error: function () {
            alert("data can't saved!");
        }
    });
}
function Edit(id) {
    debugger
    $.ajax({
        url: '/DemoGrid/Edit?id=' + id,
        type: 'Get',
        contentType: "application/json;charset=utf-8",
        datatype: 'json',
        success: function (response) {
            debugger
            $('#demoModal').modal('show');
            $('#customerId').val(response.customerID);
            $('#fname').val(response.firstName);
            $('#lname').val(response.lastName);
            $('#address').val(response.address);
            $('#country').val(response.country);
            $('#state').val(response.state);
            $('#city').val(response.city);
            $('#pnumber').val(response.phoneno);
            $('#email').val(response.email);
            $('#pincode').val(response.pincode);
            $('#AddEmployee').css('display', 'none');
            $('#btnUpdate').css('display', 'block');

        },
        error: function () {
            alert('data not found');
        }
    });
}
function HideModelPopUp() {
    $('#demoModal').modal('hide');
}

function ClearTextBox() {
    $('#customerId').val('');
    $('#fname').val('');
    $('#lname').val('');
    $('#address').val('');
    $('#country').val('');
    $('#state').val('');
    $('#city').val('');
    $('#pnumber').val('');
    $('#email').val('');
    $('#pincode').val('');
}
function Delete(id) {
    debugger
    $.ajax({
        url: '/DemoGrid/Delete?id=' + id,
        success: function () {
            alert('Record Deleted! ');
            GetCustomerList();
        },
        error: function () {
            alert("Data can't be deleted!");
        }
    })
}

function Update() {
    debugger
    var objdata = {
        CustomerID: $('#customerId').val(),
        FirstName: $('#fname').val(),
        LastName: $('#lname').val(),
        Address: $('#address').val(),
        Country: $('#country').val(),
        State: $('#state').val(),
        City: $('#city').val(),
        Phoneno: $('#pnumber').val(),
        Email: $('#email').val(),
        Pincode: $('#pincode').val()

    }
    $.ajax({
        url: '/DemoGrid/Update',
        type: 'post',
        data: objdata,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        datatype: 'json',
        success: function () {
            alert("data updated");
            GetCustomerList();
            HideModelPopUp();
            ClearTextBox();
        },
        error: function () {
            alert("data can't updated!");
        }
    });

}
