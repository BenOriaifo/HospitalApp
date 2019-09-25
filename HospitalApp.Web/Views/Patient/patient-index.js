$(document).ready(function () {
    $("#patientGrid").DataTable({

        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 10,

        "ajax": {
            "url": "/Patient/LoadData",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
        [{
            "targets": [0],
            "searchable": true
        },
        {
            "targets": [1],
            "searchable": true
        },
        {
            "targets": [2],
            "searchable": true
        },
        {
            "targets": [3],
            "searchable": true
        },
        {
            "targets": [4],
            "searchable": true
        }],

        "columns": [
              { "data": "First Name", "name": "FirstName", "autoWidth": true },
              { "data": "Last Name", "name": "LastName", "autoWidth": true },
              { "data": "Phone", "title": "Phone", "name": "ContactName", "autoWidth": true },
              { "data": "Patient ID", "name": "PatientID", "autoWidth": true },
              { "data": "Status", "name": "Status", "autoWidth": true },
              {
                  "render": function (data, type, full, meta)
                  { return '<a class="btn btn-info" href="/Patient/Edit/' + full.CustomerID + '">Edit</a>'; }
              },
               {
                   data: null, render: function (data, type, row) {
                       return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.CustomerID + "'); >Delete</a>";
                   }
               },

        ]

    });
});