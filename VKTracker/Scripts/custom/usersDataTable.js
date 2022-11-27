function bindUsers() {
    $('#gridUser').DataTable({
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
            url: "/Master/GetUsersGrid/",
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "firstName", name: "First Name", "autoWidth": true },
            { data: "lastName", name: "Last Name", "autoWidth": true },
            { data: "userName", name: "User Name", "autoWidth": true },
            { data: "emailId", name: "Email Id", "autoWidth": true },
            { data: "mobileNo", name: "Mobile No", "autoWidth": true },
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
                        "                                    <a class=\"link-success fs-20 sa-warning\" onclick='editUserRecourd(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteUserRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindUserLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#userLogModal'>\n" +
                        "                                        <i class=\"ri-database-2-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                </div>";
                }
            },
        ],
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4]
                }
            }
        ]
    }).buttons().container().appendTo('#gridUser_wrapper .col-md-6:eq(0)');
}

$(document).ready(function () {
    const table = $('#gridUser').DataTable();
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

function deleteUserRecord(id) {
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
            url: "/Master/DeleteUser/" + id,
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
                    const table = $("#gridUser").DataTable();
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

function editUserRecourd(id) {
    $.ajax({
        url: "/Master/EditUser/" + id,
        type: "GET",
        success: function (response) {
            console.log(response.data);
            if (response.status) {
                $("#userForm #Id").val(response.data.Id);
                $("#userForm #FirstName").val(response.data.FirstName);
                $("#userForm #LastName").val(response.data.LastName);
                $("#userForm #UserName").val(response.data.UserName);
                $("#userForm #Password").val(response.data.Password);
                $("#userForm #EmailId").val(response.data.EmailId);
                $("#userForm #MobileNo").val(response.data.MobileNo);
                $("#userForm #OrganizationId").val(response.data.OrganizationId);
                $("#btnUserModal").click();
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
            $('#btnSubmit').removeAttr('disabled');
        }
    })
}

$("#addUser").click(function () {

    if ($("#userForm").valid()) {

        event.preventDefault();
        $('#btnSubmit').attr('disabled', 'disabled');
        var formData = $("#userForm").serialize();
        $.ajax({
            url: "/Master/SaveUser/",
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
                        const table = $("#gridUser").DataTable();
                        table.ajax.reload(null, false);
                        $("#userForm #close-modal").click();
                    });
                }
                else {
                    $("#userForm #errorName").text("User name alrady exist.");
                }
            },
            error: function (response) {
                alert('Error!');
            },
            complete: function () {
                $('#btnSubmit').removeAttr('disabled');
            }
        })
    }
    else {
        return false;
    }
});

$('#userModal').on('hidden.bs.modal', function () {
    $("#userForm #Id").val("");
    $("#userForm #FirstName-error").text("");
    $("#userForm #UserName-error").text("");
    $("#userForm #Password-error").text("");
    $("#userForm #OrganizationId-error").text("");
    $('form#userForm').trigger("reset");
    $("#userForm #errorName").text("");
});

function BindOrganization() {
    $.ajax({
        url: "/Master/BindOrganizationDropdown/",
        type: "POST",
        dataType: "json",
        success: function (response) {
            if (response.status) {
                console.log(response.data)
                $("#OrganizationId").empty();
                $.each(response.data, function (index, key) {
                    $("#OrganizationId").append(`<option value='${key.Value}'>${key.Text} </option>`);
                });
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
        }
    })
}

function bindUserLogGrid(id) {
    $('#gridUserLog').DataTable().destroy();
    $('#gridUserLog').DataTable({
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
            url: "/Master/GetUserLogList/" + id,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "action", name: "Action", "autoWidth": true },
            { data: "userName", name: "User Name", "autoWidth": true },
            { data: "firstName", name: "User Name", "autoWidth": true },
            { data: "lastName", name: "User Name", "autoWidth": true },
            { data: "emailId", name: "User Name", "autoWidth": true },
            { data: "mobileNo", name: "User Name", "autoWidth": true },
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
    }).buttons().container().appendTo('#gridUserLog_wrapper .col-md-6:eq(0)');
}