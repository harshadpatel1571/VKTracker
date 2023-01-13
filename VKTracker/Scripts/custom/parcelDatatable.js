$(document).ready(function () {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var monthDate = new Date();
    var mm = new Date(monthDate.getFullYear(), monthDate.getMonth() + 1, 0).getDate();
    $('#FromDate').val(1 + '-' + months[monthDate.getMonth()] + '-' + monthDate.getFullYear());
    $('#ToDate').val(mm + '-' + months[monthDate.getMonth()] + '-' + monthDate.getFullYear());
});


function bindParcel(data) {
    $('#gridParcel').DataTable({
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, -1], [10, 25, 50, 75, 100, 'All']],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: "/Parcel/GetParcelList/",
            type: "POST",
            datatype: "json",
            data: function (data) {
                data.fromDate = $('#FromDate').val();
                data.toDate = $('#ToDate').val();
            }
        },
        data: data,
        columns: [
            {
                data: "parcelId", name: "Parcel Id", "autoWidth": true,
                render: function (data, type, row) {
                    return row.code + "-" + data;
                }
            },
            { data: "code", name: "Code", "autoWidth": true },
            {
                data: "dishpatchDate", name: "Dishpatch Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            { data: "challanNo", name: "Challan No", "autoWidth": true },
            { data: "transportNo", name: "Transport No", "autoWidth": true },
            {
                data: "challanDate", name: "Challan Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            {
                data: "arrivalDate", name: "Arrival Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            { data: "locationName", name: "Location Name", "autoWidth": true, },
            {
                bSortable: false,
                autoWidth: true,
                sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteParcelRecord(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        "                                    <a class=\"link-success fs-20 sa-warning\" onclick='editParcelRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-edit-2-line\" Title=\"Edit\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteParcelRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindParcelLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#parcelLogModal'>\n" +
                        "                                        <i class=\"ri-history-line\" Title=\"Log History\"></i>\n" +
                        "                                    </a>\n" +
                        "                                </div>";
                }
            },
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            'pageLength'
        ]
    }).buttons().container().appendTo('#parcelHeader');
}

$(document).ready(function () {
    const table = $('#gridParcel').DataTable();
    $('.dataTables_filter input')
        .unbind()
        .bind('input', function (e) {
            if (this.value.length >= 3 || e.keyCode === 13) {
                table.search(this.value).draw();
            }
            if (this.value === "") {
                table.search("").draw();
            }
        });
});


function bindParcelLogGrid(id) {
    $('#gridParcelLog').DataTable().destroy();
    $('#gridParcelLog').DataTable({
        paging: false,
        lengthChange: false,
        searching: false,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, -1], [10, 25, 50, 75, 100, 'All']],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        scrollX: true,
        ajax: {
            url: "/Parcel/GetParcelLogList/" + id,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "action", name: "Action", "autoWidth": true },
            {
                data: "parcelId", name: "Parcel Id", "autoWidth": true,
                render: function (data, type, row) {
                    return row.code + "-" + data;
                }
            },
            { data: "code", name: "Code", "autoWidth": true },
            {
                data: "dishpatchDate", name: "Dishpatch Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            { data: "challanNo", name: "Challan No", "autoWidth": true },
            { data: "transportNo", name: "Transport No", "autoWidth": true },
            {
                data: "challanDate", name: "Challan Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            {
                data: "arrivalDate", name: "Arrival Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            { data: "locationName", name: "Location Name", "autoWidth": true, }
            ,
            { data: "logUserName", name: "Log User Name", "autoWidth": true },
            {
                data: "createdOn", name: "Created On", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            }
        ],
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }
        ]
    }).buttons().container().appendTo('#gridParcelLog_wrapper .col-md-6:eq(0)');
}

$("#addParcel").click(function () {

    if ($("#parcelForm").valid()) {

        event.preventDefault();
        var formData = $("#parcelForm").serialize();
        $.ajax({
            url: "/Parcel/SaveParcelCode/",
            type: "POST",
            data: formData,
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    Swal.fire({
                        timer: 1500,
                        title: "Saved.",
                        text: "Your record has been Saved.",
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        showCancelButton: false,
                        showConfirmButton: false,
                        buttonsStyling: !1
                    }).then(function () {
                        const table = $("#gridParcel").DataTable();
                        table.ajax.reload(null, false);
                        $("#parcelForm #close-modal").click();
                    });
                }
                else {
                    Swal.fire({
                        timer: 1500,
                        title: "Duplicate.",
                        text: response.msg,
                        icon: "error",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        showCancelButton: false,
                        showConfirmButton: false,
                        buttonsStyling: !1
                    })
                }
            },
            error: function (response) {
                alert('Error!');
            },
            complete: function () {
            }
        })
    }
    else {
        return false;
    }
});

function editParcelRecord(id) {
    $.ajax({
        url: "/Parcel/EditParcel/" + id,
        type: "GET",
        success: function (response) {
            if (response.status) {
                console.log(response.data);
                $("#btnParcelModal").click();
                $("#parcelForm #Id").val(response.data.id);
                $("#parcelForm #ParcelId").val(response.data.parcelId);
                $("#parcelForm #LocationId").val(response.data.locationId);
                $("#parcelForm #ChallanNo").val(response.data.challanNo);
                $("#parcelForm #TransportNo").val(response.data.transportNo);
                $("#parcelForm #ChallanDate").val(formateDateYMD(response.data.challanDate));

                $("#parcelForm #DishpatchDate").val(formateDateYMD(response.data.dishpatchDate));
                $("#parcelForm #ArrivalDate").val(formateDateYMD(response.data.arrivalDate));
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
        }
    })
}

function deleteParcelRecord(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: !0,
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        confirmButtonText: "Yes, delete it!",
        buttonsStyling: !1,
        showCloseButton: !0,
    }).then(function (t) {
        if (!t.isConfirmed) return;
        $.ajax({
            type: "POST",
            url: "/Parcel/DeleteParcel/" + id,
            success: function () {
                t.value && Swal.fire({
                    timer: 1500,
                    title: "Deleted.",
                    text: "Your record has been deleted.",
                    icon: "success",
                    showCancelButton: false,
                    showConfirmButton: false,
                    buttonsStyling: !1
                }).then(function () {
                    const table = $("#gridParcel").DataTable();
                    table.ajax.reload(null, false);
                });
            },
            error: function (response) {
                let message = "This entity is being referred somewhere else.";

                if (response.status === 404) {
                    message = "Entity can not be found.";
                }

                t.value && Swal.fire({
                    title: "Unable to delete.",
                    text: message,
                    icon: "warning",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                })
            }
        });
    });
}

$('#parcelModal').on('hidden.bs.modal', function () {
    $("#parcelForm #Id").val("");
    $("#parcelForm #ParcelId-error").text("");
    $("#parcelForm #LocationId-error").text("");
    $("#parcelForm #ChallanNo-error").text("");
    $("#parcelForm #DishpatchDate-error").text("");
    $("#parcelForm #ArrivalDate-error").text("");
    $('form#parcelForm').trigger("reset");
});


$("#btnSearch").click(function () {
    if ($("#frmSearch").valid()) {
        const table = $("#gridParcel").DataTable();
        table.ajax.reload(null, false);
    }
    else {
        return false;
    }
});