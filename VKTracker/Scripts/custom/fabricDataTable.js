﻿function bindFabric() {
    $('#gridFabric').DataTable({
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
            url: "/Master/GetFabricList/",
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "fabricName", name: "FabricName", "autoWidth": true },
            {
                bSortable: false,
                autoWidth: true,
                sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        "                                    <a class=\"link-success fs-20 sa-warning\" onclick='editFabricRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-edit-2-line\" Title=\"Edit\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteFabricRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindFabricLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#fabricLogModal'>\n" +
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
                    columns: [0]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0]
                }
            },
            'pageLength'
        ]
    }).buttons().container().appendTo('#fabricHeader');
}

$(document).ready(function () {
    const table = $('#gridFabric').DataTable();
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

function deleteFabricRecord(id) {
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
            url: "/Master/DeleteFabric/" + id,
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
                    const table = $("#gridFabric").DataTable();
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

function editFabricRecord(id) {
    $.ajax({
        url: "/Master/EditFabric/" + id,
        type: "GET",
        success: function (response) {
            if (response.status) {
                $("#fabricForm #Id").val(response.data.Id);
                $("#fabricForm #FabricName").val(response.data.FabricName);
                $("#btnFabricModal").click();
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
        }
    })
}

$("#addFabric").click(function () {

    if ($("#fabricForm").valid()) {

        event.preventDefault();
        $('#btnSubmit').attr('disabled', 'disabled');
        var formData = $("#fabricForm").serialize();
        $.ajax({
            url: "/Master/SaveFabric/",
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
                        const table = $("#gridFabric").DataTable();
                        table.ajax.reload(null, false);
                        $("#fabricForm #close-modal").click();
                    });
                }
                else {
                    $("#fabricForm #errorName").text("Fabric alrady exist.");
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

$('#fabricModal').on('hidden.bs.modal', function () {
    $("#fabricForm #Id").val("");
    $("#fabricForm #FabricName-error").text("");
    $('form#fabricForm').trigger("reset");
    $("#fabricForm #errorName").text("");
});


function bindFabricLogGrid(id) {
    $('#gridFabricLog').DataTable().destroy();
    $('#gridFabricLog').DataTable({
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
        ajax: {
            url: "/Master/GetFabricLogList/" + id,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "action", name: "Action", "autoWidth": true },
            { data: "fabricName", name: "FabricName", "autoWidth": true },
            { data: "logUserName", name: "LogUserName", "autoWidth": true },
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
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3]
                }
            }
        ]
    }).buttons().container().appendTo('#gridFabricLog_wrapper .col-md-6:eq(0)');
}