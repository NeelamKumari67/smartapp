  
    $(document).ready(function() {  
        $("#example").DataTable({  
            "processing": true, // for show progress bar    
            "serverSide": true, // for process server side    
            "filter": true, // this is for disable filter (search box)    
            "orderMulti": false, // for disable multiple column at once    
            "ajax": {  
                "url": "/DemoGrid/LoadData",  
                "type": "POST",  
                "datatype": "json"  
            },  
            "columnDefs": [{  
                "targets": [0],  
                "visible": false,  
                "searchable": false  
            }],  
            "columns": [  
                { "data": "customerID", "name": "customerID", "autoWidth": true },
                { "data": "name", "name": "name", "autoWidth": true },
                { "data": "address", "name": "address", "autoWidth": true },
                { "data": "country", "name": "country", "autoWidth": true },
                { "data": "city", "name": "city", "autoWidth": true },
                { "data": "phoneno", "name": "phoneno", "autoWidth": true },
                { "data": "email", "name": "email", "autoWidth": true },
                { "data": "pincode", "name": "pincode", "autoWidth": true },
                {  
                    "render": function(data, type, full, meta) { return '<a class="btn btn-info" href="/DemoGrid/Edit/' + full.CustomerID + '">Edit</a>'; }  
                },  
                {  
                    data: null,  
                    render: function(data, type, row) {  
                        return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";  
                    }  
                },  
            ]  
  
        });  
    });  
  
  
function DeleteData(CustomerID) {  
    if (confirm("Are you sure you want to delete ...?")) {  
        Delete(CustomerID);  
    } else {  
        return false;  
    }  
}  
  
  
function Delete(CustomerID) {  
    var url = '@Url.Content("~/")' + "DemoGrid/Delete";  
  
    $.post(url, { ID: CustomerID }, function(data) {  
        if (data) {  
            oTable = $('#example').DataTable();  
            oTable.draw();  
        } else {  
            alert("Something Went Wrong!");  
        }  
    });  
}  
  